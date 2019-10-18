using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraEnabler : MonoBehaviour
{
	public Renderer[] RenderPlanes;
	public GameObject[] Cameras;

    //[SerializeField, Tooltip("Scales the distance at which the plane will no longer be active")]
    //private float m_fFarClipDistance = 10.0f;

    // Memory storage for the camera frustum
    private Plane[] m_pPlanes = new Plane[6];
    // Local reference to the players camera
    private Camera m_cPlayerCamera;

    private void Start()
    {
        m_cPlayerCamera = CameraMovement.s_CameraObject.transform.GetChild(0)
            .GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // NOTE: Look into - potential fps drain

		//Plane[] planes = GeometryUtility.CalculateFrustumPlanes(
        //  CameraMovement.s_CameraObject.transform.GetChild(0).GetComponent<Camera>());

		//for (int i = 0; i < RenderPlanes.Length; i++)
        //{
		//	if (GeometryUtility.TestPlanesAABB (planes, RenderPlanes[i].bounds))
		//		Cameras[i].SetActive(true);
        //  else
		//		Cameras[i].SetActive(false);
		//}

        m_pPlanes = GeometryUtility.CalculateFrustumPlanes(m_cPlayerCamera);
        //m_pPlanes[5].Translate(m_pPlanes[5].normal * (m_cPlayerCamera.farClipPlane
        //    * m_fFarClipDistance));

        for (int i = 0; i < RenderPlanes.Length; i++)
        {
            if (GeometryUtility.TestPlanesAABB(m_pPlanes, RenderPlanes[i].bounds))
                Cameras[i].SetActive(true);
            else
                Cameras[i].SetActive(false);
        }
    }
}