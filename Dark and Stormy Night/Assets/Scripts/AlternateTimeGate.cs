using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateTimeGate : MonoBehaviour {

    public bool greaterX;
    public bool maskOne;

    public GameObject activateHolder;
    public GameObject deactivateHolder;

    public Shader MaskOne;
    public Shader MaskTwo;
    public Shader Visible;
    public Shader Hidden;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("1");
            if (other.transform.position.x > transform.position.x)
            {
                Debug.Log("2");
                if (greaterX == true)
                    ActivateObjects();
                else
                    DeactivateObjects();
            }
            if (other.transform.position.x < transform.position.x)
            {
                Debug.Log("2a");
                if (greaterX == false)
                    DeactivateObjects();
                else
                    ActivateObjects();
            }
        }
    }

    private void ActivateObjects()
    {
        Debug.Log("Woop");
        for (int i = 0; i < activateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
        {
            activateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = Visible;
        }
        for (int i = 0; i < deactivateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
        {
            deactivateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = Hidden;
        }
    }

    private void DeactivateObjects()
    {
        Debug.Log("Waap");
        if (maskOne == true)
        {
            for (int i = 0; i < activateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                activateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = MaskOne;
            }
            for (int i = 0; i < deactivateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                deactivateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = MaskTwo;
            }
        }
        else
        {
            for (int i = 0; i < activateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                activateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = MaskTwo;
            }
            for (int i = 0; i < deactivateHolder.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                deactivateHolder.GetComponentsInChildren<MeshRenderer>()[i].material.shader = MaskOne;
            }
        }
    }
}
