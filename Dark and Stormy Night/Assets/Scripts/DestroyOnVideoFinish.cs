using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DestroyOnVideoFinish : MonoBehaviour
{
    public bool disable = false;
    public List<GameObject> enableOnFinish = new List<GameObject>();
    private bool hasPlayed = false;

    private VideoPlayer m_vpLocalRef;

    private void Start()
    {
        hasPlayed = false;
        m_vpLocalRef = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_vpLocalRef.isPlaying)
        {
            hasPlayed = true;
        }
        else if (hasPlayed)
        {
            if (enableOnFinish.Count > 0)
            {
                for (int i = 0; i < enableOnFinish.Count; i++)
                {
                    enableOnFinish[i].SetActive(true);
                }
            }
                
            if (!disable)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
	}
}