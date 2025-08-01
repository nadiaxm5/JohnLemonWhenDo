using UnityEngine;
using System.Collections.Generic;

public class Gargoyle1 : MonoBehaviour {
    public bool Active = true;
    void Start() {
        if (Active) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
