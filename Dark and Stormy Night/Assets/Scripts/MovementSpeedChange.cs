using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedChange : MonoBehaviour
{
    public float tarSpeedMultiplier;

	// Called once before the first frame
	private void Start()
    {
        Movement.s_goPlayerObject.GetComponent<Movement>()
            .m_fSpeedMultiplier = tarSpeedMultiplier;
        GetComponent<MovementSpeedChange>().enabled = false;
	}
}