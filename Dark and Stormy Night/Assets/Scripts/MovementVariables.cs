using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementVariables : MonoBehaviour {
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ChangeVariables(int choice)
    {
        if (choice == 0)
            CameraMovement.canMove = true;
        if (choice == 1)
            Movement.canMove = true;
        if (choice == 2)
        {
            Movement.canMove = true;
            CameraMovement.canMove = true;
        }
    }
}
