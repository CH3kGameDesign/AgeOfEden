using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    public List<Transform> bathroomDoorPivots = new List<Transform>();

    public Transform bathroom;

    public enum choices { bathroom, houseroom}
    [Space(20)]
    public choices Room;

    private int counter;

    private Transform m_tCameraTransform;

    private void Start()
    {
        m_tCameraTransform = CameraMovement.s_CameraObject.transform;
    }

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
                        m_tCameraTransform.position);

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
                // This is really bad for performance - Fix
                if (Room == choices.bathroom)
                {
                    // Attempts to assign the bathroom transform to the bathroom object's
                    try { bathroom = GameObject.FindGameObjectWithTag("Bathroom").transform; }
                    catch { bathroom = null; }
                }
                if (Room == choices.houseroom)
                {
                    // Attempts to assign the bathroom transform to the bathroom object's
                    try { bathroom = GameObject.FindGameObjectWithTag("Houseroom").transform; }
                    catch { bathroom = null; }
                }

                //if (GameObject.FindGameObjectWithTag("Bathroom"))
                //    bathroom = GameObject.FindGameObjectWithTag("Bathroom").transform;
            }
            counter = 0;
        }
    }
}