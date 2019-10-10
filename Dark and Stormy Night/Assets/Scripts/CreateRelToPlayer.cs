using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRelToPlayer : MonoBehaviour
{
    public float howFarInFront;
    public GameObject createPrefab;

	// Use this for initialization
	private void Start ()
    {
        Vector3 playerPos = Movement.s_goPlayerObject.transform.position;
        Vector3 tarPos =  playerPos + Movement.s_goPlayerObject.transform.forward * howFarInFront;
        Quaternion tarRot = Quaternion.LookRotation(tarPos - playerPos, Vector3.up);
        Instantiate(createPrefab, tarPos, tarRot);
	}
}