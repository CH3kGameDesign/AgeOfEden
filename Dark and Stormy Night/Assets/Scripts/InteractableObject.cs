﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header ("Colours")]
    public Color defaultColor;
    public Color hoverOverColor;
    public Color clickColor;

    [Header ("Variables")]
    public int interactionAnimation;

    [Header("GameObjects")]
    public GameObject grabObject;

    public bool inUse = false;
    
	// Update is called once per frame
	private void Update ()
    {
        if (Vector3.Distance(transform.position, Movement.m_goPlayerObject.transform.position) < 2)
        {
            if (!inUse)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(
                    GetComponent<SpriteRenderer>().color, hoverOverColor, Time.deltaTime);

                transform.localScale = Vector3.Lerp(
                    transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), Time.deltaTime);

                if (Input.GetMouseButtonDown(0))
                {
                    inUse = true;
                    GetComponent<SpriteRenderer>().color = clickColor;

                    Movement.m_goPlayerObject.GetComponent<Movement>().
                        m_aModelAnimator.GetComponent<Animator>().SetInteger("Interaction", 1);
                }
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(
                GetComponent<SpriteRenderer>().color, defaultColor, Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);
        }

        if (inUse)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale, Vector3.zero, Time.deltaTime * 2);

            Movement.canMove = false;
            Movement.m_goPlayerObject.transform.position = Vector3.Lerp(
                Movement.m_goPlayerObject.transform.position, transform.parent.GetChild(1).position,
                Time.deltaTime);

            CameraMovement.canMove = false;
            CameraMovement.cameraObject.transform.LookAt(grabObject.transform);
        }
    }
}
