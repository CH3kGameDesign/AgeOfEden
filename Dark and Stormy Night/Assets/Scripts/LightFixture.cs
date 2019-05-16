﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixture : MonoBehaviour {

    [Header("GameObjects")]
    public List<Light> Lights = new List<Light>();
    public List<MeshRenderer> Fixtures = new List<MeshRenderer>();

    [Space (10)]

    [Header ("Variables")]

    public Vector2 flashOffTimesBounds;
    public Vector2 flashOnTimesBounds;
    public Vector2 timeBetweenSetsBounds;
    public Vector2 flashPerSetBounds;
    [Space (10)]
    private float setTimer = 0;
    private float flashTimer = 0;
    private int flashCounter = 0;

    private float flashOffTimes;
    private float flashOnTimes;
    private float timeBetweenSets;
    private int flashPerSet;

    [Header ("Materials")]

    public Material onMat;
    public Material offMat;
    [Space(10)]
    public float onAmount;
    public float offAmount;

    // Use this for initialization
    void Start () {
        if (Lights.Count >= 1)
            for (int i = 0; i < Lights.Count; i++)
            {
                Lights[i].intensity = onAmount;
            }
        else
            GetComponentInChildren<Light>().intensity = onAmount;

        if (Fixtures.Count >= 1)
            for (int i = 0; i < Fixtures.Count; i++)
            {
                Fixtures[i].material = onMat;
            }
        else
            GetComponent<MeshRenderer>().material = onMat;


        flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
        flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
        timeBetweenSets = Random.Range(timeBetweenSetsBounds.x, timeBetweenSetsBounds.y);
        flashPerSet = Mathf.RoundToInt(Random.Range(flashPerSetBounds.x, flashPerSetBounds.y));
    }
	
	// Update is called once per frame
	void Update () {
        if (setTimer > timeBetweenSets)
        {
            if (flashTimer < flashOffTimes)
            {
                if (Lights.Count >= 1)
                    for (int i = 0; i < Lights.Count; i++)
                    {
                        Lights[i].intensity = offAmount;
                    }
                else
                    GetComponentInChildren<Light>().intensity = offAmount;

                if (Fixtures.Count >= 1)
                    for (int i = 0; i < Fixtures.Count; i++)
                    {
                        Fixtures[i].material = offMat;
                    }
                else
                    GetComponent<MeshRenderer>().material = offMat;
            }
            else
            {
                if (Lights.Count >= 1)
                    for (int i = 0; i < Lights.Count; i++)
                    {
                        Lights[i].intensity = onAmount;
                    }
                else
                    GetComponentInChildren<Light>().intensity = onAmount;

                if (Fixtures.Count >= 1)
                    for (int i = 0; i < Fixtures.Count; i++)
                    {
                        Fixtures[i].material = onMat;
                    }
                else
                    GetComponent<MeshRenderer>().material = onMat;

                if (flashTimer > flashOffTimes + flashOnTimes)
                {
                    if (flashCounter < flashPerSet)
                    {
                        flashTimer = 0;
                        flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
                        flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
                        flashCounter++;
                    }
                    else
                    {
                        flashCounter = 0;
                        setTimer = 0;
                        timeBetweenSets = Random.Range(timeBetweenSetsBounds.x, timeBetweenSetsBounds.y);
                        flashPerSet = Mathf.RoundToInt(Random.Range(flashPerSetBounds.x, flashPerSetBounds.y));
                    }
                }
            }
        }
        else
        {
            flashTimer = 0;
        }
        setTimer += Time.deltaTime;
        flashTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("FPS: " + flashPerSet);
            Debug.Log("TBS: " + timeBetweenSets);
            Debug.Log("FOffT: " + flashOffTimes + "; FOnT: " + flashOnTimes);
        }
    }
}
