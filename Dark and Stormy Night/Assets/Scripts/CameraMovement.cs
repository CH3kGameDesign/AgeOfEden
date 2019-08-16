using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraMovement : MonoBehaviour
{
    // Static variables
    public static bool s_bSnapToPlayer;
    public static bool s_bGoToPlayer;
    public static bool s_CanMove = true;
    public static bool s_CanZoom = true;
    public static bool s_Shake = true;
    public static Vector2 s_CamShakeDirection;
    public static GameObject s_CameraObject;

    [SerializeField]
    [Tooltip("Allows the player to look around immediately")]
    private bool m_bCanMoveOnStart = true;
    [SerializeField]
    private bool m_bSnapToPlayerOnStart = true;

    [Header("Camera Speed")]
	public float camRunShakeSpeed = 0.04f;
	public float zoomSpeed = 4;
    public float camIdleShakeSpeed = 0.04f;

    [Header("Camera Limits")]
	public float camRunShakeMax = 1;
    public float camIdleShakeMax = 0.2f;
    public int camIdleTickAmount = 10;
    public float fovNormal = 60;
	public float fovMin = 25;

    [Header("Transforms")]
    [SerializeField]
    [Tooltip("The an object on the players face the camera is attached to")]
    private Transform m_tCameraHook;
    [SerializeField]
    [Tooltip("An object placed on the side of the object a player is looking at")]
    private Transform m_tAimPoint;
    [SerializeField]
    private Transform m_tReticle;

    [Header("GameObjects")]
    public List<GameObject> m_LgoEnableObjects;

	private float camRunShakeAmount;
	private float camRunShake;
    
    private float camIdleShake;
    private int camIdleTicker;
    private Vector2 camRandomShakeDirection;

	// Use this for initialization
	private void Start()
    {
        // Seizes control of the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        s_CameraObject = gameObject;
        s_bSnapToPlayer = m_bSnapToPlayerOnStart;
        s_bGoToPlayer = m_bSnapToPlayerOnStart;
		s_CanMove = m_bCanMoveOnStart;

        // Starts the camera shake in a random direction
        camRandomShakeDirection = Random.insideUnitCircle;
    }
	
	// Update is called once per frame
	private void Update()
    {
        // Keeps the camera snapped to the characters face
        if (s_bSnapToPlayer)
            transform.position = m_tCameraHook.position;

        if (s_Shake)
        {
            RunShake();
            IdleShake();
        }

        if (m_tAimPoint)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                m_tAimPoint.position = hit.point;
            else
                m_tAimPoint.position = transform.position + (transform.forward * 100);
        }

        transform.GetChild(0).localPosition = Vector3.Lerp(
            transform.GetChild(0).localPosition, Vector3.zero, 0.3f);

        if (s_bGoToPlayer)
        {
            transform.position = Vector3.Lerp(
                transform.position, m_tCameraHook.position, 1.5f * Time.deltaTime);

            if (Vector3.Distance(transform.position, m_tCameraHook.position) < 0.3f)
            {
                s_bSnapToPlayer = true;
                //Movement.canMove = true;
                s_bGoToPlayer = false;
                for (int i = 0; i < m_LgoEnableObjects.Count; i++)
                {
                    m_LgoEnableObjects[i].SetActive(true);
                }
            }
        }

        if (s_CanZoom)
        {
            if (s_CanMove)
            {
                if (Input.GetMouseButton(1))
                    transform.GetChild(0).GetComponent<Camera>().fieldOfView =
                        Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView,
                        fovMin, Time.deltaTime * zoomSpeed);
                else
                    transform.GetChild(0).GetComponent<Camera>().fieldOfView =
                        Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView,
                        fovNormal, Time.deltaTime * zoomSpeed * 2);
            }
            else
            {
                transform.GetChild(0).GetComponent<Camera>().fieldOfView =
                    Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView,
                    fovNormal, Time.deltaTime * zoomSpeed * 2);
            }
        }
    }

    /// <summary>
    /// Applies a shake to the camera when you run
    /// </summary>
	private void RunShake()
	{
		float verSpeed = PlayerModel.player.GetComponent<Movement> ().m_v2DesiredVelocity.x;
		float horSpeed = PlayerModel.player.GetComponent<Movement> ().m_v2DesiredVelocity.y;
        bool isSprinting = PlayerModel.player.GetComponent<Movement>().m_bIsSprinting;
        float sprintMult = PlayerModel.player.GetComponent<Movement>().m_fSprintMultiplier;

		if (verSpeed < 0)
			verSpeed = -verSpeed;
		if (horSpeed < 0)
			horSpeed = -horSpeed;

		if (horSpeed > verSpeed)
			camRunShakeAmount = horSpeed;
		else
			camRunShakeAmount = verSpeed;
		
		if (isSprinting)
            sprintMult = (sprintMult + 0.2f) / 2;
		
		float cameraRunBound = camRunShakeMax * sprintMult;

		if ((camRunShake > cameraRunBound && camRunShakeSpeed > 0) ||
            (camRunShake < -cameraRunBound && camRunShakeSpeed < 0))
            camRunShakeSpeed = -camRunShakeSpeed;

		if (camRunShakeAmount != 0)
			camRunShake += camRunShakeAmount * camRunShakeSpeed;
		else
			camRunShake = 0;
	}

    /// <summary>
    /// Adds an idle shake to the camera
    /// </summary>
    private void IdleShake()
    {
        if (camIdleShake > camIdleShakeMax && camIdleShakeSpeed > 0)
            camIdleShakeSpeed = -camIdleShakeSpeed;
        if (camIdleShake < -camIdleShakeMax && camIdleShakeSpeed < 0)
            camIdleShakeSpeed = -camIdleShakeSpeed;
        camIdleShake +=  camIdleShakeSpeed;

        s_CamShakeDirection = camRandomShakeDirection * camIdleShake;

        if (camIdleTickAmount <= camIdleTicker)
        {
            camRandomShakeDirection = Random.insideUnitCircle;
            camIdleTicker = 0;
        }
        camIdleTicker++;
    }
    
    /// <summary>
    /// Changes the shake status
    /// </summary>
    /// <param name="pShaking">The desired shaking state</param>
    public void ChangeShakeStatus(bool pShaking)
    {
        s_Shake = pShaking;
        s_CamShakeDirection = Vector2.zero;
    }
}