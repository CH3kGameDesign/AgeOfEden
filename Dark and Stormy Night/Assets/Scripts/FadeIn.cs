using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    [Header("Variables")]
    public float fadeSpeed;
    public float fadeOutTimer;

	// Use this for initialization
	void Start () {
        if (GetComponent<FadeOut>() != null)
            GetComponent<FadeOut>().enabled = false;
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().color = Color.clear;
        if (GetComponent<RawImage>() != null)
            GetComponent<RawImage>().color = Color.clear;
        if (fadeOutTimer != 0)
            Invoke("FadeOutActivate", fadeOutTimer);
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, fadeSpeed * Time.deltaTime);
        if (GetComponent<RawImage>() != null)
            GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, Color.white, fadeSpeed * Time.deltaTime);
		if (GetComponent<MeshRenderer>() != null)
			GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, Color.white, fadeSpeed * Time.deltaTime);
		
    }

    void FadeOutActivate ()
    {
        if (GetComponent<FadeOut>() != null)
            GetComponent<FadeOut>().enabled = true;
        GetComponent<FadeIn>().enabled = false;
    }
}
