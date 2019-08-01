using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public static bool canMove;
    public bool legacyMovement = false;
    [HideInInspector]
    public bool m_bIsSprinting = false;

    [Header("Variables")]
    public bool m_bCanMoveOnStart = true;
    [Space(5)]
    public float m_fForwardSpeed = 3;
    public float m_fStrafeSpeed = 2.5f;
    public float m_fBackwardSpeed = 2;
    public float m_fSpeedScalar = 100;

    private float m_fSizeScalar;

    [Space(5)]
    public float m_fSprintMultiplier = 1.5f;

    [Space(5)]
    public float m_fSpeedMultiplier = 1;

    private float m_fStepDelay;
    private float m_fStepTimer;

    [HideInInspector]
    // x is forward, y is sideways
    public Vector2 desiredVelocity;

    //[HideInInspector]
    //public float m_fSprint;
    
    private Rigidbody m_rbRigidbody;
    [HideInInspector]
    public Animator m_aModelAnimator;

    //public FootStepManager footStepManager;
    private float m_fSprint = 1f;

    // What is this???
    public static GameObject m_goPlayerObject;

	// Called once before the first frame
	private void Start ()
    {
        canMove = m_bCanMoveOnStart;
        desiredVelocity = new Vector2();
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
            if (!legacyMovement)
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

        desiredVelocity.x = Input.GetAxis("Vertical");
        desiredVelocity.y = Input.GetAxis("Horizontal");

        // Converts the unit square to a unit circle as to maintain constant magnitude
        desiredVelocity.x = desiredVelocity.x
            * Mathf.Sqrt(1 - 0.5f * desiredVelocity.y * desiredVelocity.y);
        desiredVelocity.y = desiredVelocity.y
            * Mathf.Sqrt(1 - 0.5f * desiredVelocity.x * desiredVelocity.x);

        // Scales the unit circle
        if (desiredVelocity.x > 0)
            desiredVelocity.x *= m_fForwardSpeed;
        else
            desiredVelocity.x *= m_fBackwardSpeed;

        desiredVelocity.y *= m_fStrafeSpeed;

        desiredVelocity *= m_fSizeScalar;

        // Apply a speed multiplier
        desiredVelocity *= m_fSpeedMultiplier;

        // Test for movement
        if (desiredVelocity == Vector2.zero)
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
                desiredVelocity *= m_fSprintMultiplier;
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

        Vector3 rotatedVelocity = transform.forward * desiredVelocity.x;
        rotatedVelocity += transform.right * desiredVelocity.y;

        Vector3 appliedForce;

        // Finds the difference between desired velocity and current
        appliedForce.x = rotatedVelocity.x - m_rbRigidbody.velocity.x;
        appliedForce.y = 0;
        appliedForce.z = rotatedVelocity.z - m_rbRigidbody.velocity.z;

        appliedForce *= m_fSpeedScalar;

        // Pushes to in-engine physics
        m_rbRigidbody.AddForce(appliedForce);
    }

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
            desiredVelocity.x = m_fForwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") < 0)
            desiredVelocity.x = -m_fBackwardSpeed * m_fSprint;
        if (Input.GetAxis("Vertical") == 0)
            desiredVelocity.x = 0;

        desiredVelocity.y = Input.GetAxis("Horizontal") * m_fStrafeSpeed * m_fSprint;

        if (desiredVelocity == Vector2.zero)
        {
            m_aModelAnimator.SetBool("Moving", false);
            if (m_fStepTimer != 0)
            {
            //   footStepManager.MakeSound();
                m_fStepTimer = 0;
            }
        }
        else
        {
            m_aModelAnimator.SetBool("Moving", true);
            m_fStepTimer += Time.deltaTime;
        }

        Vector3 desiredPosition = Vector3.ClampMagnitude(
            new Vector3(desiredVelocity.x, 0, desiredVelocity.y),
            m_fForwardSpeed * Time.deltaTime * m_fSprint);

        transform.position += transform.forward * desiredPosition.x * m_fSpeedMultiplier;
        transform.position += transform.right * desiredPosition.z * m_fSpeedMultiplier;

        if (m_fStepTimer > m_fStepDelay / m_fSprint)
        {
        //    footStepManager.MakeSound();
            m_fStepTimer = 0;
        }

        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, -0.9f, 0),
            new Vector3(0, -0.2f, 0), Color.red);

        if (Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0),
            Vector3.down, out hit, 0.2f) && Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
        }
    }
}