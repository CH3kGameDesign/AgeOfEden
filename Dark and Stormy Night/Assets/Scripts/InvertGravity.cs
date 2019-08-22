using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : MonoBehaviour
{
    public Vector3 alteredGravity;
    private Vector3 normalGravity;

    public bool playOnAwake;

    public static bool invertedGravity = false;

    // Use this for initialization
    private void Start()
    {
        normalGravity = Physics.gravity;

        if (playOnAwake)
            Flip();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Flip();
    }

    /// <summary>
    /// Sets the current gravity to an inverted version
    /// </summary>
    private void Flip()
    {
        Physics.gravity = alteredGravity;
        invertedGravity = true;
        SmoothCameraMovement.gravDirection = 180;

        ///Debug.Log("Flip");
    }

    private void OnDisable()
    {
        Physics.gravity = normalGravity;
        invertedGravity = false;
        SmoothCameraMovement.gravDirection = 0;
    }
}