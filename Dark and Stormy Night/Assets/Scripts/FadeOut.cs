using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOut : MonoBehaviour {

    public bool fade = false;

    public float fadeSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fade == true) {
			if (GetComponent<TextMeshPro> () != null) {
				GetComponent<TextMeshPro> ().color = Color.Lerp (GetComponent<TextMeshPro> ().color, Color.clear, fadeSpeed * Time.deltaTime);
            

				if (GetComponent<TextMeshPro> ().color.a == 0) {
					Destroy (this.gameObject);
				}
			} else {
				if (GetComponent<MeshRenderer> () != null) {
					GetComponent<MeshRenderer> ().material.color = Color.Lerp (GetComponent<MeshRenderer> ().material.color, Color.clear, fadeSpeed * Time.deltaTime);


					if (GetComponent<MeshRenderer> ().material.color.a == 0) {
						Destroy (this.gameObject);
					}
				}
			}
		}
    }
}
