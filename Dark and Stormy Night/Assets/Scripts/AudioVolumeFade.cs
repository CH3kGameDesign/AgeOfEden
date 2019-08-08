using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeFade : MonoBehaviour {

    public AudioSource tarSource;
    public float tarVolume;
    public float seconds;
    public bool play;

	// Use this for initialization
	void Start () {
        if (tarSource == null)
            tarSource = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (play)
        {
            Invoke("Disable", seconds * 5);
            tarSource.volume = Mathf.Lerp(tarSource.volume, tarVolume, Time.deltaTime / seconds);
        }
	}

    void Disable()
    {
        play = false;
    }
}
