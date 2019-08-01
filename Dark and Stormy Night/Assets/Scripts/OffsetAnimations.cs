using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimations : MonoBehaviour
{
	// Use this for initialization
	private void Awake ()
    {
        Animator anim = GetComponent<Animator>();

        //Set a random part of the animation to start from
        float randomIdleStart = Random.Range(0, anim.GetCurrentAnimatorStateInfo(0).length);
        anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, randomIdleStart);
        anim.speed = Random.Range(0.2f, 0.4f);
    }
}
