using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinonousRotation : MonoBehaviour
{
    public Vector3 rotationDirection;

	private void Start ()
    {
        if (rotationDirection == Vector3.zero)
            rotationDirection = new Vector3(Random.Range(0, 360),
                Random.Range(0, 360), Random.Range(0, 360));
	}
	
	// Update is called once per frame
	private void Update ()
    {
        transform.localEulerAngles += rotationDirection * Time.deltaTime;
	}
}
