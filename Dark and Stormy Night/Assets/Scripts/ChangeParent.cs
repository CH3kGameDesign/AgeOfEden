using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour {

    public Transform child;
    public Transform parent;

    public bool activateOnStart = true;

	// Use this for initialization
	void Start () {
        if (activateOnStart)
            Activate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate ()
    {
        child.SetParent(parent);
    }
}
