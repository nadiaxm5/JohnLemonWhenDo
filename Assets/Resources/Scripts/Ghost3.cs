using UnityEngine;
using System.Collections.Generic;

public class Ghost3 : MonoBehaviour {
    public bool Active = true;
    public float delta=0.5f;
    public float speed=1.0f;
    public float xObj=3.2f;
    public float zObj=12.3f;
    public float x0=3.2f;
    public float x1=6.5f;
    public float z0=5.7f;
    public float z1=12.3f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    void FixedUpdate(){
        {
            Action.NavigateTo("this.speed","this.xObj","this.y","this.zObj",gameObject,scopeList);
        }
        if(Condition.Compare("(abs(this.z-this.z1)<this.delta) && (abs(this.x-this.x0)<this.delta)",scopeList)){
            Action.Edit("this.xObj","this.x1",scopeList);
        }
        if(Condition.Compare("(abs(this.z-this.z1)<this.delta) && (abs(this.x-this.x1)<this.delta)",scopeList)){
            Action.Edit("this.zObj","this.z0",scopeList);
        }
        if(Condition.Compare("(abs(this.z-this.z0)<this.delta) && (abs(this.x-this.x1)<this.delta)",scopeList)){
            Action.Edit("this.xObj","this.x0",scopeList);
        }
        if(Condition.Compare("(abs(this.z-this.z0)<this.delta) && (abs(this.x-this.x0)<this.delta)",scopeList)){
            Action.Edit("this.zObj","this.z1",scopeList);
        }
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"NavigateTo(this.speed,this.xObj,this.y,this.zObj);(abs(this.z-this.z1)<this.delta) && (abs(this.x-this.x0)<this.delta);Edit(this.xObj,this.x1);(abs(this.z-this.z1)<this.delta) && (abs(this.x-this.x1)<this.delta);Edit(this.zObj,this.z0);(abs(this.z-this.z0)<this.delta) && (abs(this.x-this.x1)<this.delta);Edit(this.xObj,this.x0);(abs(this.z-this.z0)<this.delta) && (abs(this.x-this.x0)<this.delta);Edit(this.zObj,this.z1)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("delta=0.5;speed=1.0;xObj=3.2;zObj=12.3;x0=3.2;x1=6.5;z0=5.7;z1=12.3");
    }
}
