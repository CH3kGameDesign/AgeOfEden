using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEnable : MonoBehaviour
{
    public List<GameObject> enableObjects = new List<GameObject>();
    
	// Update is called once per frame
	private void Update ()
    {
		if (Input.anyKeyDown)
        {
            for (int i = 0; i < enableObjects.Count; i++)
                enableObjects[i].SetActive(true);
        }
	}
}
