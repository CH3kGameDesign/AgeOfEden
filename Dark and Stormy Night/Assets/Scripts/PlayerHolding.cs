﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerHolding : MonoBehaviour
{
    public float lookTimer;
    public float zoomAmount;

    private float lookTime;
    private float FOV = 0.7f;

    private Camera m_cCamera;
    private PostProcessingBehaviour m_ppbPostProcessingRef;

    // Called once before the first frame
    private void Start()
    {
        FOV = 0.7f;

        m_cCamera = CameraMovement.s_CameraObject.GetComponentInChildren<Camera>();
        m_ppbPostProcessingRef = CameraMovement.s_CameraObject
            .GetComponentInChildren<PostProcessingBehaviour>();
    }
	
	// Update is called once per frame
	private void Update()
    {
        CameraMovement.s_bGoToPlayer = false;
        Movement.s_bCanMove = false;
        RaycastHit hit;

        DepthOfFieldModel.Settings DOFSettings = m_ppbPostProcessingRef.profile.depthOfField.settings;

        if (Physics.Raycast(CameraMovement.s_CameraObject.transform.position, 
            CameraMovement.s_CameraObject.transform.forward, out hit, 10, 1 << 9))
        {
            if (hit.transform.tag == "PlayerHolding")
            {
                if (lookTime > lookTimer)
                {
                    CameraMovement.s_bGoToPlayer = true;
                    lookTime = 0;
                    m_cCamera.fieldOfView = 60 - (lookTime * zoomAmount);

                    DOFSettings.focusDistance = 3f;
                    m_ppbPostProcessingRef.profile.depthOfField.settings = DOFSettings;

                    Destroy(hit.transform.gameObject);
                    return;
                }
                lookTime += Time.deltaTime;
                FOV = Mathf.Lerp(DOFSettings.focusDistance, 1, Time.deltaTime * 2);
                DOFSettings.focusDistance = FOV;
            }
            else
            {
                lookTime = Mathf.Lerp(lookTime, 0, 0.4f);
                FOV = Mathf.Lerp(DOFSettings.focusDistance, 3f, Time.deltaTime);
                DOFSettings.focusDistance = FOV;
            }
        }
        else
        {
            FOV = Mathf.Lerp(DOFSettings.focusDistance, 3f, Time.deltaTime);
            DOFSettings.focusDistance = FOV;
            lookTime = Mathf.Lerp(lookTime, 0, 0.4f);
        }

        m_cCamera.fieldOfView = 60 - (lookTime * zoomAmount);

        m_ppbPostProcessingRef.profile.depthOfField.settings = DOFSettings;
    }
}
