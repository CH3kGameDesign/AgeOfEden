﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementCameraHook : MonoBehaviour
{
    private Transform cameraTrans;
    private Transform cameraHook;

    // Use this for initialization
    private void Start ()
    {
        cameraTrans = CameraMovement.s_CameraObject.transform;
        cameraHook = cameraTrans.GetComponent<CameraMovement>().m_tCameraHook;

        cameraTrans.GetComponent<CameraMovement>().m_tCameraHook = transform;
        SmoothCameraMovement.s_fTurnAroundValue = 180;
    }
	
    private void OnDisable()
    {
        cameraTrans.GetComponent<CameraMovement>().m_tCameraHook = cameraHook;
        SmoothCameraMovement.s_fTurnAroundValue = 0;
    }
}
