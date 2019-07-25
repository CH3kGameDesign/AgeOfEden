using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour {

    public GameObject playerManagerObjects;

	// Use this for initialization
	void Start () {
        if (Movement.player != null)
            playerManagerObjects = Movement.player.transform.parent.parent.gameObject;
        StartCoroutine(UnloadScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator UnloadScene()
    {
        SceneManager.MoveGameObjectToScene(playerManagerObjects, SceneManager.GetSceneAt(1));
        AsyncOperation async = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0).buildIndex);

        while (!async.isDone)
        {
            yield return null;
        }

    }
}
