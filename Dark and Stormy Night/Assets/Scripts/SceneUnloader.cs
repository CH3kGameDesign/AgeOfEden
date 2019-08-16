﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    public GameObject playerManagerObjects;
    public bool justAudio;

	// Called once before the first frame
	private void Start ()
    {
        if (Movement.m_goPlayerObject)
        {
            if (justAudio)
            {
                GameObject GO = CameraMovement.s_CameraObject.transform.parent.GetChild(1).gameObject;
                GO.transform.parent = null;
                playerManagerObjects = GO;
            }
            else
                playerManagerObjects = Movement.m_goPlayerObject.transform.parent.parent.gameObject;
        }

        StartCoroutine(UnloadScene());
	}
	
    /// <summary>
    /// Unloads a scene in the background
    /// </summary>
    /// <returns></returns>
    IEnumerator UnloadScene()
    {
        SceneManager.MoveGameObjectToScene(
            playerManagerObjects, SceneManager.GetSceneAt(1));

        AsyncOperation async = SceneManager.UnloadSceneAsync(
            SceneManager.GetSceneAt(0).buildIndex);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
