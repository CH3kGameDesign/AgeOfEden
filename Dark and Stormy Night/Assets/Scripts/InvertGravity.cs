﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : MonoBehaviour {

    public Vector3 alteredGravity;
    private Vector3 normalGravity;

    public static bool invertedGravity = false;
    // Use this for initialization
    void Start () {
        normalGravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.gravity = alteredGravity;
            invertedGravity = true;
            SmoothCameraMovement.gravDirection = 180;
            
            Debug.Log("Flip");
        }
    }

    private void OnDisable()
    {
        Physics.gravity = normalGravity;
        invertedGravity = false;
        SmoothCameraMovement.gravDirection = 0;
    }
}
