using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour {

    public Transform target;
    public Vector3 direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (direction.x != 0)
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        if (direction.y != 0)
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        if (direction.z != 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
    }
}
