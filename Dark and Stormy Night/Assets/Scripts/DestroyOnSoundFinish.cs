using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSoundFinish : MonoBehaviour {

    private bool hasPlayed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<AudioSource>().isPlaying == true)
            hasPlayed = true;
        if (GetComponent<AudioSource>().isPlaying == false && hasPlayed == true)
            Destroy(this.gameObject);
	}
}
