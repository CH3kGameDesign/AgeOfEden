using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class FadeToScene : MonoBehaviour {

    public float tarFogStrength;
    public float fadeSpeed;

    public bool fadeTrue = false;

	// Use this for initialization
	void Start () {
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
        {
            RenderSettings.fogDensity += fadeSpeed * Time.deltaTime;
            /*
            VignetteModel tarVig = CameraMovement.cameraObject.GetComponent<PostProcessingBehaviour>().profile.vignette;
            tarVig.settings.intensity  = Mathf.Lerp()
            */
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
