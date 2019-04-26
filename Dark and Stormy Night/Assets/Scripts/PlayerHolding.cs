using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerHolding : MonoBehaviour {

    public float lookTimer;
    public float zoomAmount;

    private float lookTime;
    private float FOV = 0.7f;

    // Use this for initialization
    void Start () {
        FOV = 0.7f;
    }
	
	// Update is called once per frame
	void Update () {
        CameraMovement.goToPlayer = false;
        Movement.canMove = false;
        RaycastHit hit;

        DepthOfFieldModel.Settings DOFSettings = CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile.depthOfField.settings;

        if (Physics.Raycast(CameraMovement.cameraObject.transform.position, CameraMovement.cameraObject.transform.forward, out hit, 10, 1 << 9))
        {
            if (hit.transform.tag == "PlayerHolding")
            {
                if (lookTime > lookTimer)
                {
                    CameraMovement.goToPlayer = true;
                    lookTime = 0;
                    CameraMovement.cameraObject.GetComponentInChildren<Camera>().fieldOfView = 60 - (lookTime * zoomAmount);

                    DOFSettings.focusDistance = 3f;
                    CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile.depthOfField.settings = DOFSettings;
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
        CameraMovement.cameraObject.GetComponentInChildren<Camera>().fieldOfView = 60 - (lookTime * zoomAmount);

        
        CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile.depthOfField.settings = DOFSettings;
    }
}
