using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTunnel : MonoBehaviour
{
    internal enum TunnelMode
    {
        PlayerAndObjects,
        ObjectsOnly,
        PlayerOnly
    };

    public static bool s_bInGravityTunnel = false;

    [Tooltip("The direction the rotation will occur in when walking through (based on facing positive x)")]
    [SerializeField]
    private bool m_bClockwise = true;

    [SerializeField]
    private TunnelMode m_tmTunnelMode = TunnelMode.PlayerAndObjects;

    // The total length of the tunnel
    private float m_fTunnelLength;
    // How far in the tunnel the player is
    private float m_fDistance;
    // The current gravity rotation
    private Vector2 m_v2CurrentGravity;

    [Tooltip("Used to determine the desired player rotation")]
    [SerializeField]
    private Transform m_tPendulum;

    [Tooltip("A list of gameobjects that are inside the tunnel")]
    public List<GameObject> m_goTunnelObjects = new List<GameObject>();

    //[Space(20)]

    //public Transform pointA;
    //public Transform pointB;

    //public float gravA;
    //public float gravB;

    //private float gravCurrent;

    //private float pointDistance;

    //public bool affectPlayer;
    //public bool affectObjects;

	// Use this for initialization
	private void Start()
    {
        // Initialises variables
        m_v2CurrentGravity = new Vector2(Physics.gravity.x, Physics.gravity.y);
        m_fTunnelLength = gameObject.GetComponent<BoxCollider>().size.z;


        //pointDistance = Vector3.Distance(pointA.position, pointB.position);
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = true;
            
            //if (Vector3.Distance(Movement.player.transform.position, pointA.position)
            //    < Vector3.Distance(Movement.player.transform.position, pointB.position))
            //{
            //    SmoothCameraMovement.gravSnap(gravB);
            //}
            //else
            //    SmoothCameraMovement.gravSnap(gravB);
        }

        if (other.tag == "Movable" && other.GetComponent<Rigidbody>())
        {
            m_goTunnelObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_v2CurrentGravity.x = Mathf.Sin(2 * Mathf.PI * m_fDistance / m_fTunnelLength)
                * Physics.gravity.y;
            m_v2CurrentGravity.y = Mathf.Cos(2 * Mathf.PI * m_fDistance / m_fTunnelLength)
                * -Physics.gravity.y - Physics.gravity.y;

            if (!m_bClockwise)
                m_v2CurrentGravity.x *= -1;

            if (m_tmTunnelMode == TunnelMode.PlayerAndObjects
                || m_tmTunnelMode == TunnelMode.PlayerOnly)
            {
                Movement.m_goPlayerObject.GetComponent<Rigidbody>().AddForce(
                    new Vector3(m_v2CurrentGravity.x, m_v2CurrentGravity.y));

                SmoothCameraMovement.originalRotation = Quaternion.Lerp(
                    SmoothCameraMovement.originalRotation,
                    m_tPendulum.localRotation, Time.deltaTime * 2);
            }

            if (m_tmTunnelMode == TunnelMode.PlayerAndObjects
                || m_tmTunnelMode == TunnelMode.ObjectsOnly)
            {
                for (int i = 0; i < m_goTunnelObjects.Count; i++)
                {
                    m_goTunnelObjects[i].GetComponent<Rigidbody>().AddForce(
                        new Vector3(m_v2CurrentGravity.x, m_v2CurrentGravity.y));
                }
            }

            //----------------------------------------------------------------------------

            //float distanceToA = Vector3.Distance(Movement.m_goPlayerObject.transform.position,
            //    pointA.position);
            //float distanceToB = Vector3.Distance(Movement.m_goPlayerObject.transform.position,
            //    pointB.position);

            //distanceToA /= pointDistance;
            //distanceToB /= pointDistance;

            //gravCurrent = ((gravA * distanceToB) + (gravB * distanceToA));
            //m_tPendulum.localEulerAngles = new Vector3(0, 0, gravCurrent);

            //Vector3 gravDirection = m_tPendulum.GetChild(0).position - m_tPendulum.position;

            //if (affectPlayer)
            //{
            //    Movement.m_goPlayerObject.GetComponent<Rigidbody>().useGravity = false;
            //    Movement.m_goPlayerObject.GetComponent<Rigidbody>().AddForce(
            //        gravDirection * 9.2f, ForceMode.Acceleration);

            //    SmoothCameraMovement.originalRotation = Quaternion.Lerp(
            //        SmoothCameraMovement.originalRotation,
            //        m_tPendulum.localRotation, Time.deltaTime * 2);

            //    //SmoothCameraMovement.gravSnap(360 - gravCurrent);
            //    //SmoothCameraMovement.gravDirection = 360 - gravCurrent;
            //    //SmoothCameraMovement.gravDirection = Pendulum.localEulerAngles.z;
            //}

            //if (affectObjects)
            //{
            //    for (int i = 0; i < m_goTunnelObjects.Count; i++)
            //    {
            //        m_goTunnelObjects[i].GetComponent<Rigidbody>().useGravity = false;
            //        m_goTunnelObjects[i].GetComponent<Rigidbody>().AddForce(
            //            gravDirection * 9.2f, ForceMode.Acceleration);
            //    }
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            s_bInGravityTunnel = false;

            //if (affectPlayer)
            //{
            //    Movement.m_goPlayerObject.GetComponent<Rigidbody>().useGravity = true;
            //    //SmoothCameraMovement.gravSnap(0);

            //    if (Vector3.Distance(Movement.m_goPlayerObject.transform.position, pointA.position)
            //        < Vector3.Distance(Movement.m_goPlayerObject.transform.position, pointB.position))
            //        SmoothCameraMovement.gravDirection = gravA;
            //    else
            //        SmoothCameraMovement.gravDirection = gravB;
            //}
        }

        if (other.tag == "Movable")
        {
            m_goTunnelObjects.Remove(other.gameObject);

            //if (affectObjects)
            //    other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}