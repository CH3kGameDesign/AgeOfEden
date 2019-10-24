using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public bool fade = false;
    public float fadeWait = 100000;

    public float fadeSpeed = 1;

    private TextMeshPro m_tmpText;
    private MeshRenderer m_mrMesh;
    private RawImage m_riImage;

    private void Start()
    {
        m_tmpText = GetComponent<TextMeshPro>();
        m_mrMesh = GetComponent<MeshRenderer>();
        m_riImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (fadeWait <= 0)
            fade = true;

        if (fade)
        {
            if (m_tmpText)
            {
                m_tmpText.color = Color.Lerp(
                    m_tmpText.color, Color.clear,
                    fadeSpeed * Time.deltaTime); 

                if (m_tmpText.color.a == 0)
                    Destroy(gameObject);
            }
            else
            {
                if (m_mrMesh)
                {
                    m_mrMesh.material.color = Color.Lerp(
                        m_mrMesh.material.color,
                        Color.clear, fadeSpeed * Time.deltaTime);

                    if (m_mrMesh.material.color.a == 0)
                        Destroy(gameObject);
                }
                else
                {
                    if (m_riImage)
                    {
                        m_riImage.color = Color.Lerp(
                            m_riImage.color, Color.clear,
                            fadeSpeed * Time.deltaTime);

                        if (m_riImage.color.a <= 0.05)
                            Destroy(gameObject);
                    }
                }
            }
        }
        fadeWait -= Time.deltaTime;
    }
}