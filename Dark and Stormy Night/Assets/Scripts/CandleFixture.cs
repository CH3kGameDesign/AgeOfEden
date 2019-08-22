using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFixture : MonoBehaviour
{
    [Header("GameObjects")]
    public List<Light> Lights = new List<Light>();
    public List<MeshRenderer> Fixtures = new List<MeshRenderer>();

    [Space(20)]
    public float onAmount;
    public float offAmount;
    [Space(10)]
    public float changePerFrame = 0.02f;

    private float saveIntensity;
    private List<Color> emissionStart = new List<Color>();

    // Use this for initialization
    private void Start()
    {
        if (Lights.Count >= 1)
            for (int i = 0; i < Lights.Count; i++)
            {
                Lights[i].intensity = onAmount;
            }
        if (Fixtures.Count >= 1)
            for (int i = 0; i < Fixtures.Count; i++)
            {
                Color tempColor = new Color(1, 1, 1, onAmount);
                Fixtures[i].material.SetColor("_EmissionColor", tempColor);
            }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GetComponent<LightFixture>() || GetComponent<LightFixture>().on)
        {
            if (Lights[0].intensity < 0.1f)
            {
                for (int i = 0; i < Lights.Count; i++)
                {
                    Lights[i].intensity = saveIntensity;
                }
                for (int i = 0; i < Fixtures.Count; i++)
                {
                    Color tempColor = Color.HSVToRGB(0, 0, saveIntensity);
                    Fixtures[i].material.SetColor("_EmissionColor", tempColor);
                }
            }

            float changeAmount = Random.Range(-changePerFrame, changePerFrame);
            if (changeAmount + Lights[0].intensity > onAmount)
                changeAmount = -changeAmount;
            if (changeAmount + Lights[0].intensity < offAmount)
                changeAmount = -changeAmount;

            if (Lights.Count >= 1)
            {
                for (int i = 0; i < Lights.Count; i++)
                {
                    Lights[i].intensity += changeAmount;
                }
            }

            if (Fixtures.Count >= 1)
            {
                for (int i = 0; i < Fixtures.Count; i++)
                {
                    Color tempColor = Fixtures[i].material.GetColor("_EmissionColor");
                    tempColor += Color.HSVToRGB(0, 0, changeAmount);
                    Fixtures[i].material.SetColor("_EmissionColor", tempColor);
                }
            }

            saveIntensity = Lights[0].intensity;
        }
        else if (GetComponent<LightFixture>())
        {
            for (int i = 0; i < Lights.Count; i++)
                Lights[i].intensity = 0;

            for (int i = 0; i < Fixtures.Count; i++)
            {
                Color tempColor = Color.HSVToRGB(0, 0, 0);
                Fixtures[i].material.SetColor("_EmissionColor", tempColor);
            }
        }
    }
}