using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSwitch : MonoBehaviour
{
    public enum choices { movement, camera, both }
    public enum choices2 { disable, enable, unChanged }

    public choices choice;
    public choices2 rigidBodyState;

    public bool enable;
    public bool changeOnStart;

    public float rotateOffset;
    public Vector2 sittingMaxRotation;

    // Called once before the first frame
    private void Start()
    {
        if (changeOnStart)
            Switch();
    }
    
    /// <summary>
    /// Updates movement options based on variable states
    /// </summary>
    public void Switch()
    {
        if (rotateOffset != 0)
            CameraMovement.s_CameraObject.GetComponent
                <SmoothCameraMovement>().m_fRotateOffset = rotateOffset;

        if (sittingMaxRotation != Vector2.zero)
            SmoothCameraMovement.s_v2SittingMaxRotation = sittingMaxRotation;

        if (choice != choices.camera)
            Movement.canMove = enable;

        if (choice != choices.movement)
        {
            CameraMovement.s_CanMove = enable;
            //CameraMovement.s_CameraObject.GetComponent<SmoothCameraMovement>()
            //    .resetRotation();
        }

        if (rigidBodyState == choices2.disable)
            Movement.m_goPlayerObject.GetComponent<Rigidbody>().isKinematic = true;

        if (rigidBodyState == choices2.enable)
            Movement.m_goPlayerObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}