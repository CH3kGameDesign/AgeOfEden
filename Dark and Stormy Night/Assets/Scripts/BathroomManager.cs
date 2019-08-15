using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomManager : MonoBehaviour {
    public List<Transform> bathroomDoorPivots = new List<Transform>();
    [HideInInspector]
    public Transform bathroom;

    private int counter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        counter++;
        if (counter > 5)
        {
            if (bathroom != null)
            {
                int choice = 0;
                float dist = 100;
                for (int i = 0; i < bathroomDoorPivots.Count; i++)
                {
                    float dist2 = Vector3.Distance(bathroomDoorPivots[i].position, CameraMovement.cameraObject.transform.position);
                    if (dist2 < dist)
                    {
                        dist = dist2;
                        choice = i;
                    }
                }
                bathroom.position = bathroomDoorPivots[choice].position;
            }
            else
                if (GameObject.FindGameObjectWithTag("Bathroom") != null)
                    bathroom = GameObject.FindGameObjectWithTag("Bathroom").transform;
            counter = 0;
        }
    }
}
