﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraMovement : MonoBehaviour {

    [Header("CameraSpeed")]
	public float camRunShakeSpeed;
	public float zoomSpeed = 4;
    public float camIdleShakeSpeed = 2;

    [Header("CameraLimits")]
	public float camRunShakeMax;
    public float camIdleShakeMax;
    public int camIdleTickAmount;
    public float fovNormal = 60;
	public float fovMin = 25;

    [Header("Transforms")]
    public Transform cameraHook;
    public Transform aimPoint;
    public Transform reticle;

    [Header("GameObjects")]
    public List<GameObject> enableObjects;

	private float camRunShakeAmount;
	private float camRunShake;
    
    private float camIdleShake;
    private int camIdleTicker;
    private Vector2 camRandomShakeDirection;


    public bool canMoveOnStart = true;
    public bool snapToPlayerOnStart = true;
    public static bool snapToPlayer;
    public static bool goToPlayer;
    public static bool canMove = true;
    public static bool canZoom = true;
    public static bool shake = true;

    public static GameObject cameraObject;

    public static Vector2 camShakeDirection;



	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        snapToPlayer = snapToPlayerOnStart;
        goToPlayer = snapToPlayerOnStart;
        cameraObject = this.gameObject;
		canMove = canMoveOnStart;
        camRandomShakeDirection = Random.insideUnitCircle;
    }
	
	// Update is called once per frame
	void Update () {
        if (snapToPlayer == true)
            transform.position = cameraHook.position;

        if (shake)
        {
            RunShake();
            IdleShake();
        }

        if (aimPoint != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                aimPoint.position = hit.point;
            else
                aimPoint.position = transform.position + (transform.forward * 100);
        }

        transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, Vector3.zero, 0.3f);

        if (goToPlayer == true)
        {
            transform.position = Vector3.Lerp(transform.position, cameraHook.position, 1.5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, cameraHook.position) < 0.3f)
            {
                snapToPlayer = true;
                //Movement.canMove = true;
                goToPlayer = false;
                for (int i = 0; i < enableObjects.Count; i++)
                {
                    enableObjects[i].SetActive(true);
                }
            }
        }

        if (canZoom == true)
        {
            if (canMove == true)
            {
                if (Input.GetMouseButton(1))
                    transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView, fovMin, Time.deltaTime * zoomSpeed);
                else
                    transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView, fovNormal, Time.deltaTime * zoomSpeed * 2);
            }
            else
                transform.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView, fovNormal, Time.deltaTime * zoomSpeed * 2);
        }
    }

	void RunShake()
	{
		float horSpeed = PlayerModel.player.GetComponent<Movement> ().horSpeed;
		float verSpeed = PlayerModel.player.GetComponent<Movement> ().verSpeed;
		float sprint = PlayerModel.player.GetComponent<Movement> ().sprint;

		if (horSpeed < 0)
			horSpeed = -horSpeed;
		if (verSpeed < 0)
			verSpeed = -verSpeed;

		if (horSpeed > verSpeed)
			camRunShakeAmount = horSpeed;
		else
			camRunShakeAmount = verSpeed;
		
		if (sprint > 1)
			sprint = (sprint + 0.4f) / 2;
		
		float cameraRunBound = camRunShakeMax * sprint;

		if (camRunShake > cameraRunBound && camRunShakeSpeed > 0)
			camRunShakeSpeed = -camRunShakeSpeed;
		if (camRunShake < -cameraRunBound && camRunShakeSpeed < 0)
			camRunShakeSpeed = -camRunShakeSpeed;
		if (camRunShakeAmount != 0)
			camRunShake += camRunShakeAmount * camRunShakeSpeed;
		else
			camRunShake = 0;
	}

    void IdleShake()
    {
        if (camIdleShake > camIdleShakeMax && camIdleShakeSpeed > 0)
            camIdleShakeSpeed = -camIdleShakeSpeed;
        if (camIdleShake < -camIdleShakeMax && camIdleShakeSpeed < 0)
            camIdleShakeSpeed = -camIdleShakeSpeed;
        camIdleShake +=  camIdleShakeSpeed;

        camShakeDirection = camRandomShakeDirection * camIdleShake;

        if (camIdleTickAmount <= camIdleTicker)
        {
            camRandomShakeDirection = Random.insideUnitCircle;
            camIdleTicker = 0;
        }
        camIdleTicker++;
    }

    public void ChangeShakeStatus(bool shaking)
    {
        shake = shaking;
        camShakeDirection = Vector2.zero;
    }
}
