using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableAfterTime : MonoBehaviour {


    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    public UnityEvent activateEvent;


    public float time;
    public bool repeatable = false;



	// Use this for initialization
	void Start () {
        if (!repeatable)
        Invoke("DoThing", time);
	}
    private void OnEnable()
    {
        if (repeatable)
            Invoke("DoThing", time);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void DoThing ()
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
