using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlammer : MonoBehaviour {

    public Vector3 tarEuler;
    private Quaternion tarRot;
    public Vector3 releaseForce;
    public float closeSpeed;

    private int counter;
    private bool move;
    private bool closed;

	// Use this for initialization
	void Start () {
        tarRot = Quaternion.Euler(tarEuler);
	}
	
	// Update is called once per frame
	void Update () {
        counter++;
        if (counter > 5)
        {
            float dist2 = Vector3.Distance(transform.position, CameraMovement.cameraObject.transform.position);
            if (dist2 > 4)
            {
                //transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
                if (Mathf.Abs(Quaternion.Angle(tarRot, transform.localRotation)) > 0.5f)
                    move = true;
                closed = true;
            }
            else
                if (closed == true)
            {
                closed = false;
                //transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                transform.GetChild(0).GetComponent<Rigidbody>().AddForce(releaseForce, ForceMode.Impulse);
            }
            counter = 0;
        }
        if (move)
        {
            transform.GetChild(0).GetComponent<Rigidbody>().AddForce(tarEuler, ForceMode.Impulse);
            /*
            float tarMove = Quaternion.Angle(transform.localRotation, tarRot);
            if (Mathf.Abs(tarMove) < closeSpeed)
            {
                transform.localRotation = tarRot;
                move = false;
            }
            else
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, tarRot, closeSpeed);
                */
        }
    }
}
