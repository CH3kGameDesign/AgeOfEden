using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSoundFinish : MonoBehaviour
{
    public bool disable = false;
    public GameObject enableOnFinish;
    private bool hasPlayed = false;

    private AudioSource m_asLocalRef;

    private void Start()
    {
        hasPlayed = false;
        m_asLocalRef = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_asLocalRef.isPlaying)
        {
            hasPlayed = true;
        }
        else if (hasPlayed)
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