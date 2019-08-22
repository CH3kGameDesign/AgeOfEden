﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    public List<Transform> bathroomDoorPivots = new List<Transform>();

    private Transform bathroom;

    private int counter;
	
	// Update is called once per frame
	private void Update()
    {
        counter++;
        if (counter > 5)
        {
            if (bathroom != null)
            {
                int choice = 0;
                float dist = 100;
                for (int i = 0; i < bathroomDoorPivots.Count; i++)
                {
                    float dist2 = Vector3.Distance(bathroomDoorPivots[i].position,
                        CameraMovement.s_CameraObject.transform.position);
                    if (dist2 < dist)
                    {
                        dist = dist2;
                        choice = i;
                    }
                }
                bathroom.position = bathroomDoorPivots[choice].position;
            }
            else
            {
                // Attempts to assign the bathroom transform to the bathroom object's
                try { bathroom = GameObject.FindGameObjectWithTag("Bathroom").transform; }
                catch { bathroom = null; }

                //if (GameObject.FindGameObjectWithTag("Bathroom"))
                //    bathroom = GameObject.FindGameObjectWithTag("Bathroom").transform;
            }
            counter = 0;
        }
    }
}