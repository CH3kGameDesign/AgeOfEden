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
            float timerOpacity = 0;
            if (timerText > 2)
                timerOpacity = Mathf.Abs(timerText - 2.5f) + 0.3f;
            else
            {
                if (timerText > 1)
                    timerOpacity = Mathf.Abs(timerText - 1.5f) + 0.3f;
                else
                    timerOpacity = Mathf.Abs(timerText - 0.5f) + 0.3f;
            }
            //TMProUI.text = "Quitting In: " + timerText;
            TMProUI.color = new Color(1, 1, 1, timerOpacity);
            TMProUI.text = "Quitting...";
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
#if UNITY_STANDALONE
            Application.Quit();
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}