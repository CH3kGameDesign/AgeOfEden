using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverEndingCorridor : MonoBehaviour
{
    private int corridorSegmentCount = 100;
    private bool goBackwards = true;
    public GameObject corridorSegment;
    public GameObject corridorSegmentLight;

	// Called once before the first frame
	private void Start ()
    {
        if (!corridorSegment)
            corridorSegment = transform.GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < 99; i++)
            Instantiate(corridorSegment, transform.position
                + new Vector3(-i * 3, 0, 0), transform.rotation, transform.GetChild(0));
	}
	
    private void OnCollisionStay (Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            float verSpeed = collision.collider.GetComponent<Movement>().desiredVelocity.x;

            if (verSpeed > 0)
                transform.GetChild(0).transform.position += new Vector3(
                    verSpeed*Time.deltaTime, 0, 0);

            if (verSpeed < 0 && goBackwards)
            {
                transform.GetChild(0).transform.position -= new Vector3(10000, 0, 0);
                goBackwards = false;
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.transform.parent.parent == transform.GetChild(0))
        {
            other.transform.parent.position = transform.position + new Vector3(
                -corridorSegmentCount * 3, 0, 0);

            corridorSegmentCount++;
        }
    }
}
