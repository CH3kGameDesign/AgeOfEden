using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraEnabler : MonoBehaviour
{
	public Renderer[] RenderPlanes;
	public GameObject[] Cameras;

	private void Update()
    {
        // NOTE: Look into - potential fps drain

		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(
            CameraMovement.s_CameraObject.transform.GetChild(0).GetComponent<Camera>());

		for (int i = 0; i < RenderPlanes.Length; i++)
        {
			if (GeometryUtility.TestPlanesAABB (planes, RenderPlanes [i].bounds))
				Cameras[i].SetActive(true);
            else
				Cameras[i].SetActive(false);
		}
	}
}