using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour {

    public Material tarSkybox;
    public float lerpPerSecond;

    public bool activateOnStart;
    public float disableTime = 5;

	// Use this for initialization
	void Start () {
		if (activateOnStart)
        {
            RenderSettings.skybox.SetTexture("_RightTex", tarSkybox.GetTexture("_RightTex"));
            RenderSettings.skybox.SetTexture("_LeftTex", tarSkybox.GetTexture("_LeftTex"));
            RenderSettings.skybox.SetTexture("_UpTex", tarSkybox.GetTexture("_UpTex"));
            RenderSettings.skybox.SetTexture("_DownTex", tarSkybox.GetTexture("_DownTex"));
            RenderSettings.skybox.SetTexture("_FrontTex", tarSkybox.GetTexture("_FrontTex"));
            RenderSettings.skybox.SetTexture("_BackTex", tarSkybox.GetTexture("_BackTex"));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (activateOnStart)
            Change();
        Invoke("Disable", disableTime);
	}

    void Change ()
    {
        RenderSettings.skybox.SetColor("_Tint", Color.Lerp(RenderSettings.skybox.GetColor("_Tint"), tarSkybox.GetColor("_Tint"), lerpPerSecond * Time.deltaTime));
    }

    void Disable ()
    {
        GetComponent<ChangeSkybox>().enabled = false;
    }
}
