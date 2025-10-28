using UnityEngine;
using System.Collections.Generic;

public class Ghost2 : MonoBehaviour {
    public bool Active = true;
    public float speed=1.0f;
    public float zObj=7.6f;
    public float z0=-3.5f;
    public float z1=7.6f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    void FixedUpdate(){
        {
            Action.NavigateTo("this.speed","this.x","this.y","this.zObj",gameObject,scopeList);
        }
        if(Condition.Compare("this.z>=this.z1",scopeList)){
            Action.Edit("this.zObj","this.z0",scopeList);
        }
        if(Condition.Compare("this.z<=this.z0",scopeList)){
            Action.Edit("this.zObj","this.z1",scopeList);
        }
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"NavigateTo(this.speed,this.x,this.y,this.zObj);this.z>=this.z1;Edit(this.zObj,this.z0);this.z<=this.z0;Edit(this.zObj,this.z1)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("speed=1.0;zObj=7.6;z0=-3.5;z1=7.6");
    }
}
