using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFPS : MonoBehaviour
{
    int m_iFrameCap = 60;

	private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_iFrameCap;
        //Debug.Log("Capped");
    }
}
