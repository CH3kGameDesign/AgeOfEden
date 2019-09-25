using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    public bool playerChild;
    public Transform child;
    public Transform parent;

    public bool activateOnStart = true;

	// Use this for initialization
	private void Start()
    {
        if (!child && playerChild)
            child = Movement.s_goPlayerObject.transform;
        
        if (activateOnStart)
            Activate();
	}
	
    /// <summary>
    /// Sets the parent of an object to a designated object
    /// </summary>
    public void Activate()
    {
        child.SetParent(parent);
    }
}