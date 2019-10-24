using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public bool global = true;

    public Vector3 rotationLock;

    private Vector3 startRotation;

    private Transform m_tTransCache;

    // Use this for initialization
    private void Start()
    {
        m_tTransCache = transform;

        if (global)
            startRotation = m_tTransCache.eulerAngles;
        else
            startRotation = m_tTransCache.localEulerAngles;

        Debug.Log(startRotation);
    }
	
	// Update is called once per frame
	private void Update()
    {
		if (global)
        {
            if (rotationLock.x != 0)
                m_tTransCache.eulerAngles = new Vector3(
                    startRotation.x, m_tTransCache.eulerAngles.y, m_tTransCache.eulerAngles.z);
            if (rotationLock.y != 0)
                m_tTransCache.eulerAngles = new Vector3(
                    m_tTransCache.eulerAngles.x, startRotation.y, m_tTransCache.eulerAngles.z);
            if (rotationLock.z != 0)
                m_tTransCache.eulerAngles = new Vector3(
                    m_tTransCache.eulerAngles.x, m_tTransCache.eulerAngles.y, startRotation.z);
        }
        else
        {
            if (rotationLock.x != 0)
                m_tTransCache.localEulerAngles = new Vector3(
                    startRotation.x, m_tTransCache.localEulerAngles.y, m_tTransCache.localEulerAngles.z);
            if (rotationLock.y != 0)
                m_tTransCache.localEulerAngles = new Vector3(
                    m_tTransCache.localEulerAngles.x, startRotation.y, m_tTransCache.localEulerAngles.z);
            if (rotationLock.z != 0)
                m_tTransCache.localEulerAngles = new Vector3(
                    m_tTransCache.localEulerAngles.x, m_tTransCache.localEulerAngles.y, startRotation.z);
        }
	}
}