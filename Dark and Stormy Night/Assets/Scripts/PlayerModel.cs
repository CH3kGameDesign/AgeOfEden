using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [HideInInspector]
    public static GameObject player;

    [Header("GameObjects")]
    public GameObject cameraHolder;

    [Header("Variables")]
    public float rotSpeed;

    private float desiredYRotation;

    // Use this for initialization
    private void Start ()
    {
        player = gameObject;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        //desiredYRotation = Mathf.Lerp(desiredYRotation,
        //    cameraHolder.transform.rotation.eulerAngles.y, rotSpeed);

        if (Movement.canMove)
        {
            transform.rotation = Quaternion.Euler(0, cameraHolder.transform.rotation.eulerAngles.y,
                cameraHolder.transform.rotation.eulerAngles.z);
        }
	}
}