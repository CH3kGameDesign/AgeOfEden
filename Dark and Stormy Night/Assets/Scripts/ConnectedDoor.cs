using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedDoor : MonoBehaviour
{
    public GameObject OtherDoor;

    private Rigidbody m_rbRigidbody;

    private Rigidbody m_rbOtherDoor;
    private Transform m_tOtherDoor;

    private Transform m_tPlayerTransform;

    private void Start()
    {
        m_rbRigidbody = GetComponent<Rigidbody>();
        m_tPlayerTransform = Movement.s_goPlayerObject.transform;

        m_rbOtherDoor = OtherDoor.GetComponent<Rigidbody>();
        m_tOtherDoor = OtherDoor.transform;
    }

    // Update is called once per frame
    private void Update()
    {
		if (Vector3.Distance(m_tPlayerTransform.position, transform.position)
            < Vector3.Distance(m_tPlayerTransform.position, m_tOtherDoor.position))
            m_rbOtherDoor.velocity = m_rbRigidbody.velocity;
	}
}