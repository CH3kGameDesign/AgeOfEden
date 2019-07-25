using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class RorschachManager : MonoBehaviour {

    public Renderer rorschachQuad;
    public TextMeshPro titleText;
    public ParticleSystem particles;
    [Space(10)]
    public float speed;

    [Space (20)]

    public int sceneToLoad;

    [Space(20)]
    public GameObject titlePlayerObject;

    private Scene tarScene;
    private int stage = 0;

    private bool sceneLoaded = false;
    private bool transitionOver = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadNewScene());
    }
	
	// Update is called once per frame
	void Update () {
        if (stage == 0)
            Change();
        if (sceneLoaded == true && transitionOver == true)
            Invoke("FinishLoad", 3);
	}

    void Change ()
    {
        titleText.alpha = Mathf.Lerp(titleText.alpha, 0, Time.deltaTime / speed);

        var main = particles.main;
        main.startColor = Color.Lerp(particles.main.startColor.color, Color.clear, Time.deltaTime / speed);

        float lerpNum = Mathf.Lerp(rorschachQuad.material.GetFloat("_Contrast"), 0, Time.deltaTime / speed);
        rorschachQuad.material.SetFloat("_Contrast", lerpNum);

        if (titleText.alpha < 0.01f)
        {
            titleText.alpha = 0;
            main.startColor = Color.clear;
            rorschachQuad.material.SetFloat("_Contrast", 0);
            stage = -1;
            transitionOver = true;
        }
    }

    void FinishLoad ()
    {
        var tarCG = Camera.main.GetComponent<PostProcessingBehaviour>().profile.colorGrading.settings;
        tarCG.basic.postExposure = -10;

        Camera.main.GetComponent<PostProcessingBehaviour>().profile.colorGrading.settings = tarCG;

        titlePlayerObject.SetActive(false);

        StartCoroutine(UnloadScene());
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        sceneLoaded = true;
    }

    IEnumerator UnloadScene()
    {
        AsyncOperation async = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0).buildIndex);

        while (!async.isDone)
        {
            yield return null;
        }

    }
}
