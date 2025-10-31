using UnityEngine;
using System.Collections.Generic;

public class John : MonoBehaviour {
    public bool Active = true;
    public float speed=1f;
    public float desp=5.6f;
    public float moving=0f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    void FixedUpdate(){
        {
            Action.Edit("Camera.x","this.x",scopeList);
            Action.Edit("Camera.z","this.z-this.desp",scopeList);
        }
        if(Condition.Collision("Enemy",gameObject)){
            Action.Edit("Caught.Active","1",scopeList);
        }
        if(Condition.Collision("End",gameObject)){
            Action.Edit("Won.Active","1",scopeList);
        }
        if(Condition.Compare("this.moving==0",scopeList)){
            Action.Animate("John_Idle",gameObject);
        }
        if(Condition.Compare("this.moving==1",scopeList)){
            Action.Animate("John_Walk",gameObject);
            Action.PlaySound("Footsteps",gameObject);
        }
    }
    void Update(){
        if(Condition.Keyboard("RightArrow","press")){
            Action.Move("this.speed","0","90","0",gameObject,scopeList);
            Action.Edit("this.ry","90",scopeList);
            Action.Edit("this.moving","1",scopeList);
        }
        if(Condition.Keyboard("LeftArrow","press")){
            Action.Move("this.speed","0","-90","0",gameObject,scopeList);
            Action.Edit("this.ry","-90",scopeList);
            Action.Edit("this.moving","1",scopeList);
        }
        if(Condition.Keyboard("UpArrow","press")){
            Action.Move("this.speed","0","0","0",gameObject,scopeList);
            Action.Edit("this.ry","0",scopeList);
            Action.Edit("this.moving","1",scopeList);
        }
        if(Condition.Keyboard("DownArrow","press")){
            Action.Move("this.speed","0","180","0",gameObject,scopeList);
            Action.Edit("this.ry","180",scopeList);
            Action.Edit("this.moving","1",scopeList);
        }
        if(Condition.Keyboard("RightArrow","up")){
            Action.Edit("this.moving","0",scopeList);
        }
        if(Condition.Keyboard("LeftArrow","up")){
            Action.Edit("this.moving","0",scopeList);
        }
        if(Condition.Keyboard("UpArrow","up")){
            Action.Edit("this.moving","0",scopeList);
        }
        if(Condition.Keyboard("DownArrow","up")){
            Action.Edit("this.moving","0",scopeList);
        }
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"Edit(Camera.x,this.x);Edit(Camera.z,this.z-this.desp);Edit(Caught.Active,1);Edit(Won.Active,1);Compare(this.moving==0);Animate(John_Idle);Compare(this.moving==1);Animate(John_Walk);PlaySound(Footsteps);Keyboard(RightArrow,press);Move(this.speed,0,90,0);Edit(this.ry,90);Edit(this.moving,1);Keyboard(LeftArrow,press);Move(this.speed,0,-90,0);Edit(this.ry,-90);Keyboard(UpArrow,press);Move(this.speed,0,0,0);Edit(this.ry,0);Keyboard(DownArrow,press);Move(this.speed,0,180,0);Edit(this.ry,180);Keyboard(RightArrow,up);Edit(this.moving,0);Keyboard(LeftArrow,up);Keyboard(UpArrow,up);Keyboard(DownArrow,up)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    public Dictionary<string, HashSet<GameObject>> TagCollisions = new Dictionary<string, HashSet<GameObject>>();
    void OnTriggerEnter(Collider other) {
        if (TagCollisions.ContainsKey(other.tag))
            TagCollisions[other.tag].Add(other.gameObject);
    }
    void OnTriggerExit(Collider other) {
        if (TagCollisions.ContainsKey(other.tag))
            TagCollisions[other.tag].Remove(other.gameObject);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("speed=1;desp=5.6;moving=0");
        TagCollisions["Untagged"] = new HashSet<GameObject>();
        TagCollisions["Respawn"] = new HashSet<GameObject>();
        TagCollisions["Finish"] = new HashSet<GameObject>();
        TagCollisions["EditorOnly"] = new HashSet<GameObject>();
        TagCollisions["MainCamera"] = new HashSet<GameObject>();
        TagCollisions["Player"] = new HashSet<GameObject>();
        TagCollisions["GameController"] = new HashSet<GameObject>();
        TagCollisions["End"] = new HashSet<GameObject>();
        TagCollisions["Enemy"] = new HashSet<GameObject>();
    }
}
