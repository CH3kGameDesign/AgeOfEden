using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class CycleThroughSounds : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();
    public bool shuffle;
    private int lastPlayed = -1;

    private AudioSource AS;

	// Use this for initialization
	void Start () {
        AS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (AS.isPlaying == false)
        {
            int play = 0;
            if (shuffle)
                play = Random.Range(0, clips.Count);
            else
                play = lastPlayed + 1;
            if (play >= clips.Count)
                play = 0;
            AS.clip = clips[play];
            AS.Play();
            lastPlayed = play;
        }

	}
}
