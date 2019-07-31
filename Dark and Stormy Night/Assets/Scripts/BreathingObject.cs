using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingObject : MonoBehaviour
{
    public float breathSpeed;
    public Vector3 breathMax;

    private bool inflate = true;
    
	// Update is called once per frame
	void Update ()
    {
        if (inflate == true)
            transform.localScale = Vector3.Lerp(transform.localScale, breathMax, breathSpeed * Time.deltaTime);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, breathSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localScale, breathMax) <= 0.01f)
            inflate = false;

        if (Vector3.Distance(transform.localScale, Vector3.one) <= 0.01f)
            inflate = true;
    }
}
