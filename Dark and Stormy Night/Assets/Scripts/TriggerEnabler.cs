using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnabler : MonoBehaviour {

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    [Space (10)]

    public List<GameObject> Collider = new List<GameObject>();
    public List<GameObject> ColliderDisable = new List<GameObject>();

    public float Timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("EnableObjects", Timer);
        }
    }

    private void EnableObjects ()
    {
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(true);
        }
        for (int i = 0; i < GODisable.Count; i++)
        {
            GODisable[i].SetActive(false);
        }

        for (int i = 0; i < Collider.Count; i++)
        {
            for (int j = 0; j < Collider[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                Collider[i].GetComponentsInChildren<Collider>()[j].enabled = true;
            }
            
        }
        for (int i = 0; i < ColliderDisable.Count; i++)
        {
            for (int j = 0; j < ColliderDisable[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                ColliderDisable[i].GetComponentsInChildren<Collider>()[j].enabled = false;
            }
        }
    }
}
