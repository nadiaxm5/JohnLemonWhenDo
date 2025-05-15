using B83.LogicExpressionParser;
using UnityEngine;
using System;
using System.Collections.Generic;
public static class Condition {
    public static bool Compare(string a, Dictionary<string, GameObject> scopeList) {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList)
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        return (parser.Parse(a).GetResult());
    }
    public static bool Collision(string tag, GameObject obj) {
        var script = obj.GetComponent(typeof(MonoBehaviour));
        Dictionary<string, bool> Tags = (Dictionary<string, bool>)script.GetType().GetField("Tags").GetValue(script);
        return (Tags[tag]);
    }
    public static bool Keyboard(string key, string keyMode) {
        KeyCode k = (KeyCode)Enum.Parse(typeof(KeyCode), key);
        switch (keyMode) {
            case "Press": return Input.GetKey(k);
            case "Down": return Input.GetKeyDown(k);
            case "Up": return Input.GetKeyUp(k);
            default: break;
        }
        return false;
    }
    public static bool Mouse(string type, GameObject obj) {
        var script = obj.GetComponent(typeof(MonoBehaviour));
        bool value = (bool)script.GetType().GetField("Mouse" + type).GetValue(script);
        script.GetType().GetField("Mouse" + type).SetValue(script, false);
        return value;
    }
}
