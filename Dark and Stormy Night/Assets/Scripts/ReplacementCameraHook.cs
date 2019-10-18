using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementCameraHook : MonoBehaviour
{
    private Transform cameraTrans;
    private Transform cameraHook;

    private CameraMovement m_cmScriptRef;

    // Use this for initialization
    private void Start ()
    {
        cameraTrans = CameraMovement.s_CameraObject.transform;
        m_cmScriptRef = cameraTrans.GetComponent<CameraMovement>();
        cameraHook = m_cmScriptRef.m_tCameraHook;

        m_cmScriptRef.m_tCameraHook = transform;
        SmoothCameraMovement.s_fTurnAroundValue = 180;
    }
	
    private void OnDisable()
    {
        m_cmScriptRef.m_tCameraHook = cameraHook;
        SmoothCameraMovement.s_fTurnAroundValue = 0;
    }
}
