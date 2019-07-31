using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public bool m_bTurnAround = false;

	public Transform m_tPlayerCamera;
	public Transform m_tPortal;
	public Transform m_tOtherPortal;

    // Called once before the first frame
    private void Start ()
    {
        if (CameraMovement.cameraObject != null)
            m_tPlayerCamera = CameraMovement.cameraObject.transform.GetChild(0);
    }

    // Update is called once per frame
    private void LateUpdate ()
    {
		Vector3 playerOffsetFromPortal = m_tPlayerCamera.position - m_tOtherPortal.position;
        if (!m_bTurnAround)
        {
            transform.position = m_tPortal.position + playerOffsetFromPortal;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(m_tPortal.rotation, m_tOtherPortal.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * m_tPlayerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
        else
        {
            transform.position = m_tPortal.position + new Vector3(-playerOffsetFromPortal.x, playerOffsetFromPortal.y, -playerOffsetFromPortal.z);

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(m_tPortal.rotation, m_tOtherPortal.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * m_tPlayerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }

        if (InvertGravity.m_bInvertedGravity)
            transform.localEulerAngles += new Vector3(0, 0, 180);

		GetComponent<Camera> ().fieldOfView = CameraMovement.cameraObject.transform.GetChild (0).GetComponent<Camera> ().fieldOfView;
	}
}
