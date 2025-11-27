using UnityEngine;
using System.Collections.Generic;

public class Won : MonoBehaviour {
    public bool Active = false;
    public float counter=0f;
    public Dictionary<string, float> propertyList = new Dictionary<string, float>();
    private Dictionary<string, float> timers = new Dictionary<string, float>();
    void FixedUpdate(){
        if(Condition.Compare("this.counter>=100",scopeList)){
            Action.QuitGame();
        }
        if(Condition.Compare("this.counter<=100",scopeList)){
            Action.Edit("this.counter","this.counter+1",scopeList);
        }
    }
    public Dictionary<string, GameObject> scopeList = new Dictionary<string, GameObject>();
    void Start() {
        scopeList = Utils.CreateScope(gameObject.GetInstanceID(),"Compare(this.counter>=100);QuitGame();Compare(this.counter<=100);Edit(this.counter,this.counter+1)");
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    void Awake() {
        propertyList = Utils.CreateProperties("counter=0");
    }
}
