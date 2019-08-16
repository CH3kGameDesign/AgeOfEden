﻿using System.Collections;
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
    private Vector3 m_v3ObjectOffsetFromPlayer = new Vector3(0, -0.1f, 0.25f);

    private CameraMovement m_cmCameraMovement;

    // Variables for the held object
    private bool m_bObjectGravityState;
    [SerializeField]
    private Vector3 m_v3ObjectRelativePos;
    [SerializeField]
    private Transform m_tObjectTransform;
    [SerializeField]
    private Rigidbody m_rbObjectRb;

    private void Start()
    {
        m_cmCameraMovement = GetComponent<CameraMovement>();
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

                Vector3 translatedPosition = transform.forward * m_fPickupDistance +
                    m_cmCameraMovement.transform.position;

                Vector3 relPos = translatedPosition - m_tObjectTransform.position
                     - m_rbObjectRb.velocity * 0.5f;

                Vector3 appliedForce = (relPos * (relPos.magnitude * 0.2f + 1) 
                    * (10000 * m_fHoldSpeedMult) / m_rbObjectRb.mass);

                m_rbObjectRb.AddForce(appliedForce);

                m_v3ObjectRelativePos = m_cmCameraMovement.transform.position
                    - m_tObjectTransform.position;
            }
            else
            {
                // Simulates the ray
                Debug.DrawLine(m_cmCameraMovement.m_tCameraHook.transform.position,
                    transform.forward * m_fPickupDistance +
                    m_cmCameraMovement.transform.position,
                    Color.red);

                // Tests for an object under players vision
                RaycastHit ray;
                if (Physics.Raycast(m_cmCameraMovement.m_tCameraHook.transform.position,
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
        m_tObjectTransform.position = m_cmCameraMovement.transform.position
            + m_v3ObjectRelativePos;
    }
}