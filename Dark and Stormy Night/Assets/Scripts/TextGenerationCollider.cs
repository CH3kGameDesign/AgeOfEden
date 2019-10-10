using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGenerationCollider : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject TextObject;

	// Called once before the first frame
	private void Start()
    {
        if (!TextObject)
            TextObject = transform.parent.gameObject;
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !TextObject.GetComponent<TextGeneration>().played)
        {
            TextObject.GetComponent<TextGeneration>().StartCoroutine(
                "TextGenerate", TextObject.GetComponent<TextGeneration>().textSpeed);

            TextObject.GetComponent<TextGeneration>().played = true;
        }
    }
}
