using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public GameObject pushee;

    public Vector3 pushForce;

    private Rigidbody m_rb;

    private void Start()
    {
        m_rb = pushee.GetComponent<Rigidbody>();
        m_rb.AddForce(pushForce, ForceMode.Impulse);
        //gameObject.SetActive(false);
        GetComponent<PushObject>().enabled = false;
    }
}