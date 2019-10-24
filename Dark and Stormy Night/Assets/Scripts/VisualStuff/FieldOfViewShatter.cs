using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewShatter : MonoBehaviour
{
    public float baseFOV;
    public float FOVDif;
    public Vector2 changeTimer;
    private float currentTimeLimit;
    private float timer;
    private float tarFOV;
    //private float startFOV;

    public float lerpSpeed;
    private float lerpSpeedActual = 0;

    private Camera mainCam;
    
	// Use this for initialization
	private void Start()
    {
        mainCam = Camera.main;
        //startFOV = mainCam.fieldOfView;
	}
	
	// Update is called once per frame
	private void Update()
    {
        lerpSpeedActual = Mathf.Lerp(lerpSpeedActual, lerpSpeed, Time.deltaTime);
        if (timer >= currentTimeLimit)
        {
            currentTimeLimit = Random.Range(changeTimer.x, changeTimer.y);
            timer = 0;

            float tarFOV2 = baseFOV + Random.Range(-FOVDif, FOVDif);
            if (Mathf.Abs(tarFOV - tarFOV2) < FOVDif / 3)
            {
                if (tarFOV2 - baseFOV > 0)
                    tarFOV2 -= FOVDif;
                else
                    tarFOV2 += FOVDif;
            }
            tarFOV = tarFOV2;
        }
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, tarFOV, lerpSpeedActual * Time.deltaTime);
        timer += Time.deltaTime;
    }
}