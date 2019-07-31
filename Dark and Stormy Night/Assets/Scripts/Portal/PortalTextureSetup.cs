using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public List<Camera> m_lCameras = new List<Camera>();
    public List<Material> m_lCameraMats = new List<Material>();

	private void Start ()
    {
        for (int i = 0; i < m_lCameras.Count; i++)
        {
            if (m_lCameras[i].targetTexture != null)
                m_lCameras[i].targetTexture.Release();

            m_lCameras[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            m_lCameraMats[i].mainTexture = m_lCameras[i].targetTexture;
        }
	}
}
