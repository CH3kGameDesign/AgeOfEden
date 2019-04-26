using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : MonoBehaviour {

    public Vector3 alteredGravity;
    private Vector3 normalGravity;
	// Use this for initialization
	void Start () {
        normalGravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.gravity = alteredGravity;
            
            Debug.Log("Flip");
        }
    }

    private void OnDisable()
    {
        Physics.gravity = normalGravity;
    }
}
