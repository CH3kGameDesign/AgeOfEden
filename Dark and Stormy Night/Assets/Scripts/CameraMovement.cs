using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraMovement : MonoBehaviour
{
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
    [SerializeField]
    [Tooltip("The an object on the players face the camera is attached to")]
    private Transform m_tCameraHook;
    [SerializeField]
    [Tooltip("An object placed on the side of the object a player is looking at")]
    private Transform m_tAimPoint;
    [SerializeField]
    private Transform m_tReticle;

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
	private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        snapToPlayer = snapToPlayerOnStart;
        goToPlayer = snapToPlayerOnStart;
        cameraObject = this.gameObject;
		canMove = canMoveOnStart;
        camRandomShakeDirection = Random.insideUnitCircle;
    }
	
	// Update is called once per frame
	private void Update()
    {
        if (snapToPlayer)
            transform.position = m_tCameraHook.position;

        if (shake)
        {
            RunShake();
            IdleShake();
        }
        
        if (m_tAimPoint)
        {
            RaycastHit hit2;
            if (Physics.Raycast(transform.position, transform.forward, out hit2, 100))
                m_tAimPoint.position = hit2.point;
            else
                m_tAimPoint.position = transform.position + (transform.forward * 100);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            m_tAimPoint.position = hit.point;
        else
            m_tAimPoint.position = transform.position + (transform.forward * 100);

        transform.GetChild(0).localPosition = Vector3.Lerp(
            transform.GetChild(0).localPosition, Vector3.zero, 0.3f);

        if (goToPlayer)
        {
            transform.position = Vector3.Lerp(
                transform.position, m_tCameraHook.position, 1.5f * Time.deltaTime);

            if (Vector3.Distance(transform.position, m_tCameraHook.position) < 0.3f)
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

        if (canZoom)
        {
            if (canMove)
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

        camShakeDirection = camRandomShakeDirection * camIdleShake;

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
        shake = pShaking;
        camShakeDirection = Vector2.zero;
    }
}