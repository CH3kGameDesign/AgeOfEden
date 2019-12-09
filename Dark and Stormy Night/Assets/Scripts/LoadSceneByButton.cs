using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadSceneByButton : MonoBehaviour
{
    [SerializeField]
    private bool m_bDebug = false;

    private float timer = 3;

    public GameObject canvasObject;
    public TextMeshProUGUI TMProUI;


    // Update is called once per frame
    private void Update()
    {
        if (m_bDebug)
        {
            /*
            if (Input.GetKeyDown(KeyCode.Keypad0))
                SceneManager.LoadScene(0);
            if (Input.GetKeyDown(KeyCode.Keypad1))
                SceneManager.LoadScene(1);
            if (Input.GetKeyDown(KeyCode.Keypad2))
                SceneManager.LoadScene(2);
            if (Input.GetKeyDown(KeyCode.Keypad3))
                SceneManager.LoadScene(3);
                */
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            timer -= Time.deltaTime;
            canvasObject.SetActive(true);
            float timerText = Mathf.Round(timer * 100);
            timerText /= 100;
            TMProUI.text = "Quitting In: " + timerText;
        }
        else
        {
            timer += Time.deltaTime * 3;
            if (timer > 3)
                timer = 3;
            canvasObject.SetActive(false);
        }
        if (timer <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}