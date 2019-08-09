using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTextureSetup : MonoBehaviour
{
    public Camera m_cCamera;

    public Material cameraMat;

	private void Start ()
    {
        if (m_cCamera.targetTexture)
            m_cCamera.targetTexture.Release();

        m_cCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMat.mainTexture = m_cCamera.targetTexture;
    }
}