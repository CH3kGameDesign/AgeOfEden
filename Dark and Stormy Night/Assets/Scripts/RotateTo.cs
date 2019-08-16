using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : MonoBehaviour
{
    public float degreesPerSecond;

    public Transform Movee;
    public bool movePlayer;
    public bool moveCamera;

    public Vector3 eulerTarRotation;
    private Quaternion tarRot;

    public bool localRot;
    public bool relativeRot;

    public GameObject activateOnFinish;

    // Called once before the first frame
    private void Start()
    {
        if (!Movee && movePlayer)
            Movee = Movement.m_goPlayerObject.transform;

        if (!Movee && moveCamera)
        {
            Movee = CameraMovement.s_CameraObject.transform;
            CameraMovement.s_CameraObject.GetComponent
                <SmoothCameraMovement>().resetRotation();
        }

        tarRot = Quaternion.Euler(eulerTarRotation);

        if (!Movee)
            Movee = transform;

        if (relativeRot)
        {
            if (localRot)
                tarRot *= Movee.localRotation;
            else
                tarRot *= Movee.rotation;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        float speed = degreesPerSecond * Time.deltaTime;

        if (localRot)
        {
            if (Quaternion.Angle(Movee.localRotation, tarRot) < speed)
            {
                Movee.localRotation = tarRot;

                if (activateOnFinish)
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

                if (activateOnFinish)
                    activateOnFinish.SetActive(true);
            }
            else
            {
                Movee.rotation = Quaternion.RotateTowards(Movee.rotation, tarRot, speed);
            }
        }
    }
}