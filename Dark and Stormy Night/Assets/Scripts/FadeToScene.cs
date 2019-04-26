using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeToScene : MonoBehaviour {

    public float tarFogStrength;
    public float fadeSpeed;

    public bool fadeTrue;

	// Use this for initialization
	void Start () {
        fadeTrue = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeTrue == true)
            Fade();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            fadeTrue = true;
    }

    private void Fade ()
    {
        if (RenderSettings.fogDensity < tarFogStrength)
            RenderSettings.fogDensity += fadeSpeed * Time.deltaTime;
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
