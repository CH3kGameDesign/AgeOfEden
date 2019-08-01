using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [Space(10)]
    public Vector3 offset;

    [Space (10)]

    public bool tilt = false;
    public float delay = 1;
    
	// Update is called once per frame
	private void Update ()
    {
        if (tilt)
        {
            transform.LookAt(Movement.m_goPlayerObject.transform.position + offset);
            if (InvertGravity.invertedGravity)
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        }
        else
        {
            Vector3 lookPos = transform.position - (Movement.m_goPlayerObject.transform.position + offset);
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * delay);
        }
	}
}