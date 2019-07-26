using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : MonoBehaviour {

    public float degreesPerSecond;

    public Transform Movee;
    public bool movePlayer;
    public bool moveCamera;

    public Vector3 eulerTarRotation;
    private Quaternion tarRot;

    public bool localRot;
    public bool relativeRot;

    public GameObject activateOnFinish;

    // Use this for initialization
    void Start()
    {
        if (Movee == null && movePlayer == true)
            Movee = Movement.player.transform;
        if (Movee == null && moveCamera == true)
        {
            Movee = CameraMovement.cameraObject.transform;
            CameraMovement.cameraObject.GetComponent<SmoothCameraMovement>().resetRotation();
        }

        tarRot = Quaternion.Euler(eulerTarRotation);
        if (Movee == null)
            Movee = this.transform;
        if (relativeRot == true)
        {
            if (localRot == true)
                tarRot *= Movee.localRotation;
            else
                tarRot *= Movee.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speed = degreesPerSecond * Time.deltaTime;

        if (localRot == true)
        {
            if (Quaternion.Angle(Movee.localRotation, tarRot) < speed)
            {
                Movee.localRotation = tarRot;
                if (activateOnFinish != null)
                    activateOnFinish.SetActive(true);
            }
            else
            {
                Movee.localRotation = Quaternion.RotateTowards(Movee.localRotation, tarRot, speed);
            }
        }
        else
        {

            if (Quaternion.Angle(Movee.rotation, tarRot) < speed)
            {
                Movee.rotation = tarRot;
                if (activateOnFinish != null)
                    activateOnFinish.SetActive(true);
            }
            else
            {
                Movee.rotation = Quaternion.RotateTowards(Movee.rotation, tarRot, speed);
            }
        }

    }
}
