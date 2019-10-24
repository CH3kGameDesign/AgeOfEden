using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class FadeToScene : MonoBehaviour
{
    public float tarFogStrength;
    public float fadeSpeed;

    public bool fadeTrue = false;

    public bool restartScene = false;
    public int tarSceneNumber = 1;

	private float startVignetteStrength = -1;

    private PostProcessingBehaviour m_ppbPostProcessing;

    // Update is called once per frame
    private void Update()
    {
        m_ppbPostProcessing = CameraMovement.s_CameraObject.
            GetComponentInChildren<PostProcessingBehaviour>();

        if (startVignetteStrength == -1)
        {
			PostProcessingProfile mainCamProfile = m_ppbPostProcessing.profile;
			startVignetteStrength = mainCamProfile.vignette.settings.intensity;
		}

        if (fadeTrue)
            Fade();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            fadeTrue = true;
    }

    /// <summary>
    /// Initiates a fade
    /// </summary>
    private void Fade()
    {
        if (RenderSettings.fogDensity < tarFogStrength)
        {
            RenderSettings.fogDensity += fadeSpeed * Time.deltaTime;

            PostProcessingProfile mainCamProfile = m_ppbPostProcessing.profile;

            var vignette = mainCamProfile.vignette.settings;

            vignette.intensity = Mathf.Lerp(vignette.intensity, 0, fadeSpeed * Time.deltaTime);
            mainCamProfile.vignette.settings = vignette;
        }
        else
        {
			PostProcessingProfile mainCamProfile = m_ppbPostProcessing.profile;

            var vignette = mainCamProfile.vignette.settings;

			vignette.intensity = startVignetteStrength;
			mainCamProfile.vignette.settings = vignette;

            if (restartScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
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