﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject portal;
    public GameObject room;

    public static bool inRoom = false;

    // Use this for initialization
    private void Start()
    {
        inRoom = false;
    }

    // Update is called once per frame
    private void Update()
    {
		if ((Input.GetKeyDown(KeyCode.Mouse2) || Input.GetKeyDown(KeyCode.Alpha9))
            && !inRoom)
        {
            portal.transform.position = Movement.s_goPlayerObject.transform.position
                + Movement.s_goPlayerObject.transform.forward;
            portal.transform.LookAt(Movement.s_goPlayerObject.transform);

            if (InvertGravity.invertedGravity)
                portal.transform.eulerAngles = new Vector3(0, portal.transform.eulerAngles.y, 180);

            room.transform.rotation = portal.transform.rotation;
            portal.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

		if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Alpha0))
            && !inRoom)
        {
            portal.transform.position = room.transform.position;
        }

        portal.transform.localScale = Vector3.Lerp(portal.transform.localScale, Vector3.one,
            Time.deltaTime * 2);
    }
}