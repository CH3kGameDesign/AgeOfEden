using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    public Transform child;
    public bool playerChild;
    public Transform parent;

    public bool activateOnStart = true;

	// Use this for initialization
	void Start ()
    {
        if (child == null && playerChild == true)
            child = Movement.player.transform;

        if (activateOnStart)
            Activate();
	}
	
    /// <summary>
    /// Sets the parent of an object to a designated object
    /// </summary>
    public void Activate ()
    {
        child.SetParent(parent);
    }
}
