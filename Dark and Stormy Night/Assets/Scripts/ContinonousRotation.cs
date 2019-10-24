using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinonousRotation : MonoBehaviour
{
    public Vector3 rotationDirection;

    private Transform m_tTransCache;

	private void Start()
    {
        if (rotationDirection == Vector3.zero)
            rotationDirection = new Vector3(Random.Range(0, 360),
                Random.Range(0, 360), Random.Range(0, 360));

        m_tTransCache = transform;
	}
	
	// Update is called once per frame
	private void Update()
    {
        m_tTransCache.localEulerAngles += rotationDirection * Time.deltaTime;
	}
}