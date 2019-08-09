using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public static bool canMove;
    [HideInInspector]
    public bool m_bIsSprinting = false;
    [HideInInspector]
    public bool m_bGrounded;

    [Header("Booleans")]
    [Tooltip("Enable this if not at typewriter")]
    [SerializeField]
    private bool m_bCanMoveOnStart = true;
    [SerializeField]
    [Tooltip("Uses the old movement system based around translation" +
        "\nWARNING: Allows the player to force themselves through walls and other objects")]
    private bool m_bLegacyMovement = false;
    [SerializeField]
    private bool m_bAllowJumping = false;
    
    [Header("Movement variables")]
    [SerializeField]
    private float m_fForwardSpeed = 3;
    [SerializeField]
    private float m_fStrafeSpeed = 2.5f;
    [SerializeField]
    private float m_fBackwardSpeed = 2;
    [SerializeField]
    private float m_fSpeedScalar = 500;

    [Header("Aerial Stuff")]
    [Tooltip("Only used if jumping is enabled")]
    [SerializeField]
    private float m_fJumpForce = 30;
    [Tooltip("Scales the movement speed while airborne")]
    [SerializeField]
    private float m_fAerialManuverability = 0.1f;

    [Header("Tweeks")]
    [Tooltip("How much faster the player is while sprinting compared to normal movement")]
    public float m_fSprintMultiplier = 2;
    [Tooltip("The first scaling done to the input")]
    public float m_fSpeedMultiplier = 1;

    private float m_fSizeScalar;
    private float m_fStepDelay;
    private float m_fStepTimer;

    [HideInInspector]
    private float m_fSprint = 1f;

    [HideInInspector]
    // x is forward, y is sideways
    public Vector2 m_v2DesiredVelocity;

    private Rigidbody m_rbRigidbody;
    [HideInInspector]
    public Animator m_aModelAnimator;
    //public FootStepManager footStepManager;

    // What is this???
    public static GameObject m_goPlayerObject;

    // Called once before the first frame
    private void Start ()
    {
        canMove = m_bCanMoveOnStart;
        m_v2DesiredVelocity = new Vector2();
        m_fStepDelay = m_fForwardSpeed / 4;
        m_fSizeScalar = transform.localScale.y;

        m_rbRigidbody = GetComponent<Rigidbody>();
        m_aModelAnimator = gameObject.GetComponentInChildren<Animator>();

        m_goPlayerObject = gameObject;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (canMove)
        {
            // Test for ground below the player
            RaycastHit groundRay;
            //Debug.DrawRay(transform.position + new Vector3(0, -0.95f, 0),
            //    new Vector3(0, -0.1f, 0), Color.red);

            if (Physics.Raycast(transform.position + new Vector3(0, -0.95f, 0),
                Vector3.down, out groundRay, 0.1f))
                m_bGrounded = true;
            else
                m_bGrounded = false;

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
    }

    /// <summary>
    /// Call to do movement based calculations
    /// </summary>
    private void DoMovement ()
    {
        m_aModelAnimator.SetBool("Standing", true);

        m_v2DesiredVelocity.x = Input.GetAxis("Vertical");
        m_v2DesiredVelocity.y = Input.GetAxis("Horizontal");

        // Converts the unit square to a unit circle as to maintain constant magnitude
        m_v2DesiredVelocity.x = m_v2DesiredVelocity.x
            * Mathf.Sqrt(1 - 0.5f * m_v2DesiredVelocity.y * m_v2DesiredVelocity.y);
        m_v2DesiredVelocity.y = m_v2DesiredVelocity.y
            * Mathf.Sqrt(1 - 0.5f * m_v2DesiredVelocity.x * m_v2DesiredVelocity.x);

        // Scales the unit circle
        if (m_v2DesiredVelocity.x > 0)
            m_v2DesiredVelocity.x *= m_fForwardSpeed;
        else
            m_v2DesiredVelocity.x *= m_fBackwardSpeed;

        m_v2DesiredVelocity.y *= m_fStrafeSpeed;

        m_v2DesiredVelocity *= m_fSizeScalar;

        // Apply a speed multiplier
        m_v2DesiredVelocity *= m_fSpeedMultiplier;

        // Test for movement
        if (m_v2DesiredVelocity == Vector2.zero)
        {
            m_aModelAnimator.SetBool("Moving", false);
            if (m_fStepTimer != 0)
                m_fStepTimer = 0;
        }
        else
        {
            // Test for sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_aModelAnimator.SetBool("Sprinting", true);
                m_v2DesiredVelocity *= m_fSprintMultiplier;
                m_bIsSprinting = true;
            }
            else
            {
                m_aModelAnimator.SetBool("Sprinting", false);
                m_aModelAnimator.SetBool("Moving", true);
                m_bIsSprinting = false;
            }

            m_fStepTimer += Time.deltaTime;
        }

        Vector3 rotatedVelocity = transform.forward * m_v2DesiredVelocity.x;
        rotatedVelocity += transform.right * m_v2DesiredVelocity.y;

        Vector3 appliedForce;

        // Finds the difference between desired velocity and current
        appliedForce.x = rotatedVelocity.x - m_rbRigidbody.velocity.x;
        appliedForce.z = rotatedVelocity.z - m_rbRigidbody.velocity.z;

        if (m_bAllowJumping && m_bGrounded && Input.GetKeyDown(KeyCode.Space))
            appliedForce.y = m_fJumpForce;
        else
            appliedForce.y = 0;

        // If the player is in the air, their movement control is extremely limited
        if (m_bGrounded)
            appliedForce *= m_fSpeedScalar;
        else
            appliedForce *= m_fSpeedScalar * m_fAerialManuverability;

        // Pushes to in-engine physics
        m_rbRigidbody.AddForce(appliedForce);
    }

    /// <summary>
    /// Call to do movement based calculations
    /// Note: This is the old version that uses direct translation
    /// </summary>
    private void DoMovementLegacy ()
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
            m_v2DesiredVelocity.x = m_fForwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") < 0)
            m_v2DesiredVelocity.x = -m_fBackwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") == 0)
            m_v2DesiredVelocity.x = 0;

        m_v2DesiredVelocity.y = Input.GetAxis("Horizontal") * m_fStrafeSpeed * m_fSprint;

        if (m_v2DesiredVelocity == Vector2.zero)
        {
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
        }

        Vector3 desiredPosition = Vector3.ClampMagnitude(
            new Vector3(m_v2DesiredVelocity.x, 0, m_v2DesiredVelocity.y),
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
}