using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTunnel : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;

    public Transform Pendulum;

    public float gravA;
    public float gravB;

    private float gravCurrent;

    private float pointDistance;

    public static bool inGravTunnel = false;

	// Use this for initialization
	void Start () { 
        pointDistance = Vector3.Distance(pointA.position, pointB.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            /*
            if (Vector3.Distance(Movement.player.transform.position, pointA.position) < Vector3.Distance(Movement.player.transform.position, pointB.position))
            {
                SmoothCameraMovement.gravSnap(gravB);
            }
            else
                SmoothCameraMovement.gravSnap(gravB);
                */
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            inGravTunnel = true;
            Movement.player.GetComponent<Rigidbody>().useGravity = false;

            float distanceToA = Vector3.Distance(Movement.player.transform.position, pointA.position);
            float distanceToB = Vector3.Distance(Movement.player.transform.position, pointB.position);

            distanceToA /= pointDistance;
            distanceToB /= pointDistance;

            gravCurrent = ((gravA * distanceToB) + (gravB * distanceToA));
            Pendulum.localEulerAngles = new Vector3(0, 0, gravCurrent);

            Vector3 gravDirection = Pendulum.GetChild(0).position - Pendulum.position;

            Movement.player.GetComponent<Rigidbody>().AddForce(gravDirection * 9.2f, ForceMode.Acceleration);

            SmoothCameraMovement.originalRotation = Quaternion.Lerp(SmoothCameraMovement.originalRotation, Pendulum.localRotation, Time.deltaTime * 2);
            //SmoothCameraMovement.gravSnap(360 - gravCurrent);
            //SmoothCameraMovement.gravDirection = 360 - gravCurrent;
            //SmoothCameraMovement.gravDirection = Pendulum.localEulerAngles.z;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inGravTunnel = false;
            Movement.player.GetComponent<Rigidbody>().useGravity = true;
            //SmoothCameraMovement.gravSnap(0);
            
            if (Vector3.Distance(Movement.player.transform.position, pointA.position) < Vector3.Distance(Movement.player.transform.position, pointB.position))
            {
                SmoothCameraMovement.gravDirection = gravA;
            }
            else
                SmoothCameraMovement.gravDirection = gravB;
                

        }
    }
}
