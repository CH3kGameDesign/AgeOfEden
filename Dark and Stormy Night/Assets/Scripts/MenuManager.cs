using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject portal;
    public GameObject room;

    public static bool inRoom = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1) && inRoom == false)
        {
            portal.transform.position = Movement.player.transform.position + Movement.player.transform.forward;
            portal.transform.LookAt(Movement.player.transform);
            if (InvertGravity.invertedGravity == true)
                portal.transform.eulerAngles = new Vector3(0, portal.transform.eulerAngles.y, 180);
            room.transform.rotation = portal.transform.rotation;
        }
	}
}
