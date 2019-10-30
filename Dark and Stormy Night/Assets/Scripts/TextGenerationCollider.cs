using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGenerationCollider : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject TextObject;

    // Fix this
    private TextGeneration tempRef;

	// Called once before the first frame
	private void Start()
    {
        if (!TextObject)
            TextObject = transform.parent.gameObject;

        tempRef = TextObject.GetComponent<TextGeneration>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !tempRef.m_bPlayed)
        {
            tempRef.PresentTextCoroutine();
        }
    }
}