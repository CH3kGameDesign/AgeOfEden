using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateOnAnimation : MonoBehaviour {

    public string animationName;
    public Animator tarAnimator;

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    public UnityEvent activateEvent;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (tarAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
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
            GetComponent<ActivateOnAnimation>().enabled = false;
        }
	}
}
