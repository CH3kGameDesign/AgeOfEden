using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject portal;
    public GameObject room;

    public static bool inRoom = false;

    // Use this for initialization
    void Start()
    {
        inRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && inRoom == false)
        {
            portal.transform.position = Movement.player.transform.position + Movement.player.transform.forward;
            portal.transform.LookAt(Movement.player.transform);
            if (InvertGravity.invertedGravity == true)
                portal.transform.eulerAngles = new Vector3(0, portal.transform.eulerAngles.y, 180);
            room.transform.rotation = portal.transform.rotation;
            portal.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && inRoom == false)
        {
            portal.transform.position = room.transform.position;
        }
        portal.transform.localScale = Vector3.Lerp(portal.transform.localScale, Vector3.one, Time.deltaTime * 2);
    }
}