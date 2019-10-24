using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
	public Transform playerCamera;
	public Transform renderPlane;

    private Camera m_cLocalCamera;
    private Camera m_cPlayerCamera;

    private Transform m_tTransCache;

    private void Start()
    {
        m_cLocalCamera = GetComponent<Camera>();
        m_cPlayerCamera = CameraMovement.s_CameraObject.transform.GetChild(0)
            .GetComponent<Camera>();

        m_tTransCache = transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
		Vector3 playerOffsetFromPortal = playerCamera.position - renderPlane.position;
        m_tTransCache.position = new Vector3(renderPlane.position.x - playerOffsetFromPortal.x,
            renderPlane.position.y + playerOffsetFromPortal.y,
            renderPlane.position.z - playerOffsetFromPortal.z);

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(
            renderPlane.rotation, renderPlane.rotation * Quaternion.Euler(0, 180, 0));

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(
            angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        m_tTransCache.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

        m_cLocalCamera.fieldOfView = m_cPlayerCamera.fieldOfView;
	}
}