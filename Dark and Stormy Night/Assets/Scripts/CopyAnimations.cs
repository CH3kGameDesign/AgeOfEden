using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyAnimations : MonoBehaviour
{
    public Animator target;
    public bool copyPlayer = false;

    private Animator m_aLocalAnimator;

    private void Start()
    {
        if (copyPlayer)
            target = Movement.s_goPlayerObject.GetComponent<Movement>().m_aModelAnimator;

        AnimatorStateInfo animationState = target.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = target.GetCurrentAnimatorClipInfo(0);
        float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;

        m_aLocalAnimator = GetComponent<Animator>();

        m_aLocalAnimator.Play(target.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, myTime);
    }

    // Update is called once per frame
    private void Update()
    {
        m_aLocalAnimator.SetBool("Standing", target.GetBool("Standing"));
        m_aLocalAnimator.SetBool("Moving", target.GetBool("Moving"));
        m_aLocalAnimator.SetBool("Sprinting", target.GetBool("Sprinting"));
    }
}