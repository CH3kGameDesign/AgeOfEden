using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptInParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            if (GetComponentInParent<SceneChanger>() != null)
                GetComponentInParent<SceneChanger>().StartLoad();
        }
    }
}
