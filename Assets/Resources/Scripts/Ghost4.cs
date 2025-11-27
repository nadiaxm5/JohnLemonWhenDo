using UnityEngine;
using System.Collections.Generic;

public class Ghost4 : MonoBehaviour {
    public bool Active = true;
    public float delta=0.5f;
    public float speed=1.0f;
    public float xObj=7.4f;
    public float zObj=-2.0f;
    public float x0=3.2f;
    public float x1=7.4f;
    public float z0=-5f;
    public float z1=-2f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    private Dictionary<string, float> timers = new Dictionary<string, float>();
    void FixedUpdate(){
        {
            Action.NavigateTo("this.speed","this.xObj","this.y","this.zObj",gameObject,scopeList);
        }
        if(Condition.Compare("this.z<this.z0+this.delta",scopeList) && Condition.Compare("this.z>this.z0-this.delta",scopeList) && Condition.Compare("this.x<this.x0+this.delta",scopeList) && Condition.Compare("this.x>this.x0-this.delta",scopeList)){
            Action.Edit("this.xObj","this.x1",scopeList);
            Action.Edit("this.zObj","this.z1",scopeList);
        }
        if(Condition.Compare("this.z<this.z1+this.delta",scopeList) && Condition.Compare("this.z>this.z1-this.delta",scopeList) && Condition.Compare("this.x<this.x1+this.delta",scopeList) && Condition.Compare("this.x>this.x1-this.delta",scopeList)){
            Action.Edit("this.xObj","this.x0",scopeList);
            Action.Edit("this.zObj","this.z0",scopeList);
        }
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"NavigateTo(this.speed,this.xObj,this.y,this.zObj);Compare(this.z<this.z0+this.delta);Compare(this.z>this.z0-this.delta);Compare(this.x<this.x0+this.delta);Compare(this.x>this.x0-this.delta);Edit(this.xObj,this.x1);Edit(this.zObj,this.z1);Compare(this.z<this.z1+this.delta);Compare(this.z>this.z1-this.delta);Compare(this.x<this.x1+this.delta);Compare(this.x>this.x1-this.delta);Edit(this.xObj,this.x0);Edit(this.zObj,this.z0)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("delta=0.5;speed=1.0;xObj=7.4;zObj=-2.0;x0=3.2;x1=7.4;z0=-5;z1=-2");
    }
}
