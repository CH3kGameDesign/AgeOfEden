using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateOnAnimation : MonoBehaviour
{
    public string animationName;
    public Animator tarAnimator;

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    public UnityEvent activateEvent;

    private ActivateOnAnimation m_aoaScriptRef;

    // Use this for initialization
    private void Start ()
    {
        m_aoaScriptRef = GetComponent<ActivateOnAnimation>();

    }
	
	// Update is called once per frame
	private void Update ()
    {
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
            m_aoaScriptRef.enabled = false;
        }
	}
}
