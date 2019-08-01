using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public int sceneToLoad;

    public bool additive;
    public bool async = true;
    public bool activateOnStart = false;
    public bool fadeOut = false;

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
        if (fadeOut == true && added == true)
        {
            var tarCG = Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile.colorGrading.settings;
            tarCG.basic.postExposure = Mathf.Lerp(tarCG.basic.postExposure, -10f, Time.deltaTime /4);
            
            if (tarCG.basic.postExposure <= -8f)
            {
                tarCG.basic.postExposure = -10f;
                Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile.colorGrading.settings = tarCG;
                MidLoad();
                fadeOut = false;
            }
            else
                Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile.colorGrading.settings = tarCG;
        }
    }

    public void StartLoad()
    {
        if (added == false)
        {
            added = true;
            if (fadeOut == false)
                MidLoad();
        }
        
    }

    void MidLoad ()
    {
        if (sceneToLoad == -1)
            Application.Quit();
        else
        {
            if (async)
            {
                if (additive == true)
                    StartCoroutine(LoadNewSceneAdditive());
                else
                    StartCoroutine(LoadNewScene());
            }
            else
                SceneManager.LoadScene(sceneToLoad);
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
