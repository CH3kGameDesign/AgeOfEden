using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptInParent : MonoBehaviour
{
    private SceneChanger m_scSceneChanger = null;

    private void Start()
    {
        m_scSceneChanger = GetComponentInParent<SceneChanger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (m_scSceneChanger != null)
                m_scSceneChanger.StartLoad();
        }
    }
}