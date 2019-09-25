using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityTunnel : MonoBehaviour
{
    /*
     * TODO:
     * Flesh out objects entering and exiting
     * Four different object lists:
     * 1. Locked in place
     * 2. Follows the players gravity
     * 3. Inverts the players gravity
     * 4. Inverted x axis of players gravity
     */

    //[System.Serializable]
    //private class ObjectGroup
    //{
    //    [SerializeField]
    //    internal List<GameObject> m_LgoObjects;
    //};

    [HideInInspector]
    public bool s_bInGravityTunnel = false;

    [Tooltip("The rotation of the room requires the player inside it, works better with no telomeres")]
    [SerializeField]
    private bool m_bRequiresPlayer = true;
    [Tooltip("The direction the world will rotate around the player")]
    [SerializeField]
    private bool m_bClockwise = true;
    [Tooltip("Applies a small amount of smoothing to the rotation")]
    [SerializeField]
    private bool m_bSmoothing;

    [Space(5)]
    [Tooltip("How much the object rotates by at the end of the tunnel (0-360)")]
    [SerializeField]
    private float m_fRotationAmount = 360;
    [Tooltip("A small grace period in the hitbox where small adjustments can be made")]
    [SerializeField]
    private float m_fTelomeres = 0.5f;
    [Tooltip("How gradual the rotation is applied to the gameobjects")]
    [SerializeField]
    private float m_fSmoothStrength = 10;
    
    // The total length of the tunnel
    private float m_fTunnelLength;
    // How far in the tunnel the player is
    private float m_fProgress;
    // The current rotation around the z axis of the room
    private float m_fCurrentRotation = 0f;
    // Smooths out the rotation
    private float m_fSmoothing;
    
    [Header("References")]

    [Tooltip("The tiggerbox used to lerp rotation over the Z axis")]
    [SerializeField]
    private BoxCollider m_bcTriggerBox = null;
    
    [Tooltip("A list of gameobjects with physics that rotate with the room")]
    [SerializeField]
    private List<GameObject> m_LgoPhysicsChildren = new List<GameObject>();
    
    [Tooltip("A list of gameobjects with physics that rotate perpendicularly to the player")]
    [SerializeField]
    private List<GameObject> m_LgoPerpPhysicsChildren = new List<GameObject>();
    
    [Tooltip("A list of gameobjects with physics that rotate inversely to the room")]
    [SerializeField]
    private List<GameObject> m_LgoInvPhysicsChildren = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
        // If no triggerbox is found, try to assign any attached or throw an error
        if (!m_bcTriggerBox)
            m_bcTriggerBox = gameObject.GetComponent<BoxCollider>();

        if (!m_bcTriggerBox)
            Debug.LogError("No BoxCollider found");

        // Starts the tunnel length as the length of the hitbox minus the telomeres at each end
        m_fTunnelLength = m_bcTriggerBox.size.z;

        if (m_fTelomeres > m_fTunnelLength * 0.5)
            m_fTelomeres = 0;

        m_fTunnelLength -= m_fTelomeres * 2;

        // Clamps the rotation amount between 0 and 360 then scales to 0-1
        if (m_fRotationAmount < 0)
            m_fRotationAmount = 0;
        if (m_fRotationAmount > 360)
            m_fRotationAmount = 360;

        m_fRotationAmount /= 360;
	}

    private void Update()
    {
        if (!(m_bRequiresPlayer && !s_bInGravityTunnel))
        {
            Vector3 triggerBoxGlobalPos = gameObject.transform.position + m_bcTriggerBox.center;

            // Calculates the players progression through the tunnel between 0 and 1 (extendeds outside)
            m_fProgress = (Movement.s_goPlayerObject.transform.position.z
                - triggerBoxGlobalPos.z + (m_fTunnelLength * 0.5f)) / m_fTunnelLength;
            
            // Clamps the players progress
            if (m_bRequiresPlayer)
            {
                if (m_fProgress < 0)
                    m_fProgress = 0;
                if (m_fProgress > 1)
                    m_fProgress = 1;
            }
            
            // Scales the progress to suit the desired end rotation
            m_fProgress *= m_fRotationAmount;

            // Uses the progression to test the expected rotation to the current rotation
            if (m_fCurrentRotation != (2 * Mathf.PI * m_fProgress))
            {
                // Calculates the rotation based on the new position
                float anticipatedRotation = 2 * Mathf.PI * m_fProgress;
                // Calculates the difference from the previous frame
                float rotDifference = anticipatedRotation - m_fCurrentRotation;
                // Rotates by the difference (why is it in deg??)
                if (m_bClockwise)
                    m_fSmoothing -= rotDifference * Mathf.Rad2Deg;
                else
                    m_fSmoothing += rotDifference * Mathf.Rad2Deg;
                
                // Logs the rotation for the next frame
                m_fCurrentRotation = anticipatedRotation;
            }

            // Applies a fake gravity to the list of gameobjects to follow the rooms rotation
            for (int i = 0; i < m_LgoPhysicsChildren.Count; i++)
            {
                if (m_bClockwise)
                {
                    m_LgoPhysicsChildren[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        Mathf.Cos(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
                else
                {
                    m_LgoPhysicsChildren[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(2 * Mathf.PI * m_fProgress)
                        * -Physics.gravity.y,
                        Mathf.Cos(2 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
            }

            // Applies an inverted gravity to the list of gameobjects to oppose but follow
            //the rooms rotation
            for (int i = 0; i < m_LgoPerpPhysicsChildren.Count; i++)
            {
                m_LgoPerpPhysicsChildren[i].GetComponent<Rigidbody>().AddForce(
                        new Vector2(-Physics.gravity.x, -Physics.gravity.y));
            }

            // Applies a perpendicular gravity to the list of gameobjects to contrast the rooms rotation
            for (int i = 0; i < m_LgoInvPhysicsChildren.Count; i++)
            {
                if (m_bClockwise)
                {
                    m_LgoInvPhysicsChildren[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(4 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        Mathf.Cos(4 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
                else
                {
                    m_LgoInvPhysicsChildren[i].GetComponent<Rigidbody>().AddForce(
                        Mathf.Sin(4 * Mathf.PI * m_fProgress)
                        * -Physics.gravity.y,
                        Mathf.Cos(4 * Mathf.PI * m_fProgress)
                        * Physics.gravity.y,
                        0);
                }
            }
        }

        // Do smoothing
        transform.Rotate(new Vector3(0, 0, m_fSmoothing / m_fSmoothStrength));
        m_fSmoothing -= m_fSmoothing / m_fSmoothStrength;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = true;

            for (int i = 0; i < m_LgoPhysicsChildren.Count; i++)
                m_LgoPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = false;

            for (int i = 0; i < m_LgoPerpPhysicsChildren.Count; i++)
                m_LgoPerpPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = false;

            for (int i = 0; i < m_LgoInvPhysicsChildren.Count; i++)
                m_LgoInvPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = false;
        }

        //if (other.tag == "Movable" && other.GetComponent<Rigidbody>())
        //    m_LgoPhysicsObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = false;

            for (int i = 0; i < m_LgoPhysicsChildren.Count; i++)
                m_LgoPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = true;

            for (int i = 0; i < m_LgoPerpPhysicsChildren.Count; i++)
                m_LgoPerpPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = true;

            for (int i = 0; i < m_LgoInvPhysicsChildren.Count; i++)
                m_LgoInvPhysicsChildren[i].GetComponent<Rigidbody>().useGravity = true;
        }

        //if (other.tag == "Movable")
        //    m_LgoPhysicsObjects.Remove(other.gameObject);
    }
}