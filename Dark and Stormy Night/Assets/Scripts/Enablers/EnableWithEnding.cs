using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWithEnding : MonoBehaviour
{
    public List<GameObject> enableObjects = new List<GameObject>();
    public List<GameObject> disableObjects = new List<GameObject>();
    [Space (10)]
    public int endingCompleted;
    [Space(10)]
    public List<GameObject> enableIfNotObjects = new List<GameObject>();

	// Use this for initialization
	private void Start()
    {
        if (PermanentData.saveInfo.endingsAchieved[endingCompleted])
        {
            for (int i = 0; i < enableObjects.Count; i++)
                enableObjects[i].SetActive(true);
            for (int i = 0; i < disableObjects.Count; i++)
                disableObjects[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < enableIfNotObjects.Count; i++)
                enableIfNotObjects[i].SetActive(true);
        }
    }
}