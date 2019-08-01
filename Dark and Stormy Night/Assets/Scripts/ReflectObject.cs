using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectObject : MonoBehaviour
{
    public Transform reflectableObject;
    public bool reflectPlayer = false;
    public Transform reflectPoint;
    public Vector3 reflectAmount;
    public Transform reflectingThing;

	// Called once before the first frame
	private void Start ()
    {
        if (!reflectPoint)
            reflectPoint = transform;
        if (reflectPlayer)
            reflectableObject = Movement.m_goPlayerObject.GetComponent<Movement>().m_aModelAnimator.transform;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        Vector3 DistanceBetween = reflectPoint.position - reflectableObject.position;

        Vector3 TarRelPos = new Vector3(
            DistanceBetween.x * reflectAmount.x,
            DistanceBetween.y * reflectAmount.y,
            DistanceBetween.z * reflectAmount.z);

        reflectingThing.position = reflectPoint.position + TarRelPos;
	}
}