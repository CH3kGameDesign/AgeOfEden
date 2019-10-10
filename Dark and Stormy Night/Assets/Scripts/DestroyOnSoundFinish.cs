using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSoundFinish : MonoBehaviour
{
    public bool disable = false;
    public GameObject enableOnFinish;
    private bool hasPlayed = false;

    private void Start()
    {
        hasPlayed = false;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (GetComponent<AudioSource>().isPlaying)
            hasPlayed = true;

        if (!GetComponent<AudioSource>().isPlaying && hasPlayed)
        {
            if (enableOnFinish != null)
                enableOnFinish.SetActive(true);
            if (!disable)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
	}
}