using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicChildren : MonoBehaviour
{
    public bool kinematic = true;
    public Transform parent;

	// Use this for initialization
	private void Start ()
    {
        Rigidbody[] rb = parent.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].isKinematic = kinematic;
        }
	}
}