using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnabler : MonoBehaviour {

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    [Space (10)]

    public List<GameObject> Collider = new List<GameObject>();
    public List<GameObject> ColliderDisable = new List<GameObject>();

    [Space(10)]

    public List<Rigidbody> Gravity = new List<Rigidbody>();
    public List<Rigidbody> GravityDisable = new List<Rigidbody>();

    [Space(10)]

    public List<SoftRotation> SoftRotation = new List<SoftRotation>();
    public List<SoftRotation> SoftRotationDisable = new List<SoftRotation>();

    [Space(10)]

	public List<FadeOut> FadeOut = new List<FadeOut>();

	[Space(10)]

    public float Timer;

    public UnityEvent activateEvent;
    public enum Collision {OnEnter, OnExit};

    public Collision collision;

	// Use this for initialization
	void Start () {
        if (activateEvent == null)
            activateEvent = new UnityEvent();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player" && collision == Collision.OnEnter)
        {
            Invoke("EnableObjects", Timer);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && collision == Collision.OnExit)
        {
            Invoke("EnableObjects", Timer);
        }
    }

    private void EnableObjects ()
    {
        activateEvent.Invoke();
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

        for (int i = 0; i < Gravity.Count; i++)
        {
            Gravity[i].useGravity = true;
            Gravity[i].isKinematic = false;
        }
        for (int i = 0; i < GravityDisable.Count; i++)
        {
            Gravity[i].useGravity = false;
            Gravity[i].isKinematic = true;
        }

        for (int i = 0; i < SoftRotation.Count; i++)
        {
            SoftRotation[i].enabled = true;
        }
        for (int i = 0; i < SoftRotationDisable.Count; i++)
        {
            SoftRotationDisable[i].enabled = false;
        }

		for (int i = 0; i < FadeOut.Count; i++)
		{
			FadeOut[i].enabled = true;
		}

    }
}
