using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [HideInInspector]
    public static GameObject player;
    public static Vector3 faceDirection;

    [Header("GameObjects")]
    public GameObject cameraHolder;

    [Header("Variables")]
    public float rotSpeed;

    private float desiredYRotation;

    // Use this for initialization
    private void Start()
    {
        faceDirection = Vector3.zero;
        player = gameObject;
	}
	
	// Update is called once per frame
	private void Update()
    {
        //desiredYRotation = Mathf.Lerp(desiredYRotation,
        //    cameraHolder.transform.rotation.eulerAngles.y, rotSpeed);

        if (Movement.canMove)
        {
            if (faceDirection == Vector3.zero)
                transform.rotation = Quaternion.Euler(0, cameraHolder.transform.rotation.eulerAngles.y,
                    cameraHolder.transform.rotation.eulerAngles.z);
            else
                transform.LookAt(new Vector3(transform.position.x + faceDirection.x, transform.position.y, transform.position.z + faceDirection.z));
        }
	}
}