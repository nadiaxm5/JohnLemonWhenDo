using B83.LogicExpressionParser;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Action
{
    public static void Edit(string property, string valueExp, Dictionary<string, GameObject> scopeList)
    {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList)
        {
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        }
        float value = (float)parser.ParseNumber(valueExp).GetNumber();
        GameObject obj = scopeList[property];
        Utils.SetProperty(property, value, obj);
    }

    //Modificado
    public static void Spawn(string prefabName, GameObject me)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        GameObject newObj = Object.Instantiate(prefab);
        newObj.name = prefab.name;
        newObj.SetActive(true);
        System.Type scriptType = System.Type.GetType(prefabName);

        if (scriptType != null && scriptType.IsSubclassOf(typeof(MonoBehaviour)))
        {
            var script = newObj.AddComponent(scriptType);
            scriptType.GetField("Active").SetValue(script, true);

            var propListField = scriptType.GetField("propertyList");
            if (propListField != null)
            {
                Dictionary<string, float> propertyList = (Dictionary<string, float>)propListField.GetValue(script);
                if (propertyList != null)
                {
                    foreach (var pair in propertyList)
                        scriptType.GetField(pair.Key).SetValue(script, pair.Value);
                }
            }
        }

        newObj.transform.position = me.transform.position;
        newObj.transform.eulerAngles = me.transform.eulerAngles;
        newObj.transform.localScale = prefab.transform.localScale;

        // Lógica especial si tiene LineRenderer
        LineRenderer lr = newObj.GetComponent<LineRenderer>();
        if (lr != null)
        {
            Vector3 start = newObj.transform.position;
            float angle = newObj.transform.eulerAngles.y * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            Vector3 end = start + dir * 100f;
            if (Physics.Raycast(start, dir, out RaycastHit hit, 100f)) end = hit.point;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }
    }

    public static void Animation(string state, GameObject obj)
    {
        obj.GetComponent<Animator>().SetInteger("State", int.Parse(state));
    }

    public static void Move(string angleExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList)
    {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList)
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        float angle = (float)parser.ParseNumber(angleExp).GetNumber() * Mathf.Deg2Rad;
        float speed = (float)parser.ParseNumber(speedExp).GetNumber();
        Utils.SetProperty("this.x", obj.transform.position.x + speed * Mathf.Sin(angle) * Time.deltaTime, obj); //Intercambiado Sin x Cos z
        Utils.SetProperty("this.z", obj.transform.position.z + speed * Mathf.Cos(angle) * Time.deltaTime, obj);
    }

    public static void MoveTo(string xExp, string zExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList)
    {
        Parser parser = new Parser();
        foreach (KeyValuePair<string, GameObject> s in scopeList)
            parser.ExpressionContext[s.Key].Set(Utils.GetProperty(s));
        float x = (float)parser.ParseNumber(xExp).GetNumber();
        float z = (float)parser.ParseNumber(zExp).GetNumber();
        float speed = (float)parser.ParseNumber(speedExp).GetNumber();
        Utils.SetProperty("this.x", obj.transform.position.x + speed * (x - obj.transform.position.x) * Time.deltaTime, obj);
        Utils.SetProperty("this.z", obj.transform.position.z + speed * (z - obj.transform.position.z) * Time.deltaTime, obj);
    }

    public static void NavigateTo(string xExp, string zExp, string speedExp, GameObject obj, Dictionary<string, GameObject> scopeList)
    {
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

    public static void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void PlaySound(string audioClip, GameObject obj)
    {
        AudioSource[] audios = obj.GetComponents<AudioSource>();
        foreach (AudioSource audio in audios)
            if (audio.clip.name == audioClip && !audio.isPlaying) audio.Play();
    }

    public static void StopSound(string audioClip, GameObject obj)
    {
        AudioSource[] audios = obj.GetComponents<AudioSource>();
        foreach (AudioSource audio in audios)
            if (audio.clip.name == audioClip) audio.Stop();
    }

    //Nuevo
    public static void Delete(GameObject me)
    {
        Utils.RemoveFromCollisions(me);
        Object.Destroy(me);
    }

    public static void UpdateMousePosition()
    {
        var screen = GameObject.Find("MouseScreen");
        var world = GameObject.Find("MouseWorld");
        if (screen == null || world == null) return;

        screen.transform.position = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (new Plane(Vector3.up, Vector3.zero).Raycast(ray, out float enter))
            world.transform.position = ray.GetPoint(enter);
    }

    public static void LookAt(string targetName, GameObject obj)
    {
        var target = GameObject.Find(targetName);
        if (target == null || obj == null) return;

        Vector3 dir = target.transform.position - obj.transform.position;
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        obj.transform.rotation = Quaternion.LookRotation(dir);

        Utils.SetProperty(obj.name + ".ry", obj.transform.eulerAngles.y, obj);
    }
}