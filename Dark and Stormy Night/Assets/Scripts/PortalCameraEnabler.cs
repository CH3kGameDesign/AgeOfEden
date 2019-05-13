using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraEnabler : MonoBehaviour {

	public Renderer[] RenderPlanes;
	public GameObject[] Cameras;

	void Update () {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes (CameraMovement.cameraObject.transform.GetChild (0).GetComponent<Camera> ());
		for (int i = 0; i < RenderPlanes.Length; i++) {
			if (GeometryUtility.TestPlanesAABB (planes, RenderPlanes [i].bounds)) {
				Cameras [i].SetActive (true);
			} else {
				Cameras [i].SetActive (false);
			}
		}

	}
}

