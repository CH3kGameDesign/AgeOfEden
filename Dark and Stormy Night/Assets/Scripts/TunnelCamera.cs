using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCamera : MonoBehaviour {

    public GameObject tarCamera;
    public GameObject altWall;
    public GameObject altCamera;

    private Quaternion startRot;

    static bool inAlternate;
	// Use this for initialization
	void Start () {
        startRot = transform.rotation;
        inAlternate = false;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(CameraMovement.cameraObject.transform.position);
        transform.forward = -transform.forward;

        tarCamera.transform.rotation = transform.rotation;

        transform.rotation = startRot;
	}
}
