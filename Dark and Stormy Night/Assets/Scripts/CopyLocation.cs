﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour
{
    public Transform target;
    public Transform movee;
    public bool copyPlayer = false;
    public bool copyCamera = false;
    public Vector3 direction;
    public bool copyRotation = false;

    private void Start()
    {
        if (movee == null)
            movee = this.transform;
        if (copyPlayer)
            target = Movement.m_goPlayerObject.transform;
        if (copyCamera)
            target = CameraMovement.s_CameraObject.transform;
    }


    // Update is called once per frame
    private void LateUpdate ()
    {
        if (direction.x != 0)
            movee.position = new Vector3(
                target.position.x, transform.position.y, transform.position.z);

        if (direction.y != 0)
            movee.position = new Vector3(
                transform.position.x, target.position.y, transform.position.z);

        if (direction.z != 0)
            movee.position = new Vector3(
                transform.position.x, transform.position.y, target.position.z);
                
        if (copyRotation == true)
            movee.rotation = target.rotation;
    }
}