using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

    public bool turnAround = false;

    private void Start()
    {
        if (CameraMovement.s_CameraObject && !playerCamera)
            playerCamera = CameraMovement.s_CameraObject.transform.GetChild(0);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        playerOffsetFromPortal *= portal.localScale.x;
        playerOffsetFromPortal /= otherPortal.localScale.x;

        if (!turnAround)
        {
            transform.position = portal.position + playerOffsetFromPortal;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(
                portal.rotation, otherPortal.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(
                angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
        else
        {
            transform.position = portal.position + new Vector3(
                -playerOffsetFromPortal.x, playerOffsetFromPortal.y, -playerOffsetFromPortal.z);

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(
                portal.rotation, otherPortal.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(
                angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
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