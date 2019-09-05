using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRelToPlayer : MonoBehaviour {

    public float howFarInFront;
    public GameObject createPrefab;

	// Use this for initialization
	void Start () {
        Vector3 playerPos = Movement.m_goPlayerObject.transform.position;
        Vector3 tarPos =  playerPos + Movement.m_goPlayerObject.transform.forward * howFarInFront;
        Quaternion tarRot = Quaternion.LookRotation(tarPos - playerPos, Vector3.up);
        Instantiate(createPrefab, tarPos, tarRot);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
