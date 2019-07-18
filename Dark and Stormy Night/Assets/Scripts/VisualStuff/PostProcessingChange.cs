using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingChange : MonoBehaviour {

    public PostProcessingProfile tarProfile;
    public PostProcessingBehaviour tarCamera;

    public float lerpSpeed;

	// Use this for initialization
	void Start () {
        if (tarCamera == null)
        {
            if (Camera.main != null)
                tarCamera = Camera.main.GetComponent<PostProcessingBehaviour>();    
        }
	}

    private void Awake()
    {
        //tarCamera.profile = tarProfile;
        Invoke("Disable", 5);
    }

    // Update is called once per frame
    void Update () {
        if (tarCamera == null)
        {
            if (Camera.main != null)
                tarCamera = Camera.main.GetComponent<PostProcessingBehaviour>();
        }
        else
        Change();
	}

    void Change ()
    {
        var tarCG = tarCamera.profile.colorGrading.settings;

        tarCG.basic.postExposure = Mathf.Lerp(tarCG.basic.postExposure, tarProfile.colorGrading.settings.basic.postExposure, lerpSpeed * Time.deltaTime);
        tarCG.basic.temperature = Mathf.Lerp(tarCG.basic.temperature, tarProfile.colorGrading.settings.basic.temperature, lerpSpeed * Time.deltaTime);
        tarCG.basic.tint = Mathf.Lerp(tarCG.basic.tint, tarProfile.colorGrading.settings.basic.tint, lerpSpeed * Time.deltaTime);
        tarCG.basic.hueShift = Mathf.Lerp(tarCG.basic.hueShift, tarProfile.colorGrading.settings.basic.hueShift, lerpSpeed * Time.deltaTime);
        tarCG.basic.saturation = Mathf.Lerp(tarCG.basic.saturation, tarProfile.colorGrading.settings.basic.saturation, lerpSpeed * Time.deltaTime);
        tarCG.basic.contrast = Mathf.Lerp(tarCG.basic.contrast, tarProfile.colorGrading.settings.basic.contrast, lerpSpeed * Time.deltaTime);

        tarCG.channelMixer.red = Vector3.Lerp(tarCG.channelMixer.red, tarProfile.colorGrading.settings.channelMixer.red, lerpSpeed * Time.deltaTime);
        tarCG.channelMixer.green = Vector3.Lerp(tarCG.channelMixer.green, tarProfile.colorGrading.settings.channelMixer.green, lerpSpeed * Time.deltaTime);
        tarCG.channelMixer.blue = Vector3.Lerp(tarCG.channelMixer.blue, tarProfile.colorGrading.settings.channelMixer.blue, lerpSpeed * Time.deltaTime);

        tarCamera.profile.colorGrading.settings = tarCG;
    }

    void Disable ()
    {
        GetComponent<PostProcessingChange>().enabled = false;
    }
}
