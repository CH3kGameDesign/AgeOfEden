using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedDoor : MonoBehaviour
{
    public GameObject OtherDoor;
    
	// Update is called once per frame
	private void Update()
    {
		if (Vector3.Distance(Movement.s_goPlayerObject.transform.position, transform.position)
            < Vector3.Distance(
                Movement.s_goPlayerObject.transform.position, OtherDoor.transform.position))
            OtherDoor.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
	}
}