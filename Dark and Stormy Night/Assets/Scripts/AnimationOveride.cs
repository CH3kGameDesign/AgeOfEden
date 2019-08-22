using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOveride : MonoBehaviour
{
    public Animator tarAnimator;
    public bool player;

    public string parameter;
    public bool value;

    public bool activateOnStart;

    public float waitTime;
    private float timer;

	// Use this for initialization
	private void Start()
    {
        if (!tarAnimator && player)
            tarAnimator = Movement.m_goPlayerObject.GetComponent<Movement>().
                m_aModelAnimator;
        /*
        if (activateOnStart)
            Change();
            */
	}
	
	// Update is called once per frame
	private void Update()
    {
        if (timer >= waitTime)
            Change();
        else
            timer += Time.deltaTime;
	}

    /// <summary>
    /// Sets an animation variable to a preset
    /// </summary>
    public void Change()
    {
        tarAnimator.GetComponent<Animator>().SetBool(parameter, value);
    }
}