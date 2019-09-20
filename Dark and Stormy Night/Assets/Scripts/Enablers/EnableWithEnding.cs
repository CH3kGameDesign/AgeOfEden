﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableWithEnding : MonoBehaviour
{
    public List<GameObject> enableObjects = new List<GameObject>();
    public List<GameObject> disableObjects = new List<GameObject>();
    public UnityEvent activateEvent;
    [Space (10)]
    public int endingCompleted;
    [Space(10)]
    public List<GameObject> enableIfNotObjects = new List<GameObject>();
    public bool lastCompleted = false;

	// Use this for initialization
	private void Start()
    {
        if (!lastCompleted)
        {
            if (PermanentData.saveInfo.endingsAchieved[endingCompleted])
            {
                for (int i = 0; i < enableObjects.Count; i++)
                    enableObjects[i].SetActive(true);
                for (int i = 0; i < disableObjects.Count; i++)
                    disableObjects[i].SetActive(false);
                activateEvent.Invoke();
            }
            else
            {
                for (int i = 0; i < enableIfNotObjects.Count; i++)
                    enableIfNotObjects[i].SetActive(true);
            }
        }
        else
        {
            if (PermanentData.saveInfo.lastEndingAchieved == endingCompleted)
            {
                for (int i = 0; i < enableObjects.Count; i++)
                    enableObjects[i].SetActive(true);
                for (int i = 0; i < disableObjects.Count; i++)
                    disableObjects[i].SetActive(false);
                activateEvent.Invoke();
            }
            else
            {
                for (int i = 0; i < enableIfNotObjects.Count; i++)
                    enableIfNotObjects[i].SetActive(true);
            }
        }
    }
}