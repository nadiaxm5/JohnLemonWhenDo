using UnityEngine;
using System.Collections.Generic;

public class Ghost1 : MonoBehaviour {
    public bool Active = true;
    public float speed=1.0f;
    public float zObj=6.4f;
    public float z0=-4.5f;
    public float z1=6.4f;
    void FixedUpdate(){
        {
                Action.NavigateTo("this.x","this.zObj","this.speed",gameObject,scopeList);
        }
        if(Condition.Compare("this.z>=this.z1",scopeList)){
                Action.Edit("this.zObj","this.z0",scopeList);
        }
        if(Condition.Compare("this.z<=this.z0",scopeList)){
                Action.Edit("this.zObj","this.z1",scopeList);
        }
    }
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    void Awake() {
        propertyList = Utils.CreateProperties("speed=1.0;zObj=6.4;z0=-4.5;z1=6.4");
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"NavigateTo(this.x,this.zObj,this.speed);this.z>=this.z1;Edit(this.zObj,this.z0);this.z<=this.z0;Edit(this.zObj,this.z1)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
