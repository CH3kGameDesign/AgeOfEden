using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterEnabler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Change (bool value)
    {
        CameraMovement.s_CameraObject.GetComponent<CameraMovement>().underWaterQuad.gameObject.SetActive(value);
    }
}
