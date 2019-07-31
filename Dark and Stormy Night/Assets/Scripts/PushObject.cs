using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour {

    public GameObject pushee;

    public Vector3 pushForce;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        pushee.GetComponent<Rigidbody>().AddForce(pushForce, ForceMode.Impulse);
        this.gameObject.SetActive(false);
	}
}
