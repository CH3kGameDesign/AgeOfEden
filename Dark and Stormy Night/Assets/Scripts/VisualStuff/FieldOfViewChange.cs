using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewChange : MonoBehaviour {

    public float tarFOV;
    private float startFOV;

    public float lerpSpeed;
    private float lerpSpeedActual = 0;

    private Camera mainCam;

    public enum FOVVar { outwards, back, back2, none}
    public FOVVar FOVChange;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        startFOV = mainCam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
        lerpSpeedActual = Mathf.Lerp(lerpSpeedActual, lerpSpeed, Time.deltaTime);
        if (FOVChange == FOVVar.outwards)
            ZoomOut();
        if (FOVChange == FOVVar.back)
            ZoomBack();
        if (FOVChange == FOVVar.back2)
            ZoomBack2();
    }

    void ZoomOut ()
    {
        CameraMovement.canZoom = false;
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, tarFOV, lerpSpeedActual * Time.deltaTime);
        if (Mathf.Abs(mainCam.fieldOfView - tarFOV) < 0.5f)
        {
            FOVChange = FOVVar.back;
        }
    }

    void ZoomBack ()
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, startFOV - 25, lerpSpeedActual * Time.deltaTime);
        if (Mathf.Abs(mainCam.fieldOfView - (startFOV - 25)) < 0.5f)
        {
            mainCam.fieldOfView = startFOV - 25;
            FOVChange = FOVVar.back2;
        }
    }
    void ZoomBack2()
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, startFOV, lerpSpeed * Time.deltaTime/ 2);
        if (Mathf.Abs(mainCam.fieldOfView - startFOV) < 0.5f)
        {
            mainCam.fieldOfView = startFOV;
            FOVChange = FOVVar.none;
            CameraMovement.canZoom = true;
        }
    }
}
