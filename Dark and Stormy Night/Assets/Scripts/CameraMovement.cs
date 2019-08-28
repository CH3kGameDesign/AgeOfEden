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
    public static Vector2 s_CamShakeDirection;
    public static GameObject s_CameraObject;

    [Header("Booleans")]
    [SerializeField]
    [Tooltip("Allows the player to look around immediately")]
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
    [SerializeField]
    [Tooltip("An object placed on the side of the object a player is looking at")]
    private Transform m_tAimPoint;
    // Unused
    //[SerializeField]
    //private Transform m_tReticle;

    [Header("GameObjects")]
    [SerializeField]
    private List<GameObject> m_LgoEnableObjects;
    

    // Called once before the first frame
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
        m_v2CamRandomShakeDirection = Random.insideUnitCircle;
    }
	
	// Update is called once per frame
	private void Update()
    {
        // Keeps the camera snapped to the characters face
        if (s_bSnapToPlayer && m_tCameraHook)
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
                        m_fFovMin, Time.deltaTime * m_fZoomSpeed);
                else
                    transform.GetChild(0).GetComponent<Camera>().fieldOfView =
                        Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView,
                        m_fFovNormal, Time.deltaTime * m_fZoomSpeed * 2);
            }
            else
            {
                transform.GetChild(0).GetComponent<Camera>().fieldOfView =
                    Mathf.Lerp(transform.GetChild(0).GetComponent<Camera>().fieldOfView,
                    m_fFovNormal, Time.deltaTime * m_fZoomSpeed * 2);
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