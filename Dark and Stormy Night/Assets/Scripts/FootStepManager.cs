using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepManager : MonoBehaviour {


    [System.Serializable]
    public class footStepHolder
    {
        public List<GameObject> leftStep = new List<GameObject>();
        public List<GameObject> rightStep = new List<GameObject>();
    }

    [SerializeField]
    [Help("FOOTSTEP ORGANISATION\n\n   Element0: Carpet Steps\n   Element1: Wood Steps")]
    public int defaultSound;

    private int area = 100;
    private bool leftStepNext;

    public List<footStepHolder> footSteps = new List<footStepHolder>(1);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void MakeSound ()
    {
        int stepNo = 0;
        if (leftStepNext == false)
        {
            stepNo = Random.Range(0, 1);
        }
        leftStepNext = !leftStepNext;

        if (area != 100)
        {
            if (leftStepNext)
                Instantiate(footSteps[area].leftStep[stepNo], transform.position, transform.rotation);
            else
                Instantiate(footSteps[area].rightStep[stepNo], transform.position, transform.rotation);
        }
        else
        {
            if (leftStepNext)
                Instantiate(footSteps[defaultSound].leftStep[stepNo], transform.position, transform.rotation);
            else
                Instantiate(footSteps[defaultSound].rightStep[stepNo], transform.position, transform.rotation);
        }
    }

    void OnTriggerStay (Collider other)
    {
        int stepArea = 100;
        if (other.tag == "CarpetArea")
            stepArea = 0;
        if (other.tag == "WoodArea")
            stepArea = 1;

        if (stepArea < area)
            area = stepArea;
    }
    void OnTriggerExit(Collider other)
    {
        int stepArea = 100;
        if (other.tag == "CarpetArea")
            stepArea = 0;
        if (other.tag == "WoodArea")
            stepArea = 1;

        if (area == stepArea)
            area = 100;
    }
    
}
