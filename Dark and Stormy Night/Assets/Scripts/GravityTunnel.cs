using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityTunnel : MonoBehaviour
{
    /*
     * TODO:
     * Smoothing, currently entering the hitbox immediately starts the rotation, try easing into it
     * Half rotation, currently the room rotates a full 360 throughout the length, add option to reduce
     * Have objects be able to enter and exit 
     */

    public static bool s_bInGravityTunnel = false;

    [Tooltip("The rotation of the room requires the player inside it, works better with no telomeres")]
    [SerializeField]
    private bool m_bRequiresPlayer = true;
    [Tooltip("The direction the world will rotate around the player")]
    [SerializeField]
    private bool m_bClockwise = true;
    
    [Tooltip("A small grace period in the hitbox where small adjustments can be made")]
    [SerializeField]
    private float m_fTelomeres = 0.5f;
    
    // The total length of the tunnel
    private float m_fTunnelLength;
    // How far in the tunnel the player is
    private float m_fProgress;
    // The current rotation around the z axis of the room
    private float m_fCurrentRotation = 0f;

    [Space(5)]
    [Header("References")]

    [Tooltip("The tiggerbox used to lerp rotation over the Z axis")]
    [SerializeField]
    private BoxCollider m_bcTriggerBox = null;
    [Tooltip("A list of loose gameobjects with physics that rotate with the room")]
    [FormerlySerializedAs("m_LgoPhysicsObjects")]
    [SerializeField]
    private List<GameObject> m_LgoPhysicsObjects = new List<GameObject>();

    [System.Serializable]
    private struct ObjectGroup
    {
        [SerializeField]
        private List<GameObject> m_LgoObjects;
    };

    [Tooltip("A list of gameobjects that can be use to add them to the physics objects list")]
    [SerializeField]
    private List<ObjectGroup> m_LgoObjectStandby = new List<ObjectGroup>();

    // Use this for initialization
    private void Start()
    {
        if (m_fTelomeres > m_fTunnelLength * 0.5)
            m_fTelomeres = 0;

        // If no triggerbox is found, try to assign any attached or throw an error
        if (!m_bcTriggerBox)
            m_bcTriggerBox = gameObject.GetComponent<BoxCollider>();
        else
            Debug.LogWarning("No BoxCollider found");

        // Starts the tunnel length as the length of the hitbox minus the telomeres at each end
        m_fTunnelLength = m_bcTriggerBox.size.z;

        m_fTunnelLength -= m_fTelomeres * 2;
	}

    private void Update()
    {
        if (!(m_bRequiresPlayer && !s_bInGravityTunnel))
        {
            Vector3 triggerBoxGlobalPos = gameObject.transform.position + m_bcTriggerBox.center;

            // Calculates the players progression through the tunnel between 0 and 1 (extendeds outside)
            m_fProgress = (Movement.m_goPlayerObject.transform.position.z
                - triggerBoxGlobalPos.z + (m_fTunnelLength * 0.5f)) / m_fTunnelLength;
            
            // Clamps the players progress
            if (m_bRequiresPlayer)
            {
                if (m_fProgress < 0)
                    m_fProgress = 0;
                if (m_fProgress > 1)
                    m_fProgress = 1;
            }

            // Uses the progression to test the expected rotation to the current rotation
            if (m_fCurrentRotation != (2 * Mathf.PI * m_fProgress))
            {
                // Calculates the rotation based on the new position
                float anticipatedRotation = 2 * Mathf.PI * m_fProgress;
                // Calculates the difference from the previous frame
                float rotDifference = anticipatedRotation - m_fCurrentRotation;
                // Rotates by the difference (why is it in deg??)
                if (m_bClockwise)
                    transform.Rotate(new Vector3(0, 0, -rotDifference * Mathf.Rad2Deg));
                else
                    transform.Rotate(new Vector3(0, 0, rotDifference * Mathf.Rad2Deg));
                // Logs the rotation for the next frame
                m_fCurrentRotation = anticipatedRotation;
            }

            // Applies a fake gravity to the list of gameobjects
            for (int i = 0; i < m_LgoPhysicsObjects.Count; i++)
            {
                if (m_bClockwise)
                {
                    m_LgoPhysicsObjects[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        Mathf.Cos(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
                else
                {
                    m_LgoPhysicsObjects[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(2 * Mathf.PI * m_fProgress)
                        * -Physics.gravity.y,
                        Mathf.Cos(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = true;

            for (int i = 0; i < m_LgoPhysicsObjects.Count; i++)
                m_LgoPhysicsObjects[i].GetComponent<Rigidbody>().useGravity = false;
        }

        //if (other.tag == "Movable" && other.GetComponent<Rigidbody>())
        //    m_LgoPhysicsObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = false;

            for (int i = 0; i < m_LgoPhysicsObjects.Count; i++)
                m_LgoPhysicsObjects[i].GetComponent<Rigidbody>().useGravity = true;
        }

        //if (other.tag == "Movable")
        //    m_LgoPhysicsObjects.Remove(other.gameObject);
    }
}