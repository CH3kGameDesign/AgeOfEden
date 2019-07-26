using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public int sceneToLoad;

    public bool additive;
    public bool activateOnStart = false;

    private bool added;
    private Scene tarScene;

	// Use this for initialization
	void Start () {
        
        tarScene = SceneManager.GetSceneByBuildIndex(sceneToLoad);
        if (sceneToLoad == -2)
            sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        if (activateOnStart)
            StartLoad();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void StartLoad()
    {
        if (sceneToLoad == -1)
            Application.Quit();

        else if (added == false)
        {
            //SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            added = true;
            if (additive == true)
                StartCoroutine(LoadNewSceneAdditive());
            else
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
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadNewSceneAdditive()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        FinishLoad();

    }
}
