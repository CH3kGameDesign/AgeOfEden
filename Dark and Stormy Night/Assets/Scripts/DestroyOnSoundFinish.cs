using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSoundFinish : MonoBehaviour
{
    private bool hasPlayed = false;
    
	// Update is called once per frame
	private void Update ()
    {
        if (GetComponent<AudioSource>().isPlaying == true)
            hasPlayed = true;

        if (GetComponent<AudioSource>().isPlaying == false && hasPlayed == true)
            Destroy(this.gameObject);
	}
}
