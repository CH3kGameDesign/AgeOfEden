using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverEndingCorridor : MonoBehaviour
{
    private int corridorSegmentCount = 100;
    private bool goBackwards = true;
    public GameObject corridorSegment;
    public GameObject corridorSegmentLight;

    private Transform m_tChildTrans;

	// Called once before the first frame
	private void Start()
    {
        if (!corridorSegment)
            corridorSegment = transform.GetChild(0).GetChild(0).gameObject;

        // If problem, add '.transform' to the end of this
        m_tChildTrans = transform.GetChild(0);
        
        for (int i = 0; i < 99; i++)
            Instantiate(corridorSegment, transform.position
                + new Vector3(-i * 3, 0, 0), transform.rotation, m_tChildTrans);
	}
	
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            float verSpeed = collision.collider.GetComponent<Movement>().m_v2InputVec2.x;

            if (verSpeed > 0)
                m_tChildTrans.position += new Vector3(
                    verSpeed*Time.deltaTime, 0, 0);

            if (verSpeed < 0 && goBackwards)
            {
                m_tChildTrans.position -= new Vector3(10000, 0, 0);
                goBackwards = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent == m_tChildTrans)
        {
            other.transform.parent.position = transform.position + new Vector3(
                -corridorSegmentCount * 3, 0, 0);

            corridorSegmentCount++;
        }
    }
}