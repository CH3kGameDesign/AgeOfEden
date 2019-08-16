using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [Header("Variables")]
    public bool m_bCanPickup = true;
    [SerializeField]
    private float m_fPickupDistance;
    [SerializeField]
    private Vector3 m_v3ObjectOffsetFromPlayer;

    private CameraMovement m_cmCameraMovement;
    
    private Transform m_tInteractObject;

    private void Start()
    {
        m_cmCameraMovement = GetComponent<CameraMovement>();
    }

    private void Update()
    {
        if (!m_bCanPickup)
            return;

        // Simluates the ray being drawn
        Debug.DrawLine(m_cmCameraMovement.m_tCameraHook.transform.position,
            new Vector3(m_cmCameraMovement.m_tCameraHook.transform.position.x,
            m_cmCameraMovement.m_tCameraHook.transform.position.y,
            m_cmCameraMovement.m_tCameraHook.transform.position.z + m_fPickupDistance),
            Color.red);

        // Tests for an object under players vision
        RaycastHit ray;
        if (Physics.Raycast(m_cmCameraMovement.m_tCameraHook.transform.position,
            transform.forward, out ray, m_fPickupDistance))
        {
            // If the object can be moved, assign it
            if (ray.transform.gameObject.GetComponent<Rigidbody>())
            {
                if (!ray.transform.gameObject.GetComponent<Rigidbody>().isKinematic)
                    m_tInteractObject = ray.transform;
            }
        }

        /*
         * Moves a transform a set distance in front of the camera
         * Does a raycast to look for any small object in front of the player
         * If they interact
         * Move the object to the transform
         * Use physics to avoid clipping
         * To avoid overshooting, scale by distance and velocity
         */
    }
}