using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
public static class Utils {
    public static float GetProperty(KeyValuePair<string, GameObject> s) {
        string[] elements = s.Key.Split(new string[] { "." }, StringSplitOptions.None);
        GameObject obj = s.Value;
        float value = float.NaN;
        if (obj != null) { // if the object exist, it is not deleted
            switch (elements[1]) {
                case "x": value = obj.transform.position.x; break;
                case "y": value=obj.transform.position.y; break;
                case "z": value=obj.transform.position.z; break;
                case "rx": value=obj.transform.eulerAngles.x; break;
                case "ry": value=obj.transform.eulerAngles.y; break;
                case "rz": value=obj.transform.eulerAngles.z; break;
                case "sx": value=obj.transform.localScale.x; break;
                case "sy": value=obj.transform.localScale.y; break;
                case "sz": value=obj.transform.localScale.z; break;
                default: {// search on script properties
                    var script = obj.GetComponent(obj.name);
                    value = (float)script.GetType().GetField(elements[1]).GetValue(script);break;
                }
            }
        }
        return (value);
    }
    public static void SetProperty(string property, float value, GameObject obj) {
        string[] elements = property.Split(new string[] { "." }, StringSplitOptions.None);
        if (obj != null) {
            switch (elements[1]) {
                case "x": obj.transform.position = new Vector3(value, obj.transform.position.y, obj.transform.position.z); break;
                case "y": obj.transform.position = new Vector3(obj.transform.position.x, value, obj.transform.position.z); break;
                case "z": obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, value); break;
                case "rx": obj.transform.eulerAngles = new Vector3(value, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z); break;
                case "ry": obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, value, obj.transform.eulerAngles.z); break;
                case "rz": obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, value); break;
                case "sx": obj.transform.localScale = new Vector3(value, obj.transform.localScale.y, obj.transform.localScale.z); break;
                case "sy": obj.transform.localScale = new Vector3(obj.transform.localScale.x, value, obj.transform.localScale.z); break;
                case "sz": obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, value); break;
                default: {// search on script properties
                        var script = obj.GetComponent(obj.name);
                        script.GetType().GetField(elements[1]).SetValue(script, value);break;
                }
            }
        }
    }
    public static Dictionary<string, GameObject> CreateScope(int objID, string scope) {
        Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<string> transformProperties = new List<string> { "x", "y", "z", "rx", "ry", "rz", "sx", "sy", "sz" };
        foreach (GameObject obj in allObjects) {
            if (obj.transform.parent == null) { // we are only interested in gameobjects without parent
                var script = obj.GetComponent(obj.name);
                List<string> properties = transformProperties;
                if (script != null && script.GetType().GetField("propertyList") != null) {
                    Dictionary<string, float> propertyList = (Dictionary<string, float>)script.GetType().GetField("propertyList").GetValue(script);
                    properties.AddRange(propertyList.Keys);
                }
                properties.Distinct().ToList();
                foreach (string p in properties) { // Add object properties
                    if (scope.Contains(obj.name + "." + p) && !scopeList.ContainsKey(obj.name + "." + p)) scopeList.Add(obj.name + "." + p, obj);
                }
                if (obj.GetInstanceID() == objID) { // Add scope properties
                    foreach (string p in properties) {
                        if (scope.Contains("this." + p) && !scopeList.ContainsKey("this." + p)) scopeList.Add("this." + p, obj);
                    }
                }
            }
        }

        return (scopeList);
    }
    public static Dictionary<string, GameObject> CreateObjectList(string s) {
        Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (s.Contains(obj.name) && !objectList.ContainsKey(obj.name)) {
                objectList.Add(obj.name, obj);
            }
        }
        return (objectList);
    }
    public static Dictionary<string, float> CreateProperties(string s) {
        Dictionary<string, float> properties = new Dictionary<string, float>();
        if (s != "") {
            var asign = s.Split(new string[] { ";" }, StringSplitOptions.None);
            foreach (string a in asign) {
                var elements = a.Split(new string[] { "=" }, StringSplitOptions.None);
                if (!properties.ContainsKey(elements[0]))
                    properties.Add(elements[0], float.Parse(elements[1]));
            }
        }
        return (properties);
    }
}
