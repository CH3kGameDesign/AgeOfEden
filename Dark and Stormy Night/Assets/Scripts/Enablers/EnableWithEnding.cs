using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWithEnding : MonoBehaviour {

    public List<GameObject> enableObjects = new List<GameObject>();
    public List<GameObject> disableObjects = new List<GameObject>();

    public int endingCompleted;

	// Use this for initialization
	void Start () {
		if (PermanentData.saveInfo.endingsAchieved[endingCompleted])
        {
            for (int i = 0; i < enableObjects.Count; i++)
                enableObjects[i].SetActive(true);
            for (int i = 0; i < disableObjects.Count; i++)
                disableObjects[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
