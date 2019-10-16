using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    // A static reference to the players GameObject
    public static GameObject s_goPlayerObject;
    public static bool s_bCanMove;

    [HideInInspector]
    public bool m_bIsSprinting = false;
    [HideInInspector]
    public bool m_bGrounded;

    [Space(5)]
    //Button will automatically close the game
    private KeyCode m_kcQuitButton = KeyCode.End;
    // Debug speed tools
    private KeyCode m_kcSpeedUp = KeyCode.Equals;
    private KeyCode m_kcSpeedDown = KeyCode.Minus;

    [Header("Booleans")]
    [SerializeField, Tooltip("Enable this if not at typewriter")]
    private bool m_bCanMoveOnStart = true;
    [SerializeField, Tooltip("Uses the old movement system based around translation" +
        "\nWARNING: Allows the player to force themselves through walls and other objects")]
    private bool m_bLegacyMovement = false;
    [SerializeField]
    private bool m_bAllowJumping = false;
    [SerializeField]
    private bool m_bDebugMode = false;
    
    [Header("Movement Variables")]
    [SerializeField]
    private float m_fForwardSpeed = 1.5f;
    [SerializeField]
    private float m_fStrafeSpeed = 1.3f;
    [SerializeField]
    private float m_fBackwardSpeed = 1f;
    [SerializeField]
    private float m_fSpeedScalar = 500;
    [Tooltip("How much faster the player is while sprinting compared to normal movement")]
    public float m_fSprintMultiplier = 2.4f;
    [Tooltip("Entirely for legacy movement")]
    public float m_fSpeedMultiplier = 1;
    private float m_fSprint = 1f;

    [Header("Aerial Stuff")]
    [SerializeField, Tooltip("Only used if jumping is enabled")]
    private float m_fJumpForce = 30;
    [SerializeField, Tooltip("Scales the movement speed while airborne")]
    private float m_fAerialManuverability = 0.1f;
    [SerializeField, Tooltip("The y height of the ray origin")]
    private float m_fRayOrigin = -0.8f;
    [SerializeField, Tooltip("How long down the ray stretches" +
        "\nWARNING: If ray length does not reach below the players hitbox " +
        "the player will have difficulty moving")]
    private float m_fRaylength = 0.3f;
    [SerializeField, Tooltip("The ground check uses 4 rays to avoid pothole problems, " +
        "this is how far from the center these rays are positioned")]
    private float m_fRaySpread = 0.3f;
    
    private float m_fSizeScalar;
    private float m_fStepDelay;
    private float m_fStepTimer;

    private float m_fStartMass;

    public GameObject fallSound;
    private float fallTimer;
    
    [HideInInspector]
    // x is forward, y is sideways
    public Vector2 m_v2InputVec2;
    private Vector3 m_v3MovementVec3;

    private Rigidbody m_rbRigidbody;
    [HideInInspector]
    public Animator m_aModelAnimator;
    //public FootStepManager footStepManager;

    private void Awake()
    {
        s_goPlayerObject = gameObject;
        m_rbRigidbody = GetComponent<Rigidbody>();
        m_aModelAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    // Called once before the first frame
    private void Start()
    {
        s_bCanMove = m_bCanMoveOnStart;
        m_v2InputVec2 = new Vector2();
        m_fStepDelay = m_fForwardSpeed / 4;
        m_fSizeScalar = transform.localScale.y;
        m_fStartMass = m_rbRigidbody.mass;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bDebugMode)
        {
            Debug.DrawRay(transform.position + new Vector3(0, m_fRayOrigin, m_fRaySpread),
                new Vector3(0, -m_fRaylength, 0), Color.red);
            Debug.DrawRay(transform.position + new Vector3(m_fRaySpread, m_fRayOrigin, 0),
                new Vector3(0, -m_fRaylength, 0), Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, m_fRayOrigin, -m_fRaySpread),
                new Vector3(0, -m_fRaylength, 0), Color.red);
            Debug.DrawRay(transform.position + new Vector3(-m_fRaySpread, m_fRayOrigin, 0),
                new Vector3(0, -m_fRaylength, 0), Color.red);

            if (Input.GetKeyDown(m_kcSpeedUp))
                m_fSprintMultiplier += 3;
            if (Input.GetKeyDown(m_kcSpeedDown))
                m_fSprintMultiplier -= 3;
        }

        // Test for ground below the player
        RaycastHit groundRay1;
        RaycastHit groundRay2;
        RaycastHit groundRay3;
        RaycastHit groundRay4;

        if (Physics.Raycast(transform.position + new Vector3(0, m_fRayOrigin, m_fRaySpread) * transform.localScale.y,
            Vector3.down, out groundRay1, m_fRaylength * transform.localScale.y)
            || Physics.Raycast(transform.position + new Vector3(m_fRaySpread, m_fRayOrigin, 0) * transform.localScale.y,
            Vector3.down, out groundRay2, m_fRaylength * transform.localScale.y)
            || Physics.Raycast(transform.position + new Vector3(0, m_fRayOrigin, -m_fRaySpread) * transform.localScale.y,
            Vector3.down, out groundRay3, m_fRaylength * transform.localScale.y)
            || Physics.Raycast(transform.position + new Vector3(-m_fRaySpread, m_fRayOrigin, 0) * transform.localScale.y,
            Vector3.down, out groundRay4, m_fRaylength * transform.localScale.y))
        {
            m_bGrounded = true;
            m_aModelAnimator.GetComponent<FootStepManager>().makeNoises = true;
            if (fallTimer >= 0.3f && fallSound != null)
                Instantiate(fallSound);
            fallTimer = 0;
        }
        else
        {
            m_bGrounded = false;
            m_aModelAnimator.GetComponent<FootStepManager>().makeNoises = false;
            fallTimer += Time.deltaTime;
        }

        if (s_bCanMove)
        {
            if (!m_bLegacyMovement)
                DoMovement();
            else
                DoMovementLegacy();
        }
        else
        {
            m_aModelAnimator.SetBool("Sprinting", false);
            m_aModelAnimator.SetBool("Moving", false);
            m_aModelAnimator.SetBool("Standing", false);
            m_fStepTimer = 0;
        }

        // Snuck in a quick quit button
        if (Input.GetKeyDown(m_kcQuitButton))
        {
            #if UNITY_STANDALONE
                Application.Quit();
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    /// <summary>
    /// Call to do movement based calculations
    /// </summary>
    private void DoMovement()
    {
        m_fSizeScalar = transform.localScale.y;
        m_aModelAnimator.SetBool("Standing", true);

        m_v2InputVec2.x = Input.GetAxis("Vertical");
        m_v2InputVec2.y = Input.GetAxis("Horizontal");

        m_v3MovementVec3 = new Vector3(m_v2InputVec2.x
            * Mathf.Sqrt(1 - 0.5f * m_v2InputVec2.y * m_v2InputVec2.y),
            m_v2InputVec2.y
            * Mathf.Sqrt(1 - 0.5f * m_v2InputVec2.x * m_v2InputVec2.x));

        m_v2InputVec2 = m_v3MovementVec3;

        // Converts the unit square to a unit circle as to maintain constant magnitude
        m_v2InputVec2.x = m_v2InputVec2.x
            * Mathf.Sqrt(1 - 0.5f * m_v2InputVec2.y * m_v2InputVec2.y);
        m_v2InputVec2.y = m_v2InputVec2.y
            * Mathf.Sqrt(1 - 0.5f * m_v2InputVec2.x * m_v2InputVec2.x);

        // Scales the unit circle
        if (m_v2InputVec2.x > 0)
            m_v2InputVec2.x *= m_fForwardSpeed;
        else
            m_v2InputVec2.x *= m_fBackwardSpeed;

        m_v2InputVec2.y *= m_fStrafeSpeed;

        m_v2InputVec2 *= m_fSizeScalar;
        m_rbRigidbody.mass = m_fStartMass * m_fSizeScalar;

        // Test for movement
        if (m_v2InputVec2 == Vector2.zero)
        {
            m_aModelAnimator.SetBool("Moving", false);
            if (m_fStepTimer != 0)
                m_fStepTimer = 0;
        }
        else
        {
            m_aModelAnimator.SetBool("Moving", true);
            m_fStepTimer += Time.deltaTime;
        }

        AnimUpdate();

        // Rotates the velocity to face the player's "forward" direction
        m_v3MovementVec3 = transform.forward * m_v2InputVec2.x;
        m_v3MovementVec3 += transform.right * m_v2InputVec2.y;
        
        // Finds the difference between desired velocity and current
        m_v3MovementVec3.x -= m_rbRigidbody.velocity.x;
        m_v3MovementVec3.z -= m_rbRigidbody.velocity.z;

        if (m_bAllowJumping && m_bGrounded && Input.GetKeyDown(KeyCode.Space))
            m_v3MovementVec3.y = m_fJumpForce;
        else
            m_v3MovementVec3.y = 0;

        // If the player is in the air, their movement control is extremely limited
        if (m_bGrounded)
            m_v3MovementVec3 *= m_fSpeedScalar;
        else
            m_v3MovementVec3 *= m_fSpeedScalar * m_fAerialManuverability;
        
        // Pushes to in-engine physics
        m_rbRigidbody.AddForce(m_v3MovementVec3);
    }

    /// <summary>
    /// Call to do movement based calculations
    /// Note: This is the old version that uses direct translation
    /// </summary>
    private void DoMovementLegacy()
    {
        //----------vvv OLD SYSTEM vvv----------

        m_aModelAnimator.SetBool("Standing", true);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_aModelAnimator.SetBool("Sprinting", true);
            m_fSprint = Mathf.Lerp(m_fSprint, m_fSprintMultiplier, Time.deltaTime / 0.2f);
        }
        else
        {
            m_aModelAnimator.SetBool("Sprinting", false);
            m_fSprint = Mathf.Lerp(m_fSprint, 1, Time.deltaTime / 0.1f);
        }

        if (Input.GetAxis("Vertical") > 0)
            m_v2InputVec2.x = m_fForwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") < 0)
            m_v2InputVec2.x = -m_fBackwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") == 0)
            m_v2InputVec2.x = 0;

        m_v2InputVec2.y = Input.GetAxis("Horizontal") * m_fStrafeSpeed * m_fSprint;

        if (m_v2InputVec2 == Vector2.zero)
        {
            m_aModelAnimator.SetInteger("WalkDirection", 0);
            m_aModelAnimator.SetBool("Moving", false);
            if (m_fStepTimer != 0)
            {
                //footStepManager.MakeSound();
                m_fStepTimer = 0;
            }
        }
        else
        {
            m_aModelAnimator.SetBool("Moving", true);
            m_fStepTimer += Time.deltaTime;
            AnimUpdate();
        }

        Vector3 desiredPosition = Vector3.ClampMagnitude(
            new Vector3(m_v2InputVec2.x, 0, m_v2InputVec2.y),
            m_fForwardSpeed * Time.deltaTime * m_fSprint);

        transform.position += transform.forward * desiredPosition.x * m_fSpeedMultiplier;
        transform.position += transform.right * desiredPosition.z * m_fSpeedMultiplier;

        if (m_fStepTimer > m_fStepDelay / m_fSprint)
        {
            //footStepManager.MakeSound();
            m_fStepTimer = 0;
        }

        //RaycastHit hit;
        //Debug.DrawRay(transform.position + new Vector3(0, -0.9f, 0),
        //    new Vector3(0, -0.2f, 0), Color.red);

        //if (Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0),
        //    Vector3.down, out hit, 0.2f) && Input.GetKeyDown(KeyCode.Space))
        //{
        //    GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
        //}
    }

    private void AnimUpdate ()
    {
        float verSpeed = Input.GetAxis("Vertical");
        float horSpeed = Input.GetAxis("Horizontal");

        if (Mathf.Abs(verSpeed) >= Mathf.Abs(horSpeed))
        {
            if (verSpeed > 0)
            {
                m_aModelAnimator.SetInteger("WalkDirection", 0);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    m_aModelAnimator.SetBool("Sprinting", true);
                    m_v2InputVec2 *= m_fSprintMultiplier;
                    m_bIsSprinting = true;
                }
                else
                {
                    m_aModelAnimator.SetBool("Sprinting", false);
                    m_bIsSprinting = false;
                }
            }
            else
            {
                m_aModelAnimator.SetInteger("WalkDirection", 3);
                m_aModelAnimator.SetBool("Sprinting", false);
                m_bIsSprinting = false;
            }
        }
        else
        {
            if (Mathf.Abs(horSpeed) > 0.01f)
            {
                if (horSpeed > 0)
                    m_aModelAnimator.SetInteger("WalkDirection", 2);
                else
                    m_aModelAnimator.SetInteger("WalkDirection", 1);
                m_aModelAnimator.SetBool("Sprinting", false);
                m_bIsSprinting = false;
            }
        }
    }
}