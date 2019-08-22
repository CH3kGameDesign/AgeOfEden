using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject portal;
    public GameObject room;

    public static bool inRoom = false;
    
	// Update is called once per frame
	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1) && !inRoom)
        {
            portal.transform.position = Movement.m_goPlayerObject.transform.position
                + Movement.m_goPlayerObject.transform.forward;

            portal.transform.LookAt(Movement.m_goPlayerObject.transform);

            if (InvertGravity.invertedGravity)
                portal.transform.eulerAngles = new Vector3(0, portal.transform.eulerAngles.y, 180);

            room.transform.rotation = portal.transform.rotation;
        }
	}
}