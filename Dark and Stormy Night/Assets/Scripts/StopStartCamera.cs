using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStartCamera : MonoBehaviour {

    public bool variable;

	// Use this for initialization
	void Start () {
        CameraMovement.s_bSnapToPlayer = variable;
        CameraMovement.s_bGoToPlayer = variable;
        this.gameObject.GetComponent<StopStartCamera>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
