using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

    [HideInInspector]
    public static GameObject player;

    [Header("GameObjects")]
    public GameObject cameraHolder;

    [Header("Variables")]
    public float rotSpeed;

    private float desiredYRotation;

    // Use this for initialization
    void Start () {
        player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //desiredYRotation = Mathf.Lerp(desiredYRotation, cameraHolder.transform.rotation.eulerAngles.y, rotSpeed);
        if (Movement.canMove == true)
            transform.rotation = Quaternion.Euler(0, cameraHolder.transform.rotation.eulerAngles.y, 0);
	}
}
