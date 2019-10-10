using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterEnabler : MonoBehaviour
{
    public void Change(bool value)
    {
        CameraMovement.s_CameraObject.GetComponent<CameraMovement>()
            .underWaterQuad.gameObject.SetActive(value);
    }
}
