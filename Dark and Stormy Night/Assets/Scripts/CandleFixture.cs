using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFixture : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField]
    private List<Light> m_lLights = new List<Light>();
    [SerializeField]
    private List<MeshRenderer> m_mrFixtures = new List<MeshRenderer>();

    [Space(20)]
    public float m_fOnAmount;
    public float m_fOffAmount;
    [Space(10)]
    public float m_fChangePerFrame = 0.02f;

    private float m_fSaveIntensity;
    private List<Color> emissionStart;

    // Use this for initialization
    private void Start()
    {
        if (m_lLights.Count >= 1)
        {
            for (int i = 0; i < m_lLights.Count; i++)
            {
                m_lLights[i].intensity = m_fOnAmount;
            }
        }

        if (m_mrFixtures.Count >= 1)
        {
            for (int i = 0; i < m_mrFixtures.Count; i++)
            {
                Color tempColor = new Color(1, 1, 1, m_fOnAmount);
                m_mrFixtures[i].material.SetColor("_EmissionColor", tempColor);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GetComponent<LightFixture>() || GetComponent<LightFixture>().on)
        {
            if (m_lLights.Count > 0 && m_lLights[0].intensity < 0.1f)
            {
                for (int i = 0; i < m_lLights.Count; i++)
                    m_lLights[i].intensity = m_fSaveIntensity;

                for (int i = 0; i < m_mrFixtures.Count; i++)
                {
                    Color tempColor = Color.HSVToRGB(0, 0, m_fSaveIntensity);
                    m_mrFixtures[i].material.SetColor("_EmissionColor", tempColor);
                }
            }

            if (m_mrFixtures.Count >= 1)
            {
                float changeAmount = Random.Range(-m_fChangePerFrame, m_fChangePerFrame);
                if (changeAmount + m_lLights[0].intensity > m_fOnAmount)
                    changeAmount = -changeAmount;
                if (changeAmount + m_lLights[0].intensity < m_fOffAmount)
                    changeAmount = -changeAmount;

                for (int i = 0; i < m_lLights.Count; i++)
                    m_lLights[i].intensity += changeAmount;

                for (int i = 0; i < m_mrFixtures.Count; i++)
                {
                    Color tempColor = m_mrFixtures[i].material.GetColor("_EmissionColor");
                    tempColor += Color.HSVToRGB(0, 0, changeAmount);
                    m_mrFixtures[i].material.SetColor("_EmissionColor", tempColor);
                }
            }

            if (m_lLights.Count > 0)
                m_fSaveIntensity = m_lLights[0].intensity;
        }
        else if (GetComponent<LightFixture>())
        {
            for (int i = 0; i < m_lLights.Count; i++)
                m_lLights[i].intensity = 0;

            for (int i = 0; i < m_mrFixtures.Count; i++)
            {
                Color tempColor = Color.HSVToRGB(0, 0, 0);
                m_mrFixtures[i].material.SetColor("_EmissionColor", tempColor);
            }
        }
    }
}