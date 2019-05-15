using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionManager : MonoBehaviour {

    [Header("Options")]
    public Slider camSensitivitySlider;
    public Text camSensitivityText;
    [Space(10)]
    public TextMeshProUGUI windowedText;
    public TextMeshProUGUI fullscreenText;
    

	// Use this for initialization
	void Start () {
        Invoke("LateStart", 0.01f);
        if (Screen.fullScreen == false)
        {
            windowedText.color = new Color(1, 1, 1, 1);
            fullscreenText.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            windowedText.color = new Color(1, 1, 1, 0.3f);
            fullscreenText.color = new Color(1, 1, 1, 1);
        }
    }
	
    void LateStart ()
    {
        float camSensitivity = CameraMovement.cameraObject.GetComponent<SmoothCameraMovement>().sensitivityX;
        camSensitivitySlider.value = camSensitivity;
        camSensitivityText.text = camSensitivitySlider.value.ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CamSensitivityValueChange(int add)
    {
        CameraMovement.cameraObject.GetComponent<SmoothCameraMovement>().sensitivityX += add;
        CameraMovement.cameraObject.GetComponent<SmoothCameraMovement>().sensitivityY += add;

        camSensitivitySlider.value += add;
        camSensitivityText.text = camSensitivitySlider.value.ToString();
    }
    public void ScreenChange (int windowStyle)
    {
        if (windowStyle == 0)
        {
            Screen.fullScreen = false;
            windowedText.color = new Color(1, 1, 1, 1);
            fullscreenText.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            Screen.fullScreen = true;
            windowedText.color = new Color(1, 1, 1, 0.3f);
            fullscreenText.color = new Color(1, 1, 1, 1);
        }
    }
}
