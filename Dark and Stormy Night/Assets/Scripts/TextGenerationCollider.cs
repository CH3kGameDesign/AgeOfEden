using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGenerationCollider : MonoBehaviour {

    [Header("GameObjects")]
    public GameObject TextObject;

	// Use this for initialization
	void Start () {
        if (TextObject == null)
            TextObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && TextObject.GetComponent<TextGeneration>().played == false)
        {
            TextObject.GetComponent<TextGeneration>().StartCoroutine("TextGenerate", TextObject.GetComponent<TextGeneration>().textSpeed);
            TextObject.GetComponent<TextGeneration>().played = true;
        }
    }
}
