using B83.LogicExpressionParser;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public static class Action {
    public static void Edit(string property, string valueExp, Dictionary<string, GameObject> scopeList) {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList) {
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        }
        float value = (float)parser.ParseNumber(valueExp).GetNumber();
        GameObject obj = scopeList[property];
        Utils.SetProperty(property,value,obj);
    }
    public static void Delete(GameObject obj) {
        Object.Destroy(obj);
    }
    public static void Spawn(GameObject objToSpawn, GameObject me) {
        GameObject newObj = Object.Instantiate(objToSpawn);
        newObj.SetActive(true); // Active the spawned object y the Active variable below
        var scriptType = System.Type.GetType(objToSpawn.name);
        if (scriptType != null) {
            var script = newObj.GetComponent(scriptType);
            if (script.GetType().GetField("propertyList") != null) {
                Dictionary<string, float> propertyList = (Dictionary<string, float>)script.GetType().GetField("propertyList").GetValue(script);
                foreach (KeyValuePair<string, float> property in propertyList)
                    script.GetType().GetField(property.Key).SetValue(script, property.Value);
            }
            if (script.GetType().GetField("Active")!=null) script.GetType().GetField("Active").SetValue(script, true); // Active variable set to true
        }
        newObj.transform.position = new Vector3(me.transform.position.x, me.transform.position.y, me.transform.position.z);
        newObj.transform.eulerAngles = new Vector3(me.transform.eulerAngles.x, me.transform.eulerAngles.y, me.transform.eulerAngles.z);
        newObj.transform.localScale = new Vector3(me.transform.localScale.x, me.transform.localScale.y, me.transform.localScale.z);
    }
    public static void Active(GameObject obj) {
        obj.SetActive(true);
    }
    public static void Inactive(GameObject obj) {
        obj.SetActive(false);
    }
    public static void Animation(string state, GameObject obj) {
        obj.GetComponent<Animator>().SetInteger("State", int.Parse(state));
    }
    public static void Move(string angleExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList) {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList)
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        float angle = (float)parser.ParseNumber(angleExp).GetNumber() * Mathf.Deg2Rad;
        float speed = (float)parser.ParseNumber(speedExp).GetNumber();
        Utils.SetProperty("this.x", obj.transform.position.x + speed * Mathf.Cos(angle) * Time.deltaTime, obj);
        Utils.SetProperty("this.z", obj.transform.position.z + speed * Mathf.Sin(angle) * Time.deltaTime, obj);
    }
    public static void MoveTo(string xExp, string zExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList) {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList) 
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        float x = (float)parser.ParseNumber(xExp).GetNumber();
        float z = (float)parser.ParseNumber(zExp).GetNumber();
        float speed = (float)parser.ParseNumber(speedExp).GetNumber();
        Utils.SetProperty("this.x", obj.transform.position.x + speed * (x - obj.transform.position.x) * Time.deltaTime, obj);
        Utils.SetProperty("this.z", obj.transform.position.z + speed * (z - obj.transform.position.z) * Time.deltaTime, obj);
    }
    public static void NavigateTo(string xExp, string zExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList) {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList) 
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        float x = (float)parser.ParseNumber(xExp).GetNumber();
        float z = (float)parser.ParseNumber(zExp).GetNumber();
        float speed = (float)parser.ParseNumber(speedExp).GetNumber();
        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.SetDestination(new Vector3(x, obj.transform.position.y, z));
    }
    public static void LoadScene() {
        SceneManager.LoadScene(0);
    }
    public static void QuitGame() {
        Application.Quit();
    }
    public static void PlaySound(string audioClip, GameObject obj) {
        AudioSource[] audios = obj.GetComponents<AudioSource>();
        foreach (AudioSource audio in audios) 
            if(audio.clip.name==audioClip && !audio.isPlaying) audio.Play();
    }
    public static void StopSound(string audioClip, GameObject obj) {
        AudioSource[] audios= obj.GetComponents<AudioSource>();
        foreach (AudioSource audio in audios)
            if(audio.clip.name==audioClip) audio.Stop();
    }
}
