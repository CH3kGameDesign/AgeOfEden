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

    // Called once before the first frame
    private void Start()
    {
        if (changeOnStart)
            Switch();
    }
    
    /// <summary>
    /// Updates movement options based on variable states
    /// </summary>
    public void Switch ()
    {
        if (rotateOffset != 0)
            CameraMovement.cameraObject.GetComponent
                <SmoothCameraMovement>().rotateOffset = rotateOffset;
        if (choice != choices.camera)
            Movement.canMove = enable;
        if (choice != choices.movement)
            CameraMovement.canMove = enable;

        if (rigidBodyState == choices2.disable)
            Movement.m_goPlayerObject.GetComponent<Rigidbody>().isKinematic = true;
        if (rigidBodyState == choices2.enable)
            Movement.m_goPlayerObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}