using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftRotation : MonoBehaviour {

    [Header ("Variables")]
    public float rotSpeed;
    public Vector3 rotMax;
    [HideInInspector]
    public Vector3 rotMin;

    public bool rotateBack = true;

    private int direction = 0;

	// Use this for initialization
	void Start () {
        rotMin = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (direction == 0)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotMax), rotSpeed * Time.deltaTime);
        else if (direction == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotMin), rotSpeed * Time.deltaTime);


        if (Mathf.Abs(Quaternion.Angle(transform.rotation, Quaternion.Euler(rotMax))) <= 1)
        {
            if (rotateBack == true)
                direction = 1;
            else
                direction = 2;
        }
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, Quaternion.Euler(rotMin))) <= 1)
            direction = 0;
    }
}
