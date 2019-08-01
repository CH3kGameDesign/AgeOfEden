using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public GameObject pushee;

    public Vector3 pushForce;

	// Update is called once per frame
	private void Update ()
    {
        pushee.GetComponent<Rigidbody>().AddForce(pushForce, ForceMode.Impulse);
        gameObject.SetActive(false);
	}
}