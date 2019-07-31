using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public Vector3 m_v3PushForce;
    
    public GameObject m_goPushee;

	// Update is called once per frame
	void Update ()
    {
        m_goPushee.GetComponent<Rigidbody>().AddForce(m_v3PushForce, ForceMode.Impulse);
        gameObject.SetActive(false);
	}
}
