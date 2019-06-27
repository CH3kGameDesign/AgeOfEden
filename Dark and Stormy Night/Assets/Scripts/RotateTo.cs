using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : MonoBehaviour {

    public float degreesPerSecond;

    public Transform Movee;

    public Vector3 eulerTarRotation;
    private Quaternion tarRot;

    public bool localRot;
    public bool relativeRot;

    // Use this for initialization
    void Start()
    {
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
                Movee.localRotation = tarRot;
            else
            {
                Movee.localRotation = Quaternion.RotateTowards(Movee.localRotation, tarRot, speed);
            }
        }
        else
        {

            if (Quaternion.Angle(Movee.rotation, tarRot) < speed)
                Movee.rotation = tarRot;
            else
            {
                Movee.rotation = Quaternion.RotateTowards(Movee.rotation, tarRot, speed);
            }
        }

    }
}
