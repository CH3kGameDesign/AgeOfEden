using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTowards : MonoBehaviour {

    public bool pullPlayer;
    public bool pullToPlayer;

    public Transform movee;
    public Transform moveTo;

    public float tarForceMagnitude;
    public float lerpSpeed;

    private float currentForceMagnitude = 0;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        if (pullPlayer && movee == null)
            movee = Movement.s_goPlayerObject.transform;
        if (pullToPlayer && moveTo == null)
            moveTo = Movement.s_goPlayerObject.transform;
        if (movee == null)
            movee = transform;
        if (moveTo == null)
            moveTo = transform;

        if (movee == moveTo)
            GetComponent<PullTowards>().enabled = false;
        rb = movee.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        currentForceMagnitude = Mathf.Lerp(currentForceMagnitude, tarForceMagnitude, Time.deltaTime * lerpSpeed);
        Vector3 dir = moveTo.position - movee.position;
        dir *= 1000;
        dir = Vector3.ClampMagnitude(dir, 1);
        rb.AddForce(dir * currentForceMagnitude);
	}
}
