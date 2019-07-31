using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : MonoBehaviour
{
    public bool m_bPlayOnAwake;

    public static bool m_bInvertedGravity = false;

    public Vector3 m_v3AlteredGravity;
    private Vector3 m_v3NormalGravity;

    // Called once before the first frame
    private void Start ()
    {
        m_v3NormalGravity = Physics.gravity;

        if (m_bPlayOnAwake == true)
            Flip();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Flip();
        }
    }

    private void Flip ()
    {
        Physics.gravity = m_v3AlteredGravity;
        m_bInvertedGravity = true;
        SmoothCameraMovement.gravDirection = 180;

        //Debug.Log("Flip");
    }

    private void OnDisable ()
    {
        Physics.gravity = m_v3NormalGravity;
        m_bInvertedGravity = false;
        SmoothCameraMovement.gravDirection = 0;
    }
}
