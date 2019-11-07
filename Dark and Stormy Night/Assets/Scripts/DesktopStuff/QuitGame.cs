using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_STANDALONE
                Application.Quit();
#endif

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
