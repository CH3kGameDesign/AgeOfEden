using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class FadeToScene : MonoBehaviour {

    public float tarFogStrength;
    public float fadeSpeed;

    public bool fadeTrue = false;

    public bool restartScene = true;
    public int tarSceneNumber = 0;

	private float startVignetteStrength = -1;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startVignetteStrength == -1) {
			PostProcessingProfile mainCamProfile = CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile;
			startVignetteStrength = mainCamProfile.vignette.settings.intensity;
		}
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

            PostProcessingProfile mainCamProfile = CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile;
            var vignette = mainCamProfile.vignette.settings;

            vignette.intensity = Mathf.Lerp(vignette.intensity, 0, fadeSpeed * Time.deltaTime);
            mainCamProfile.vignette.settings = vignette;
        }
        else
        {
			PostProcessingProfile mainCamProfile = CameraMovement.cameraObject.GetComponentInChildren<PostProcessingBehaviour>().profile;
			var vignette = mainCamProfile.vignette.settings;

			vignette.intensity = startVignetteStrength;
			mainCamProfile.vignette.settings = vignette;
            if (restartScene == true)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else
            {
                if (tarSceneNumber == -1)
                    Application.Quit();
                else
                    SceneManager.LoadScene(tarSceneNumber);
            }
        }
    }
}
