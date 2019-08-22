using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptInParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GetComponentInParent<SceneChanger>() != null)
                GetComponentInParent<SceneChanger>().StartLoad();
        }
    }
}
