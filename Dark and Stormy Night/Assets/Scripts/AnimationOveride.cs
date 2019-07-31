using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOveride : MonoBehaviour {

    public Animator tarAnimator;
    public bool player;

    public string parameter;
    public bool value;

    public bool activateOnStart;

    public float waitTime;
    private float timer;

	// Use this for initialization
	void Start () {
        if (tarAnimator == false && player == true)
            tarAnimator = Movement.player.GetComponent<Movement>().playerModel.GetComponent<Animator>();
        /*
        if (activateOnStart)
            Change();
            */
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= waitTime)
            Change();
        else
            timer += Time.deltaTime;
	}

    public void Change()
    {
        tarAnimator.GetComponent<Animator>().SetBool(parameter, value);
    }
}
