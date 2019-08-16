using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectsRandom : MonoBehaviour
{
    public List<GameObject> pushee = new List<GameObject>();
    public bool childObjects;

    public float pushForce;

    private void Start()
    {
        if (childObjects)
        {
            pushee = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
                pushee.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < pushee.Count; i++)
            if (pushee[i].GetComponent<Rigidbody>())
                pushee[i].GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * pushForce, ForceMode.Impulse);
        GetComponent<PushObjectsRandom>().enabled = false;
    }
}