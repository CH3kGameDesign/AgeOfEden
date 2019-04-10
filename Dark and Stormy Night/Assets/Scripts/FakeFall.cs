using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFall : MonoBehaviour {

    public Vector3 PlayerResetPos;

    public NeverEndingCorridor NECorridor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = PlayerResetPos;
            if (NECorridor != null)
                NECorridor.transform.GetChild(0).transform.position += new Vector3(10000, 0, 0);
        }
    }
}
