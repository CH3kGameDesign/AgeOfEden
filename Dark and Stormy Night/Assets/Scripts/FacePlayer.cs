using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [Space(10)]
    public Vector3 offset;

    [Space(10)]

    public bool faceCamera = true;
    public bool tilt = false;
    public float delay = 1;

    private Transform m_tTransCache;
    private Transform m_tCameraTrans;
    private Transform m_tPlayerTrans;

    private void Start()
    {
        m_tTransCache = transform;
        m_tCameraTrans = CameraMovement.s_CameraObject.transform;
        m_tPlayerTrans = Movement.s_goPlayerObject.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (faceCamera)
        {
            if (tilt)
            {
                m_tTransCache.LookAt(m_tCameraTrans.position + offset);
                if (InvertGravity.invertedGravity)
                    m_tTransCache.localEulerAngles = new Vector3(
                        m_tTransCache.localEulerAngles.x, m_tTransCache.localEulerAngles.y, 0);
            }
            else
            {
                Vector3 lookPos = m_tTransCache.position
                    - (m_tCameraTrans.position + offset);
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                m_tTransCache.rotation = Quaternion.Slerp(
                    m_tTransCache.rotation, rotation, Time.deltaTime * delay);
            }
        }
        else
        {
            if (tilt)
            {
                m_tTransCache.LookAt(m_tPlayerTrans.position + offset);
                if (InvertGravity.invertedGravity)
                    m_tTransCache.localEulerAngles = new Vector3(
                        m_tTransCache.localEulerAngles.x, m_tTransCache.localEulerAngles.y, 0);
            }
            else
            {
                Vector3 lookPos = m_tTransCache.position
                    - (m_tPlayerTrans.position + offset);
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                m_tTransCache.rotation = Quaternion.Slerp(
                    m_tTransCache.rotation, rotation, Time.deltaTime * delay);
            }
        }
	}
}