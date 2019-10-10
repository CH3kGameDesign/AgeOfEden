using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStartCamera : MonoBehaviour
{
    public bool variable;

	// Use this for initialization
	private void Start()
    {
        CameraMovement.s_bSnapToPlayer = variable;
        CameraMovement.s_bGoToPlayer = variable;
        gameObject.GetComponent<StopStartCamera>().enabled = false;
	}
}
