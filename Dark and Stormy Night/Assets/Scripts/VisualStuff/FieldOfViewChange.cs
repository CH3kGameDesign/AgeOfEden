using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewChange : MonoBehaviour {

    public float tarFOV;
    private float startFOV;

    public float lerpSpeed;

    private Camera mainCam;

    public enum FOVVar { outwards, back, none}
    public FOVVar FOVChange;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        startFOV = mainCam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
        if (FOVChange == FOVVar.outwards)
            ZoomOut();
        if (FOVChange == FOVVar.back)
            ZoomBack();
	}

    void ZoomOut ()
    {
        CameraMovement.canZoom = false;
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, tarFOV, lerpSpeed * Time.deltaTime);
        if (Mathf.Abs(mainCam.fieldOfView - tarFOV) < 0.5f)
        {
            FOVChange = FOVVar.back;
        }
    }

    void ZoomBack ()
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, startFOV, lerpSpeed * Time.deltaTime);
        if (Mathf.Abs(mainCam.fieldOfView - startFOV) < 0.5f)
        {
            mainCam.fieldOfView = startFOV;
            FOVChange = FOVVar.none;
            CameraMovement.canZoom = true;
        }
    }
}
