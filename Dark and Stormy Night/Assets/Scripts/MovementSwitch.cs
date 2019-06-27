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

    // Use this for initialization
    void Start()
    {
        if (changeOnStart)
        {
            Switch();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Switch()
    {
        if (choice != choices.camera)
            Movement.canMove = enable;
        if (choice != choices.movement)
            CameraMovement.canMove = enable;

        if (rigidBodyState == choices2.disable)
            Movement.player.GetComponent<Rigidbody>().isKinematic = true;
        if (rigidBodyState == choices2.enable)
            Movement.player.GetComponent<Rigidbody>().isKinematic = false;
    }
}

