using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementVariables : MonoBehaviour
{
    /// <summary>
    /// Enables variables based on input
    /// </summary>
    /// <param name="pChoice">Controls what variables are enabled</param>
    public void ChangeVariables(int pChoice)
    {
        switch (pChoice)
        {
            case 0:
                CameraMovement.s_CanMove = true;
                break;
            case 1:
                Movement.s_bCanMove = true;
                break;
            case 2:
                Movement.s_bCanMove = true;
                CameraMovement.s_CanMove = true;
                break;
            default:
                break;
        }
    }
}