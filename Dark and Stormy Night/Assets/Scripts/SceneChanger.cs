using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public int sceneToLoad;

    public bool additive;

    private bool added;
    private Scene tarScene;

    public GameObject PlayerManagerObjects;

	// Use this for initialization
	void Start () {
        tarScene = SceneManager.GetSceneByBuildIndex(sceneToLoad);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void StartLoad()
    {
        if (added == false)
        {
            //SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            added = true;
            StartCoroutine(LoadNewScene());
            //Invoke("FinishLoad", 0.5f);
        }
    }

    void FinishLoad ()
    {
        //SceneManager.SetActiveScene(tarScene);
        if (GetComponent<MoveTo>() != null)
            GetComponent<MoveTo>().moveOnStart = true;
    }


    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
            FinishLoad();

    }
}
