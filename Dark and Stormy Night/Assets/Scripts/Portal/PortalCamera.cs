using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

    public bool turnAround = false;

	// Update is called once per frame
	void Update () {
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        if (turnAround == false)
        {
            transform.position = portal.position + playerOffsetFromPortal;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
        else
        {
            transform.position = portal.position + new Vector3(-playerOffsetFromPortal.x, playerOffsetFromPortal.y, -playerOffsetFromPortal.z);

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
        if (InvertGravity.invertedGravity == true)
        {
            transform.localEulerAngles += new Vector3(0, 0, 180);
        }

		GetComponent<Camera> ().fieldOfView = CameraMovement.cameraObject.transform.GetChild (0).GetComponent<Camera> ().fieldOfView;
	}
}
