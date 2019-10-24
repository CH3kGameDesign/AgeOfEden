using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [Header("Variables")]
    public bool m_bCanPickup = true;
    [SerializeField]
    private float m_fPickupDistance = 1.75f;
    public float m_fHoldSpeedMult = 5;
    [SerializeField]
    private Vector2 m_v3ObjectOffsetFromPlayer = new Vector2(0.75f, -0.1f);

    private CameraMovement m_cmCameraMovement;

    // Variables for the held object
    private bool m_bObjectGravityState;
    private Vector3 m_v3ObjectRelativePos;
    private Transform m_tObjectTransform;
    private Rigidbody m_rbObjectRb;

    private Transform m_tCameraTrans;
    private Transform m_tCameraHookTrans;
    //m_tTransCache

    private void Start()
    {
        m_cmCameraMovement = GetComponent<CameraMovement>();
        m_tCameraTrans = m_cmCameraMovement.transform;
        m_tCameraHookTrans = m_cmCameraMovement.m_tCameraHook.transform;
    }

    private void Update()
    {
        if (!m_bCanPickup)
        {
            LetGo();
            return;
        }
        
        // When the player presses the left mouse button
        if (Input.GetAxis("Fire1") > 0)
        {
            // If they are already holding an object
            if (m_tObjectTransform)
            {
                // Moves the object towards the desired position
                // Gets the relative position of the object from the desired position

                Vector3 translatedPosition = transform.forward * m_v3ObjectOffsetFromPlayer.x
                    + new Vector3(0, m_v3ObjectOffsetFromPlayer.y)
                    + m_tCameraTrans.position;

                Vector3 relPos = translatedPosition - m_tObjectTransform.position
                     - m_rbObjectRb.velocity * 0.5f;

                Vector3 appliedForce = (relPos * (relPos.magnitude * 0.2f + 1) 
                    * (10000 * m_fHoldSpeedMult) / m_rbObjectRb.mass);

                m_rbObjectRb.AddForce(appliedForce);

                m_v3ObjectRelativePos = m_tCameraTrans.position
                    - m_tObjectTransform.position;
            }
            else
            {
                // Simulates the ray
                Debug.DrawLine(m_tCameraHookTrans.position, transform.forward * m_fPickupDistance +
                    m_tCameraTrans.position, Color.red);

                // Tests for an object under players vision
                RaycastHit ray;
                if (Physics.Raycast(m_tCameraHookTrans.position,
                    transform.forward, out ray, m_fPickupDistance))
                {
                    // If the object can be moved
                    if (ray.transform.GetComponent<Rigidbody>()
                        &&  !ray.transform.GetComponent<Rigidbody>().isKinematic)
                    {
                        // Assigns the rays object
                        m_tObjectTransform = ray.transform;
                        m_rbObjectRb = ray.transform.GetComponent<Rigidbody>();

                        // Toggles gravity
                        m_bObjectGravityState = m_tObjectTransform.gameObject.
                            GetComponent<Rigidbody>().useGravity;
                        m_rbObjectRb.useGravity = false;
                    }
                }
            }
        }
        else if (m_tObjectTransform)
        {
            // When LMB released the object is reset and de-referenced
            LetGo();
        }
    }

    /// <summary>
    /// Call to drop anything held
    /// </summary>
    public void LetGo()
    {
        if (m_tObjectTransform)
            m_rbObjectRb.useGravity = m_bObjectGravityState;

        m_v3ObjectRelativePos = new Vector3();
        m_rbObjectRb = null;
        m_tObjectTransform = null;
    }

    public void TeleportToPlayer()
    {
        m_tObjectTransform.position = m_tCameraTrans.position
            + m_v3ObjectRelativePos;
    }
}