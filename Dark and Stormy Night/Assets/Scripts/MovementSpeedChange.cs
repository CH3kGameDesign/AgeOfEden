using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedChange : MonoBehaviour
{
    public float tarSpeedMultiplier;
    public bool canSprint = true;

	// Called once before the first frame
	private void Start()
    {
        Movement.s_goPlayerObject.GetComponent<Movement>()
            .m_fSpeedMultiplier = tarSpeedMultiplier;
        Movement.s_goPlayerObject.GetComponent<Movement>()
            .m_bCanSprint = canSprint;

        enabled = false;
	}
}