using UnityEngine;
using System.Collections.Generic;

public class John : MonoBehaviour {
    public bool Active = true;
    public float move=0f;
    public float delta=0.02f;
    public float desp=5.6f;
    public float time=1.0f;
    public float oneTouch=1.0f;
    void FixedUpdate(){
        {
                Action.Edit("this.move","0",scopeList);
                Action.Edit("Camera.x","this.x",scopeList);
                Action.Edit("Camera.z","this.z-this.desp",scopeList);
        }
        if(Condition.Keyboard("RightArrow","Press")){
                Action.Edit("this.x","this.x+this.delta",scopeList);
                Action.Edit("this.ry","90",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("LeftArrow","Press")){
                Action.Edit("this.x","this.x-this.delta",scopeList);
                Action.Edit("this.ry","-90",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("UpArrow","Press")){
                Action.Edit("this.z","this.z+this.delta",scopeList);
                Action.Edit("this.ry","0",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("DownArrow","Press")){
                Action.Edit("this.z","this.z-this.delta",scopeList);
                Action.Edit("this.ry","180",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Collision("Enemy",gameObject)){
                Action.Active(objectList["Caught"]);
        }
        if(Condition.Collision("End",gameObject)){
                Action.Active(objectList["Won"]);
        }
        if(Condition.Compare("this.move==0",scopeList)){
                Action.StopSound("Footsteps",gameObject);
                Action.Animation("0",gameObject);
        }
        if(Condition.Compare("this.move==1",scopeList)){
                Action.PlaySound("Footsteps",gameObject);
                Action.Animation("1",gameObject);
        }
    }
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    public Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();
    void Awake() {
        propertyList = Utils.CreateProperties("move=0;delta=0.02;desp=5.6;time=1.0;oneTouch=1.0");
        objectList = Utils.CreateObjectList("Caught,Won");
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"Edit(this.move,0);Edit(Camera.x,this.x);Edit(Camera.z,this.z-this.desp);Edit(this.x,this.x+this.delta);Edit(this.ry,90);Edit(this.move,1);Edit(this.x,this.x-this.delta);Edit(this.ry,-90);Edit(this.move,1);Edit(this.z,this.z+this.delta);Edit(this.ry,0);Edit(this.move,1);Edit(this.z,this.z-this.delta);Edit(this.ry,180);Edit(this.move,1);this.move==0;this.move==1");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    public Dictionary<string,bool> Tags = new Dictionary<string,bool>{{"Enemy", false },{"End", false }};
    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Enemy")) Tags["Enemy"]=true;
        if (other.CompareTag("End")) Tags["End"]=true;
    }
    void OnTriggerExit (Collider other) {
        if (other.CompareTag("Enemy")) Tags["Enemy"]=false;
        if (other.CompareTag("End")) Tags["End"]=false;
    }
}
