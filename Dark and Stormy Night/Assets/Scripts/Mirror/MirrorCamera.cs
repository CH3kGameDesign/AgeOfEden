using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour {

	public Transform playerCamera;
	public Transform renderPlane;
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 playerOffsetFromPortal = playerCamera.position - renderPlane.position;
		transform.position = new Vector3(renderPlane.position.x - playerOffsetFromPortal.x, renderPlane.position.y + playerOffsetFromPortal.y, renderPlane.position.z - playerOffsetFromPortal.z);

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(renderPlane.rotation, renderPlane.rotation * Quaternion.Euler(0, 180, 0));

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = (portalRotationalDifference * playerCamera.forward);
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        
		GetComponent<Camera> ().fieldOfView = CameraMovement.cameraObject.transform.GetChild (0).GetComponent<Camera> ().fieldOfView;
	}
}
