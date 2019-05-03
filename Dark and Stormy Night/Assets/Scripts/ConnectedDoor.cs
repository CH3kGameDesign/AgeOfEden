using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedDoor : MonoBehaviour {
    
    public GameObject OtherDoor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(Movement.player.transform.position, transform.position) < Vector3.Distance(Movement.player.transform.position, OtherDoor.transform.position))
        {
            OtherDoor.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        }
            
	}
}
