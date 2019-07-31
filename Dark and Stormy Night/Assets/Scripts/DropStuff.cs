using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropStuff : MonoBehaviour {

    public List<Transform> objectsToDrop = new List<Transform>();
    private List<float> waitTime = new List<float>();

    public float waitStart;
    public GameObject ActivateOnFinish;

    private float timer;

	// Use this for initialization
	void Start () {
        waitTime.Add(waitStart);
        
        for (int i = 1; i < objectsToDrop.Count; i++)
        {
            float waitCurrent = waitStart + (0.25f * i);
            waitTime.Add(waitCurrent);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < objectsToDrop.Count; i++)
        {
            if (timer >= waitTime[i])
            {
                float speed = Mathf.Clamp(timer - waitTime[i], 0, 1);
                speed *= 10;
                objectsToDrop[i].position -= Vector3.up * Time.deltaTime * speed;
                if (i == objectsToDrop.Count - 1)
                    Invoke("Finish", 5);
            }
        }
        timer += Time.deltaTime;
	}

    void Finish ()
    {
        for (int i = 0; i < objectsToDrop.Count; i++)
        {
            objectsToDrop[i].gameObject.SetActive(false);
        }
        ActivateOnFinish.SetActive(true);
        GetComponent<DropStuff>().enabled = false;
    }
}
