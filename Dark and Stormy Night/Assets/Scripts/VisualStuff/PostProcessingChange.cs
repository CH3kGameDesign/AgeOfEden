using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingChange : MonoBehaviour {

    public PostProcessingProfile tarProfile;
    public PostProcessingBehaviour tarCamera;
    
    public float lerpSpeed;
    public float disableTime = 5;

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
        Invoke("Disable", disableTime);
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

        float lerpTime = lerpSpeed * Time.deltaTime;
        
        tarCG.basic.postExposure = Mathf.Lerp(tarCG.basic.postExposure, tarProfile.colorGrading.settings.basic.postExposure, lerpTime);
        tarCG.basic.temperature = Mathf.Lerp(tarCG.basic.temperature, tarProfile.colorGrading.settings.basic.temperature, lerpTime);
        tarCG.basic.tint = Mathf.Lerp(tarCG.basic.tint, tarProfile.colorGrading.settings.basic.tint, lerpTime);
        tarCG.basic.hueShift = Mathf.Lerp(tarCG.basic.hueShift, tarProfile.colorGrading.settings.basic.hueShift, lerpTime);
        tarCG.basic.saturation = Mathf.Lerp(tarCG.basic.saturation, tarProfile.colorGrading.settings.basic.saturation, lerpTime);
        tarCG.basic.contrast = Mathf.Lerp(tarCG.basic.contrast, tarProfile.colorGrading.settings.basic.contrast, lerpTime);

        tarCG.channelMixer.red = Vector3.Lerp(tarCG.channelMixer.red, tarProfile.colorGrading.settings.channelMixer.red, lerpTime);
        tarCG.channelMixer.green = Vector3.Lerp(tarCG.channelMixer.green, tarProfile.colorGrading.settings.channelMixer.green, lerpTime);
        tarCG.channelMixer.blue = Vector3.Lerp(tarCG.channelMixer.blue, tarProfile.colorGrading.settings.channelMixer.blue, lerpTime);
        
        tarCG.colorWheels.linear.lift = Color.Lerp(tarCG.colorWheels.linear.lift, tarProfile.colorGrading.settings.colorWheels.linear.lift, lerpTime);
        tarCG.colorWheels.linear.gamma = Color.Lerp(tarCG.colorWheels.linear.gamma, tarProfile.colorGrading.settings.colorWheels.linear.gamma, lerpTime);
        tarCG.colorWheels.linear.gain = Color.Lerp(tarCG.colorWheels.linear.gain, tarProfile.colorGrading.settings.colorWheels.linear.gain, lerpTime);

        tarCG.colorWheels.log.slope = Color.Lerp(tarCG.colorWheels.log.slope, tarProfile.colorGrading.settings.colorWheels.log.slope, lerpTime);
        tarCG.colorWheels.log.power = Color.Lerp(tarCG.colorWheels.log.power, tarProfile.colorGrading.settings.colorWheels.log.power, lerpTime);
        tarCG.colorWheels.log.offset = Color.Lerp(tarCG.colorWheels.log.offset, tarProfile.colorGrading.settings.colorWheels.log.offset, lerpTime);
        

        tarCamera.profile.colorGrading.settings = tarCG;
    }

    void Disable ()
    {
        GetComponent<PostProcessingChange>().enabled = false;
    }
}
