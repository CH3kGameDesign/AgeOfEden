using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Static variables
    public static bool s_bSnapToPlayer;
    public static bool s_bGoToPlayer;
    public static bool s_CanMove = true;
    public static bool s_CanZoom = true;
    public static bool s_Shake = true;
    public static bool s_bIsZoomed = false;
    public static Vector2 s_CamShakeDirection;
    public static GameObject s_CameraObject;

    [Header("Booleans")]
    [SerializeField, Tooltip("Allows the player to look around immediately")]
    private bool m_bCanMoveOnStart = true;
    [SerializeField]
    private bool m_bSnapToPlayerOnStart = true;

    public Transform underWaterQuad;
    [Header("Camera Shake")]
    [SerializeField]
    private int m_fCamIdleTickAmount = 10;
    [SerializeField]
	private float m_fCamRunShakeSpeed = 0.04f;
    [SerializeField]
    private float m_fCamIdleShakeSpeed = 0.04f;
    [SerializeField]
    private float m_fCamRunShakeMax = 1;
    [SerializeField]
    private float m_fCamIdleShakeMax = 0.2f;

    [Header("Camera Zoom")]
    [SerializeField]
	private float m_fZoomSpeed = 4;
    [SerializeField]
    private float m_fFovNormal = 60;
    [SerializeField]
    private float m_fFovMin = 25;

    private int m_iCamIdleTicker;
    private float m_fCamIdleShake;
	private float m_fCamRunShakeAmount;
	private float m_fCamRunShake;
    private Vector2 m_v2CamRandomShakeDirection;

    [Header("Transforms")]
    [Tooltip("The an object on the players face the camera is attached to")]
    public Transform m_tCameraHook;
    [SerializeField, Tooltip("An object placed on the side of the object a player is looking at")]
    private Transform m_tAimPoint;
    // Unused
    //[SerializeField]
    //private Transform m_tReticle;

    [Header("GameObjects")]
    [SerializeField]
    private List<GameObject> m_LgoEnableObjects;

    private Movement m_mMovementRef;

    private Transform m_tTransCache;
    private Transform m_tFirstChild;
    private Camera m_cFirstChildCamera;

    private void Awake()
    {
        s_CameraObject = gameObject;
    }

    // Called once before the first frame
    private void Start()
    {
        m_tTransCache = transform;
        m_tFirstChild = transform.GetChild(0);
        m_cFirstChildCamera = transform.GetChild(0).GetComponent<Camera>();

        // Seizes control of the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        s_bSnapToPlayer = m_bSnapToPlayerOnStart;
        s_bGoToPlayer = m_bSnapToPlayerOnStart;
		s_CanMove = m_bCanMoveOnStart;

        // Starts the camera shake in a random direction
        m_v2CamRandomShakeDirection = Random.insideUnitCircle;

        //m_mMovementRef = PlayerModel.player.GetComponent<Movement>();
        // Its like this because the title screen has a different set-up
        if (transform.parent.GetChild(2).GetComponent<Movement>())
            m_mMovementRef = transform.parent.GetChild(2).GetComponent<Movement>();
        else
            m_mMovementRef = transform.parent.GetChild(3).GetComponent<Movement>();
    }
	
	// Update is called once per frame
	private void Update()
    {
        // Keeps the camera snapped to the characters face
        if (s_bSnapToPlayer && m_tCameraHook)
            m_tTransCache.position = m_tCameraHook.position;

        if (s_Shake)
        {
            RunShake();
            IdleShake();
        }

        if (m_tAimPoint)
        {
            RaycastHit hit;
            if (Physics.Raycast(m_tTransCache.position, m_tTransCache.forward, out hit, 100))
                m_tAimPoint.position = hit.point;
            else
                m_tAimPoint.position = m_tTransCache.position + (m_tTransCache.forward * 100);
        }

        m_tFirstChild.localPosition = Vector3.Lerp(
            m_tFirstChild.localPosition, Vector3.zero, 0.3f);

        if (s_bGoToPlayer)
        {
            m_tTransCache.position = Vector3.Lerp(
                m_tTransCache.position, m_tCameraHook.position, 1.5f * Time.deltaTime);

            if (Vector3.Distance(m_tTransCache.position, m_tCameraHook.position) < 0.3f)
            {
                s_bSnapToPlayer = true;
                //Movement.s_bCanMove = true;
                s_bGoToPlayer = false;
                for (int i = 0; i < m_LgoEnableObjects.Count; i++)
                {
                    m_LgoEnableObjects[i].SetActive(true);
                }
            }
        }

        // Do camera zoom
        if (s_CanZoom)
        {
            if (s_CanMove)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    m_cFirstChildCamera.fieldOfView = Mathf.Lerp(m_cFirstChildCamera.fieldOfView,
                        m_fFovMin, Time.deltaTime * m_fZoomSpeed);
                    s_bIsZoomed = true;
                }
                else
                {
                    m_cFirstChildCamera.fieldOfView =Mathf.Lerp(m_cFirstChildCamera.fieldOfView,
                        m_fFovNormal, Time.deltaTime * m_fZoomSpeed * 2);
                    s_bIsZoomed = false;
                }
            }
            else
            {
                m_cFirstChildCamera.fieldOfView =Mathf.Lerp(m_cFirstChildCamera.fieldOfView,
                    m_fFovNormal, Time.deltaTime * m_fZoomSpeed * 2);
                s_bIsZoomed = false;
            }
        }
    }

    /// <summary>
    /// Applies a shake to the camera when you run
    /// </summary>
	private void RunShake()
	{
		float verSpeed = m_mMovementRef.m_v2InputVec2.x;
		float horSpeed = m_mMovementRef.m_v2InputVec2.y;
        bool isSprinting = m_mMovementRef.m_bIsSprinting;
        float sprintMult = m_mMovementRef.m_fSprintMultiplier;

		if (verSpeed < 0)
			verSpeed = -verSpeed;
		if (horSpeed < 0)
			horSpeed = -horSpeed;

		if (horSpeed > verSpeed)
			m_fCamRunShakeAmount = horSpeed;
		else
			m_fCamRunShakeAmount = verSpeed;
		
		if (isSprinting)
            sprintMult = (sprintMult + 0.2f) / 2;
		
		float cameraRunBound = m_fCamRunShakeMax * sprintMult;

		if ((m_fCamRunShake > cameraRunBound && m_fCamRunShakeSpeed > 0)
            || (m_fCamRunShake < -cameraRunBound && m_fCamRunShakeSpeed < 0))
            m_fCamRunShakeSpeed = -m_fCamRunShakeSpeed;

		if (m_fCamRunShakeAmount != 0)
			m_fCamRunShake += m_fCamRunShakeAmount * m_fCamRunShakeSpeed;
		else
			m_fCamRunShake = 0;
	}

    /// <summary>
    /// Adds an idle shake to the camera
    /// </summary>
    private void IdleShake()
    {
        if (m_fCamIdleShake > m_fCamIdleShakeMax && m_fCamIdleShakeSpeed > 0)
            m_fCamIdleShakeSpeed = -m_fCamIdleShakeSpeed;
        if (m_fCamIdleShake < -m_fCamIdleShakeMax && m_fCamIdleShakeSpeed < 0)
            m_fCamIdleShakeSpeed = -m_fCamIdleShakeSpeed;
        m_fCamIdleShake +=  m_fCamIdleShakeSpeed;

        s_CamShakeDirection = m_v2CamRandomShakeDirection * m_fCamIdleShake;

        if (m_fCamIdleTickAmount <= m_iCamIdleTicker)
        {
            m_v2CamRandomShakeDirection = Random.insideUnitCircle;
            m_iCamIdleTicker = 0;
        }
        m_iCamIdleTicker++;
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