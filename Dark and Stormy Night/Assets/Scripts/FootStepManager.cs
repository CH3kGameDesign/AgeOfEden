using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    [System.Serializable]
    public class footStepHolder
    {
        public List<GameObject> leftStep = new List<GameObject>();
        public List<GameObject> rightStep = new List<GameObject>();
    }

    [SerializeField]
    [Help("FOOTSTEP ORGANISATION\n\n" +
        "   Element0: Carpet Steps\n" +
        "   Element1: Wood Steps\n" +
        "   Element2: Tile Steps")]
    public int defaultSound;

    [HideInInspector]
    public bool makeNoises = true;
    public bool makeNoisesInspector = true;

    private int area = 100;
    private bool leftStepNext;

    public List<footStepHolder> footSteps = new List<footStepHolder>(1);

    private Transform m_tTransCache;

    private void Start()
    {
        m_tTransCache = transform;
    }

    /// <summary>
    /// Called to make a footstep sound
    /// </summary>
    public void MakeSound()
    {
        if (makeNoises && makeNoisesInspector)
        {
            int stepNo = 0;

            if (!leftStepNext)
                stepNo = Random.Range(0, 1);

            leftStepNext = !leftStepNext;

            if (area != 100)
            {
                if (leftStepNext)
                    Instantiate(footSteps[area].leftStep[stepNo],
                        m_tTransCache.position, m_tTransCache.rotation);
                else
                    Instantiate(footSteps[area].rightStep[stepNo],
                        m_tTransCache.position, m_tTransCache.rotation);
            }
            else
            {
                if (leftStepNext)
                    Instantiate(footSteps[defaultSound].leftStep[stepNo],
                        m_tTransCache.position, m_tTransCache.rotation);
                else
                    Instantiate(footSteps[defaultSound].rightStep[stepNo],
                        m_tTransCache.position, m_tTransCache.rotation);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int stepArea = 100;

        if (other.tag == "CarpetArea")
            stepArea = 0;
        if (other.tag == "WoodArea")
            stepArea = 1;
        if (other.tag == "TileArea")
            stepArea = 2;

        if (stepArea < area)
            area = stepArea;
    }

    private void OnTriggerExit(Collider other)
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