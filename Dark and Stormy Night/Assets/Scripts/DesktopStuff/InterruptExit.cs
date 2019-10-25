using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterruptExit : MonoBehaviour {
    public bool active;
    public UnityEvent activateOnExitAttempt = new UnityEvent();
    public bool removeOnDisable = true;

	// Use this for initialization
	void Start () {
        Application.wantsToQuit += WantsToQuit;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnApplicationQuit()
    {
        if (active)
            activateOnExitAttempt.Invoke();
    }

    private void OnDisable()
    {
        if (removeOnDisable)
            Application.wantsToQuit -= WantsToQuit;
    }

    static bool WantsToQuit()
    {
        Debug.Log("Player prevented from quitting.");
        
        return false;
    }

}
