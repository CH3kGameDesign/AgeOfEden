using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [Header("Variables")]
    public float fadeSpeed;
    public float fadeOutTimer;

    private FadeOut m_foFadeOut;
    private SpriteRenderer m_srSpriteRenderer;
    private RawImage m_riRawImage;
    private MeshRenderer m_mrMeshRenderer;

	// Use this for initialization
	private void Start()
    {
        if (GetComponent<FadeOut>() != null)
        {
            m_foFadeOut = GetComponent<FadeOut>();
            m_foFadeOut.enabled = false;
        }

        if (GetComponent<SpriteRenderer>() != null)
        {
            m_srSpriteRenderer = GetComponent<SpriteRenderer>();
            m_srSpriteRenderer.color = Color.clear;
        }

        if (GetComponent<RawImage>() != null)
        {
            m_riRawImage = GetComponent<RawImage>();
            m_riRawImage.color = Color.clear;
        }

        if (GetComponent<MeshRenderer>() != null)
            m_mrMeshRenderer = GetComponent<MeshRenderer>();

        if (fadeOutTimer != 0)
            Invoke("FadeOutActivate", fadeOutTimer);
    }
	
	// Update is called once per frame
	private void Update()
    {
        if (m_srSpriteRenderer != null)
            m_srSpriteRenderer.color = Color.Lerp(
                m_srSpriteRenderer.color, Color.white, fadeSpeed * Time.deltaTime);

        if (m_riRawImage != null)
            m_riRawImage.color = Color.Lerp(
                m_riRawImage.color, Color.white, fadeSpeed * Time.deltaTime);

		if (m_mrMeshRenderer != null)
            m_mrMeshRenderer.material.color = Color.Lerp(
                m_mrMeshRenderer.material.color, Color.white, fadeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Activates fade out
    /// </summary>
    private void FadeOutActivate()
    {
        if (m_foFadeOut)
            m_foFadeOut.enabled = true;

        enabled = false;
    }
}