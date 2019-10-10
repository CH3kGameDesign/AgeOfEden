using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public bool global = true;

    public Vector3 rotationLock;

    private Vector3 startRotation;
	// Use this for initialization
	private void Start ()
    {
        if (global)
            startRotation = transform.eulerAngles;
        else
            startRotation = transform.localEulerAngles;

        Debug.Log(startRotation);
    }
	
	// Update is called once per frame
	private void Update ()
    {
		if (global)
        {
            if (rotationLock.x != 0)
                transform.eulerAngles = new Vector3(
                    startRotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            if (rotationLock.y != 0)
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x, startRotation.y, transform.eulerAngles.z);
            if (rotationLock.z != 0)
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x, transform.eulerAngles.y, startRotation.z);
        }
        else
        {
            if (rotationLock.x != 0)
                transform.localEulerAngles = new Vector3(
                    startRotation.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            if (rotationLock.y != 0)
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x, startRotation.y, transform.localEulerAngles.z);
            if (rotationLock.z != 0)
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x, transform.localEulerAngles.y, startRotation.z);
        }
	}
}