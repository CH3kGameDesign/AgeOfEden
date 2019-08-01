﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementVariables : MonoBehaviour
{
    /// <summary>
    /// Enables variables based on input
    /// </summary>
    /// <param name="pChoice">Controls what variables are enabled</param>
    public void ChangeVariables (int pChoice)
    {
        switch (pChoice)
        {
            case 0:
                CameraMovement.canMove = true;
                break;
            case 1:
                Movement.canMove = true;
                break;
            case 2:
                Movement.canMove = true;
                CameraMovement.canMove = true;
                break;
            default:
                break;
        }
    }
}
