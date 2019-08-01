using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int sceneToLoad;

    public bool additive;
    public bool activateOnStart = false;

    private bool added;
    private Scene tarScene;

	// Called once before the first frame
	private void Start ()
    {
        tarScene = SceneManager.GetSceneByBuildIndex(sceneToLoad);

        if (sceneToLoad == -2)
            sceneToLoad = SceneManager.GetActiveScene().buildIndex;

        if (activateOnStart)
            StartLoad();
	}
	
    /// <summary>
    /// Starts the loading sequence
    /// </summary>
    public void StartLoad()
    {
        if (sceneToLoad == -1)
            Application.Quit();

        else if (!added)
        {
            //SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            added = true;

            if (additive)
                StartCoroutine(LoadNewSceneAdditive());
            else
                StartCoroutine(LoadNewScene());
            //Invoke("FinishLoad", 0.5f);
        }
    }

    /// <summary>
    /// Finishes the loading sequence
    /// </summary>
    private void FinishLoad ()
    {
        //SceneManager.SetActiveScene(tarScene);
        if (GetComponent<MoveTo>())
            GetComponent<MoveTo>().moveOnStart = true;
    }

    /// <summary>
    /// Loads scene in the background
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(
            sceneToLoad, LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Loads scene additively in the background
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadNewSceneAdditive()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(
            sceneToLoad, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        FinishLoad();
    }
}
