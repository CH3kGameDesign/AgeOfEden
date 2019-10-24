using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingObject : MonoBehaviour
{
    public float breathSpeed;
    public Vector3 breathMax;

    private bool inflate = true;

    private Transform m_tTransCache;

    private void Start()
    {
        m_tTransCache = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (inflate)
            m_tTransCache.localScale = Vector3.Lerp(m_tTransCache.localScale,
                breathMax, breathSpeed * Time.deltaTime);
        else
            m_tTransCache.localScale = Vector3.Lerp(m_tTransCache.localScale,
                Vector3.one, breathSpeed * Time.deltaTime);

        if (Vector3.Distance(m_tTransCache.localScale, breathMax) <= 0.01f)
            inflate = false;

        if (Vector3.Distance(m_tTransCache.localScale, Vector3.one) <= 0.01f)
            inflate = true;
    }
}