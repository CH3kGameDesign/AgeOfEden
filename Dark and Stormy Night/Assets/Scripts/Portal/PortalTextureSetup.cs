using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PortalTextureSetup : MonoBehaviour
{
    [FormerlySerializedAs("cameras")]
    public List<Camera> m_LcCameras = new List<Camera>();
    [FormerlySerializedAs("cameraMats")]
    public List<Material> m_LmCameraMats = new List<Material>();

    private void Start()
    {
        for (int i = 0; i < m_LcCameras.Count; i++)
        {
            if (m_LcCameras[i].targetTexture != null)
                m_LcCameras[i].targetTexture.Release();

            m_LcCameras[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            m_LmCameraMats[i].mainTexture = m_LcCameras[i].targetTexture;
        }
    }
}