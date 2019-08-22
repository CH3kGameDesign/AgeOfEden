using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public GameObject soundFX;
    public float velocityMultiplier;
    private float velocity;
    
    private void LateUpdate()
    {
        velocity = GetComponent<Rigidbody>().velocity.magnitude;
        //Debug.Log(velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GameObject GO = Instantiate(soundFX, collision.contacts[0].point,
                soundFX.transform.rotation);
            GO.GetComponent<AudioSource>().volume = velocity * velocityMultiplier;
        }
    }
}