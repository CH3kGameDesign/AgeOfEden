using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixture : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField]
    private List<Light> m_lLights = new List<Light>();
    [SerializeField]
    private List<MeshRenderer> m_mrFixtures = new List<MeshRenderer>();

    public List<GameObject> LightObjects = new List<GameObject>();
    public List<GameObject> DarkObjects = new List<GameObject>();
    public bool randomDarkObject = false;
    [Space(10)]
    public List<GameObject> RandomDarkSounds = new List<GameObject>();
    [Space (10)]
    [Header ("Variables")]

    public Vector2 flashOffTimesBounds;
    public Vector2 flashOnTimesBounds;
    public Vector2 timeBetweenSetsBounds;
    public Vector2 flashPerSetBounds;
    [Space (10)]
    private float setTimer = 0;
    private float flashTimer = 0;
    private int flashCounter = 0;

    private float flashOffTimes;
    private float flashOnTimes;
    private float timeBetweenSets;
    private int flashPerSet;
    private int darkObject;

    [Header ("Materials")]

    public Material onMat;
    public Material offMat;
    [Space(10)]
    public float m_fOnAmount;
    public float m_fOffAmount;
    

    [HideInInspector]
    public bool on;
    public bool active;

    // Use this for initialization
    private void Start()
    {
        on = true;
        if (m_lLights.Count > 0)
        {
            for (int i = 0; i < m_lLights.Count; i++)
                m_lLights[i].intensity = m_fOnAmount;

            for (int i = 0; i < m_mrFixtures.Count; i++)
                m_mrFixtures[i].material = onMat;
        }

        flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
        flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
        timeBetweenSets = Random.Range(timeBetweenSetsBounds.x, timeBetweenSetsBounds.y);
        flashPerSet = Mathf.RoundToInt(Random.Range(flashPerSetBounds.x, flashPerSetBounds.y));

        for (int i = 0; i < LightObjects.Count; i++)
            LightObjects[i].SetActive(on);

        for (int i = 0; i < DarkObjects.Count; i++)
            DarkObjects[i].SetActive(!on);
    }
	
	// Update is called once per frame
	private void Update()
    {
        if (active)
        {
            if (setTimer > timeBetweenSets)
            {
                if (flashTimer < flashOffTimes)
                {
                    on = false;
                    if (!GetComponent<CandleFixture>())
                    {
                        if (m_lLights.Count > 0)
                        {
                            for (int i = 0; i < m_lLights.Count; i++)
                                m_lLights[i].intensity = m_fOffAmount;

                            for (int i = 0; i < m_mrFixtures.Count; i++)
                                m_mrFixtures[i].material = offMat;
                        }
                    }
                }
                else
                {
                    on = true;
                    if (!GetComponent<CandleFixture>())
                    {
                        if (m_lLights.Count > 0)
                        {
                            for (int i = 0; i < m_lLights.Count; i++)
                                m_lLights[i].intensity = m_fOnAmount;

                            for (int i = 0; i < m_mrFixtures.Count; i++)
                                m_mrFixtures[i].material = onMat;
                        }
                    }

                    if (flashTimer > flashOffTimes + flashOnTimes)
                    {
                        darkObject = Random.Range(0, DarkObjects.Count);
                        if (flashCounter < flashPerSet)
                        {
                            flashTimer = 0;
                            flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
                            flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
                            flashCounter++;
                        }
                        else
                        {
                            flashCounter = 0;
                            setTimer = 0;
                            timeBetweenSets = Random.Range(
                                timeBetweenSetsBounds.x, timeBetweenSetsBounds.y);

                            flashPerSet = Mathf.RoundToInt(
                                Random.Range(flashPerSetBounds.x, flashPerSetBounds.y));
                        }
                    }
                }
                for (int i = 0; i < LightObjects.Count; i++)
                    LightObjects[i].SetActive(on);

                if (randomDarkObject == false)
                    for (int i = 0; i < DarkObjects.Count; i++)
                        DarkObjects[i].SetActive(!on);
                else
                {
                    if (on == false)
                    {
                        DarkObjects[darkObject].SetActive(true);
                        if (RandomDarkSounds.Count != 0)
                            Instantiate(RandomDarkSounds[Random.Range(0, RandomDarkSounds.Count)]);
                    }
                    else
                        for (int i = 0; i < DarkObjects.Count; i++)
                            DarkObjects[i].SetActive(false);
                }
            }
            else
                flashTimer = 0;

            setTimer += Time.deltaTime;
            flashTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Sets the active state
    /// </summary>
    /// <param name="truth">The desired state</param>
    public void Activate(bool truth)
    {
        on = true;
        active = truth;
    }
}