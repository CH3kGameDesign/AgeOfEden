using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftRotation : MonoBehaviour
{
    [Header ("Variables")]
    public float rotSpeed;
    public Vector3 rotMax;
    [HideInInspector]
    public Vector3 rotMin;
    public Transform Movee;

    public bool rotateBack = true;

    private int direction = 0;

	// Called once before the first frame
	private void Start()
    {
        if (!Movee)
            Movee = transform;
        rotMin = Movee.localEulerAngles;
	}
	
	// Called once per frame
	private void FixedUpdate()
    {
        if (direction == 0)
            Movee.localRotation = Quaternion.Lerp(
                Movee.localRotation, Quaternion.Euler(rotMax), rotSpeed * Time.deltaTime);
        else if (direction == 1)
            Movee.localRotation = Quaternion.Lerp(
                Movee.localRotation, Quaternion.Euler(rotMin), rotSpeed * Time.deltaTime);

        //Debug.Log(Time.fixedTime + " " + Mathf.Abs(
        //    Quaternion.Angle(Movee.rotation, Quaternion.Euler(rotMax))));

        if (Mathf.Abs(Quaternion.Angle(Movee.localRotation, Quaternion.Euler(rotMax))) <= 1)
        {
            if (rotateBack)
                direction = 1;
            else
                direction = 2;
        }

        if (Mathf.Abs(Quaternion.Angle(Movee.localRotation, Quaternion.Euler(rotMin))) <= 1)
            direction = 0;
    }
}