using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
public static class Scripts {
    public static void Create(List<ActorJson> actorList) {
        Directory.Delete("Assets/Resources/Scripts/", true);
        Directory.CreateDirectory("Assets/Resources/Scripts/");
        foreach (ActorJson actor in actorList) {
            // if (actor.Script.Any()) {
            List<string> tags = new List<string>();
            List<string> mouseEvents = new List<string>();
            List<string> scope = new List<string>();
            List<string> spawns = new List<string>();
            List<string> properties = new List<string>();
            string scriptsPath = "Assets/Resources/Scripts/" + actor.Name + ".cs";
            StreamWriter outfile = new StreamWriter(scriptsPath);
            // Add Header
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("");
            outfile.WriteLine("public class " + actor.Name + " : MonoBehaviour {");
            // Add New Properties
            outfile.WriteLine("    public bool Active = " + actor.Active.ToString().ToLower() + ";");
            foreach (string p in actor.Properties) {
                properties.Add(p);
                outfile.WriteLine("    public float " + p + "f;");
            }
            // Add Update 
            outfile.WriteLine("    void FixedUpdate(){");
            foreach (SentenceJson s in actor.Script) {
                if (s.When.Any()) {
                    outfile.Write("        if(");
                    foreach (string c in s.When) {
                        string newC = c;
                        if (c.Contains("Collision")) tags.Add(StringToElement(c));
                        else if (c.Contains("Mouse")) mouseEvents.Add(StringToElement(c));
                        else if (!c.Contains("Keyboard")) { // if not a Keyboard condition is a Compare condition
                            scope.Add(c);
                            newC = "Compare(" + c + ")";
                        }
                        outfile.Write("Condition." + StringToCommand(newC));
                        if (s.When.Last() != c) outfile.Write(" && ");
                    }
                    outfile.WriteLine("){");
                }
                else outfile.WriteLine("        {");
                foreach (string a in s.Do) {
                    string newA = a;
                    if (a.Contains("=")) {
                        var elements = a.Split(new string[] { "=" }, StringSplitOptions.None);
                        newA = "Edit(" + elements[0] + "," + elements[1] + ")";
                        scope.Add(newA);
                    }
                    else if (a.Contains("Spawn") || a.Contains("Active") || a.Contains("Inactive")) spawns.Add(StringToElement(newA));
                    else if (a.Contains("Move") || a.Contains("NavigateTo")) scope.Add(a);
                    outfile.WriteLine("                Action." + StringToCommand(newA) + ";");
                }
                outfile.WriteLine("        }");
            }
            outfile.WriteLine("    }");
            // Add Awake
            string joinProperties = string.Join(";", properties);
            string joinSpawns = string.Join(",", spawns);
            if (joinProperties.Length != 0 || joinSpawns.Length!=0) {
                 outfile.WriteLine("    public Dictionary<string, float> propertyList = new Dictionary<string, float>();");
                if (joinSpawns.Length != 0) outfile.WriteLine("    public Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();");
                outfile.WriteLine("    void Awake() {");
                 outfile.WriteLine("        propertyList = Utils.CreateProperties(\"" + joinProperties + "\");");
                if (joinSpawns.Length != 0) outfile.WriteLine("        objectList = Utils.CreateObjectList(\"" + joinSpawns + "\");");
                outfile.WriteLine("    }");
            }
            // Add Start
            string joinScope = string.Join(";", scope);
            if (joinScope.Length != 0) outfile.WriteLine("    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();");
            outfile.WriteLine("    void Start() {");
            if (joinScope.Length != 0) outfile.WriteLine("        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),\"" + joinScope + "\");");
            outfile.WriteLine("        if (Active) gameObject.SetActive(true);"); 
            outfile.WriteLine("        else gameObject.SetActive(false);");
            outfile.WriteLine("    }");
            // Add OnTriggerEnter and OnTriggerExit
            if (tags.Any()) {
                tags = tags.Distinct().ToList();
                outfile.Write("    public Dictionary<string,bool> Tags = new Dictionary<string,bool>{");
                foreach (string t in tags) {
                    outfile.Write("{\"" + t + "\", false }");
                    if (tags.Last() != t) outfile.Write(",");
                }
                outfile.WriteLine("};");
                outfile.WriteLine("    void OnTriggerEnter (Collider other) {");
                foreach (string t in tags) {
                    outfile.WriteLine("        if (other.CompareTag(\"" + t + "\")) Tags[\"" + t + "\"]=true;");
                }
                outfile.WriteLine("    }");
                outfile.WriteLine("    void OnTriggerExit (Collider other) {");
                foreach (string t in tags) {
                    outfile.WriteLine("        if (other.CompareTag(\"" + t + "\")) Tags[\"" + t + "\"]=false;");
                }
                outfile.WriteLine("    }");
            }
            // Add onMouseEvents
            if (mouseEvents.Any()) {
                foreach (string e in mouseEvents) {
                    outfile.WriteLine("    public bool Mouse" + e + " = false;");
                    outfile.WriteLine("    void OnMouse" + e + "(){");
                    outfile.WriteLine("        Mouse" + e + "=true;");
                    outfile.WriteLine("    }");
                }
            }
            outfile.WriteLine("}");
            outfile.Close();
        }
        AssetDatabase.Refresh();
    }
    private static string StringToCommand(string element) {// traslate a game.json comand into a valid unity command
        int init = element.IndexOf("(");
        int end = element.LastIndexOf(")");
        string name = element.Substring(0, init);
        string command = name;
        string rest = element.Substring(init + 1, end - init - 1);
        string[] parameters = rest.Split(new string[] { "," }, StringSplitOptions.None);
        command += "(";
        int counter = 0;
        foreach (string s in parameters) {
            counter++;
            command += "\"" + s + "\"";
            if (parameters.Length != counter) command += ",";
        }
        if (name == "Compare" || name == "Edit") command += ",scopeList)";
        else if (name == "Move" || name == "MoveTo" || name == "NavigateTo") command += ",gameObject,scopeList)";
        else if (name == "Collision" || name == "Mouse" || name == "Animation" || name == "PlaySound" || name == "StopSound") command += ",gameObject)";
        else if (name == "Keyboard") command += ")";
        else if (name == "Spawn") command = "Spawn(objectList[\"" + parameters[0] + "\"],gameObject)";
        else if (name == "Active") command = "Active(objectList[\"" + parameters[0] + "\"])";
        else if (name == "Inactive") command = "Inactive(objectList[\"" + parameters[0] + "\"])";
        else if (name == "Delete") command = "Delete(gameObject)";
        else if (name == "QuitGame" || name == "LoadScene") command = name + "()";
        return (command);
    }
    private static string StringToElement(string element) {
        int init = element.IndexOf("(");
        int end = element.LastIndexOf(")");
        string tag = element.Substring(init + 1, end - init - 1);
        return (tag);
    }
}
