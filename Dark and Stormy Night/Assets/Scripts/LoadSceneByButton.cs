using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByButton : MonoBehaviour
{
    [SerializeField]
    private bool m_bDebug = false;

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
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }
}