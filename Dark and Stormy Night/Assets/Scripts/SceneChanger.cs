using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("Loads the new scene automatically with no trigger")]
    [SerializeField]
    private bool m_bActivateOnStart = false;
    [Tooltip("Loads the new scene into the current scene")]
    [SerializeField]
    private bool m_bAdditive;
    [Tooltip("Loads the new scene in the background")]
    [SerializeField]
    private bool m_bAsync = true;
    [Tooltip("The desired scene index to be loaded")]
    [SerializeField]
    private sbyte m_sbSceneToLoad;

    [Space(8)]
    [Tooltip("")]
    [SerializeField]
    private bool m_bFadeOut = false;
    [Tooltip("Only used if fade out is enabled")]
    [SerializeField]
    private float m_fFadeSpeed = 4;

    public bool flipOnReset = false;

    private bool m_bAdded;
    private Scene m_sTargetScene;

    [Space(8)]
    public UnityEngine.Events.UnityEvent eventOnStart = new UnityEngine.Events.UnityEvent();

    // Called once before the first frame
    private void Start()
    {
        if (m_sbSceneToLoad == -2)
            m_sbSceneToLoad = (sbyte)SceneManager.GetActiveScene().buildIndex;

        if (m_bActivateOnStart)
            StartLoad();
	}
	
	// Update is called once per frame
	private void Update()
    {
        if (m_bFadeOut && m_bAdded)
        {
            var tarCG = Camera.main.GetComponent<
                UnityEngine.PostProcessing.PostProcessingBehaviour>().
                profile.colorGrading.settings;

            tarCG.basic.postExposure = Mathf.Lerp(
                tarCG.basic.postExposure, -10f, Time.deltaTime /m_fFadeSpeed);

            if (tarCG.basic.postExposure <= -8f)
            {
                tarCG.basic.postExposure = -10f;
                Camera.main.GetComponent<
                    UnityEngine.PostProcessing.PostProcessingBehaviour>().
                    profile.colorGrading.settings = tarCG;

                MidLoad();
                m_bFadeOut = false;
            }
            else
            {
                Camera.main.GetComponent<UnityEngine.PostProcessing.
                    PostProcessingBehaviour>().profile.colorGrading.settings = tarCG;
            }
        }
    }

    public void StartLoad()
    {
        if (!m_bAdditive)
            SmoothCameraMovement.s_bFlipOnReset = flipOnReset;
        eventOnStart.Invoke();
        if (!m_bAdded)
        {
            m_bAdded = true;
            if (!m_bFadeOut)
                MidLoad();
        }
    }

    private void MidLoad()
    {
        if (m_sbSceneToLoad == -1)
        {
            #if UNITY_STANDALONE
                Application.Quit();
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            if (m_bAsync)
            {
                if (m_bAdditive)
                    StartCoroutine(LoadNewSceneAdditive());
                else
                    StartCoroutine(LoadNewScene());
            }
            else
                SceneManager.LoadScene(m_sbSceneToLoad);
        }
    }

    /// <summary>
    /// Finishes the loading sequence
    /// </summary>
    private void FinishLoad()
    {
        m_sTargetScene = SceneManager.GetSceneByBuildIndex(m_sbSceneToLoad);
        SceneManager.SetActiveScene(m_sTargetScene);
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
            m_sbSceneToLoad, LoadSceneMode.Single);

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
            m_sbSceneToLoad, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        FinishLoad();
    }
}