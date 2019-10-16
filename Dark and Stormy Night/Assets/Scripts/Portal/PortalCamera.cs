using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PortalCamera : MonoBehaviour
{
    [FormerlySerializedAs("playerCamera")]
	public Transform m_tPlayerCamera;
    [FormerlySerializedAs("portal")]
    public Transform m_tPortal;
    [FormerlySerializedAs("otherPortal")]
    public Transform m_tOtherPortal;

    public bool turnAround = false;

    private void Start()
    {
        if (CameraMovement.s_CameraObject && !m_tPlayerCamera)
            m_tPlayerCamera = CameraMovement.s_CameraObject.transform.GetChild(0);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
		Vector3 playerOffsetFromPortal = m_tPlayerCamera.position - m_tOtherPortal.position;
        playerOffsetFromPortal *= m_tPortal.localScale.x;
        playerOffsetFromPortal /= m_tOtherPortal.localScale.x;

        if (!turnAround)
        {
            transform.position = m_tPortal.position + playerOffsetFromPortal;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(
                m_tPortal.rotation, m_tOtherPortal.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(
                angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * m_tPlayerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
        else
        {
            transform.position = m_tPortal.position + new Vector3(
                -playerOffsetFromPortal.x, playerOffsetFromPortal.y, -playerOffsetFromPortal.z);

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(
                m_tPortal.rotation, m_tOtherPortal.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(
                angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * m_tPlayerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }

        if (InvertGravity.invertedGravity)
        {
            transform.localEulerAngles += new Vector3(0, 0, 180);
        }

		GetComponent<Camera> ().fieldOfView = CameraMovement.s_CameraObject.transform
            .GetChild(0).GetComponent<Camera> ().fieldOfView;
	}
}