using UnityEngine;
using System.Collections.Generic;

public class John : MonoBehaviour {
    public bool Active = true;
    public float move=0f;
    public float speed=2f;
    public float desp=5.6f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    void FixedUpdate(){
        {
                Action.Edit("this.move","0",scopeList);
                Action.Edit("Camera.x","this.x",scopeList);
                Action.Edit("Camera.z","this.z-this.desp",scopeList);
        }
        if(Condition.Keyboard("RightArrow","Press")){
                Action.Move("90","this.speed",gameObject,scopeList);
                Action.Edit("this.ry","90",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("LeftArrow","Press")){
                Action.Move("-90","this.speed",gameObject,scopeList);
                Action.Edit("this.ry","-90",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("UpArrow","Press")){
                Action.Move("0","this.speed",gameObject,scopeList);
                Action.Edit("this.ry","0",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Keyboard("DownArrow","Press")){
                Action.Move("180","this.speed",gameObject,scopeList);
                Action.Edit("this.ry","180",scopeList);
                Action.Edit("this.move","1",scopeList);
        }
        if(Condition.Collision("Enemy",gameObject)){
                Action.Edit("Caught.Active","1",scopeList);
        }
        if(Condition.Collision("End",gameObject)){
                Action.Edit("Won.Active","1",scopeList);
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
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"Edit(this.move,0);Edit(Camera.x,this.x);Edit(Camera.z,this.z-this.desp);Move(90,this.speed);Edit(this.ry,90);Edit(this.move,1);Move(-90,this.speed);Edit(this.ry,-90);Move(0,this.speed);Edit(this.ry,0);Move(180,this.speed);Edit(this.ry,180);Edit(Caught.Active,1);Edit(Won.Active,1);this.move==0;this.move==1");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    public Dictionary<string, HashSet<GameObject>> TagCollisions = new Dictionary<string, HashSet<GameObject>>();
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Untagged")) TagCollisions["Untagged"].Add(other.gameObject);
        if (other.CompareTag("Respawn")) TagCollisions["Respawn"].Add(other.gameObject);
        if (other.CompareTag("Finish")) TagCollisions["Finish"].Add(other.gameObject);
        if (other.CompareTag("EditorOnly")) TagCollisions["EditorOnly"].Add(other.gameObject);
        if (other.CompareTag("MainCamera")) TagCollisions["MainCamera"].Add(other.gameObject);
        if (other.CompareTag("Player")) TagCollisions["Player"].Add(other.gameObject);
        if (other.CompareTag("GameController")) TagCollisions["GameController"].Add(other.gameObject);
        if (other.CompareTag("End")) TagCollisions["End"].Add(other.gameObject);
        if (other.CompareTag("Enemy")) TagCollisions["Enemy"].Add(other.gameObject);
    }
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Untagged")) TagCollisions["Untagged"].Remove(other.gameObject);
        if (other.CompareTag("Respawn")) TagCollisions["Respawn"].Remove(other.gameObject);
        if (other.CompareTag("Finish")) TagCollisions["Finish"].Remove(other.gameObject);
        if (other.CompareTag("EditorOnly")) TagCollisions["EditorOnly"].Remove(other.gameObject);
        if (other.CompareTag("MainCamera")) TagCollisions["MainCamera"].Remove(other.gameObject);
        if (other.CompareTag("Player")) TagCollisions["Player"].Remove(other.gameObject);
        if (other.CompareTag("GameController")) TagCollisions["GameController"].Remove(other.gameObject);
        if (other.CompareTag("End")) TagCollisions["End"].Remove(other.gameObject);
        if (other.CompareTag("Enemy")) TagCollisions["Enemy"].Remove(other.gameObject);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("move=0;speed=2;desp=5.6");
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
