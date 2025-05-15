using UnityEngine;
using System.Collections.Generic;

public class End : MonoBehaviour {
    public bool Active = true;
    void FixedUpdate(){
    }
    void Start() {
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
