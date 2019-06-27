using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {

    public float metresPerSecond;

    public Transform Movee;

    public Vector3 tarPos;

    public bool localPos;
    public bool relativePos;

	// Use this for initialization
	void Start () {
        if (Movee == null)
            Movee = this.transform;
        if (relativePos == true)
        {
            if (localPos == true)
                tarPos += Movee.localPosition;
            else
                tarPos += Movee.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float speed = metresPerSecond * Time.deltaTime;

        if (localPos == true)
        {
            if (Vector3.Distance(Movee.position, Movee.parent.position + tarPos) < speed)
                Movee.localPosition = tarPos;
            else
            {
                Vector3 direction = tarPos - Movee.localPosition;
                direction = Vector3.ClampMagnitude(direction, 1);

                Movee.localPosition += direction * speed;
            }
        }
        else
        {
            
            if (Vector3.Distance(Movee.position, tarPos) < speed)
                Movee.position = tarPos;
            else
            {
                Vector3 direction = tarPos - Movee.position;
                direction *= 1000;
                direction = Vector3.ClampMagnitude(direction, 1);

                Movee.position += direction * speed;
            }
        }

	}
}
