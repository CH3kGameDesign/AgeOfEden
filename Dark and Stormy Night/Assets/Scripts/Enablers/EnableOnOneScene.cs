using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnableOnOneScene : MonoBehaviour {

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    [Space(10)]

    public float Timer;

    public UnityEvent activateEvent;

    // Use this for initialization
    void Start()
    {
        if (activateEvent == null)
            activateEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.sceneCount == 1)
        {
            EnableObjects();
            GetComponent<EnableOnOneScene>().enabled = false;
        }
    }

    private void EnableObjects()
    {
        activateEvent.Invoke();
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(true);
        }
        for (int i = 0; i < GODisable.Count; i++)
        {
            GODisable[i].SetActive(false);
        }

    }
}
