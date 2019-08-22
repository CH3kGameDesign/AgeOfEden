using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyAnimations : MonoBehaviour
{
    public Animator target;
    public bool copyPlayer = false;

    private void Start()
    {
        if (copyPlayer)
            target = Movement.m_goPlayerObject.GetComponent<Movement>().m_aModelAnimator;

        AnimatorStateInfo animationState = target.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = target.GetCurrentAnimatorClipInfo(0);
        float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;

        GetComponent<Animator>().Play(
            target.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, myTime);
    }

    // Update is called once per frame
    private void Update()
    {
        GetComponent<Animator>().SetBool("Standing", target.GetBool("Standing"));
        GetComponent<Animator>().SetBool("Moving", target.GetBool("Moving"));
        GetComponent<Animator>().SetBool("Sprinting", target.GetBool("Sprinting"));
    }
}