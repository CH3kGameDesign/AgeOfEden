using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCam : MonoBehaviour {

    public Movement playerMovement;
    public SmoothCameraMovement smoothCam;
    public CameraMovement camMovement;
    public PlayerModel playerModel;

    public float camSpeed = 1;
    public float camChangeSpeed = 0.05f;
    public bool isStanding;
    public bool flyCamEnabled = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            flyCamEnabled = !flyCamEnabled;
            if (flyCamEnabled)
                SmoothCameraMovement.s_bIgnoreSittingRotation = true;
            else
            {
                SmoothCameraMovement.s_bIgnoreSittingRotation = isStanding;
                CameraMovement.s_bGoToPlayer = true;
                CameraMovement.s_bSnapToPlayer = true;
                CameraMovement.s_Shake = true;
                CameraMovement.s_CamShakeDirection = Vector2.zero;
                playerModel.enabled = true;
                playerMovement.enabled = true;
            }
        }
        if (flyCamEnabled)
        {
            playerMovement.enabled = false;
            playerModel.enabled = false;
            CameraMovement.s_bGoToPlayer = false;
            CameraMovement.s_bSnapToPlayer = false;
            CameraMovement.s_Shake = false;
            CameraMovement.s_CamShakeDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.Minus))
                camSpeed -= camChangeSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Equals))
                camSpeed += camChangeSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.W))
                transform.position += transform.forward * camSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                transform.position -= transform.forward * camSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                transform.position += transform.right * camSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                transform.position -= transform.right * camSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E))
                transform.position += transform.up * camSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q))
                transform.position -= transform.up * camSpeed * Time.deltaTime;
        }
        else
        {
            
            isStanding = SmoothCameraMovement.s_bIgnoreSittingRotation;
        }
        
    }
}
