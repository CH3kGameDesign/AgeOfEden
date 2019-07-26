using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedChange : MonoBehaviour {

    public float tarSpeedMultiplier;

	// Use this for initialization
	void Start () {
        Movement.player.GetComponent<Movement>().speedMultiplier = tarSpeedMultiplier;
        GetComponent<MovementSpeedChange>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
