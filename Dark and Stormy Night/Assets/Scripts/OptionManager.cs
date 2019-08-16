using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionManager : MonoBehaviour
{
    [Header("Options")]
    public Slider camSensitivitySlider;
    public Text camSensitivityText;
    [Space(10)]
    public TextMeshProUGUI windowedText;
    public TextMeshProUGUI fullscreenText;

	// Called once before the first frame
	private void Start()
    {
        Invoke("LateStart", 0.01f);
        if (!Screen.fullScreen)
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
	
    /// <summary>
    /// A delayed start function
    /// </summary>
    private void LateStart()
    {
        float camSensitivity = CameraMovement.s_CameraObject.
            GetComponent<SmoothCameraMovement>().sensitivityX;

        camSensitivitySlider.value = camSensitivity;
        camSensitivityText.text = camSensitivitySlider.value.ToString();
    }

    /// <summary>
    /// Adds a value to camera sensitivity
    /// </summary>
    /// <param name="pAddedValue">The value added to the current value</param>
    public void CamSensitivityValueChange(int pAddedValue)
    {
        CameraMovement.s_CameraObject.GetComponent
            <SmoothCameraMovement>().sensitivityX += pAddedValue;

        CameraMovement.s_CameraObject.GetComponent
            <SmoothCameraMovement>().sensitivityY += pAddedValue;

        camSensitivitySlider.value += pAddedValue;
        camSensitivityText.text = camSensitivitySlider.value.ToString();
    }

    /// <summary>
    /// Changes the current window state
    /// </summary>
    /// <param name="pWindowStyle"></param>
    public void ScreenChange(int pWindowStyle)
    {
        if (pWindowStyle == 0)
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