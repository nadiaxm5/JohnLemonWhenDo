using UnityEngine;
using System.Collections.Generic;

public class Gargoyle2 : MonoBehaviour {
    public bool Active = true;
    void FixedUpdate(){
    }
    void Start() {
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
