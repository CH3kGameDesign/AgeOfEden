using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlammer : MonoBehaviour
{
    public Vector3 tarEuler;
    private Quaternion tarRot;
    public Vector3 releaseForce;
    public float closeSpeed;

    private int counter;
    private bool move;
    private bool closed;

    private Rigidbody m_rbChildRb;

    private Transform m_tTransCache;
    private Transform m_tCameraTransform;

    // Use this for initialization
    private void Start()
    {
        tarRot = Quaternion.Euler(tarEuler);
        m_rbChildRb = transform.GetChild(0).GetComponent<Rigidbody>();
        m_tTransCache = transform;
        m_tCameraTransform = CameraMovement.s_CameraObject.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        counter++;
        if (counter > 5)
        {
            float dist2 = Vector3.Distance(m_tTransCache.position, m_tCameraTransform.position);

            if (dist2 > 4)
            {
                //m_rbChildRb.isKinematic = true;
                if (Mathf.Abs(Quaternion.Angle(tarRot, m_tTransCache.localRotation)) > 0.5f)
                    move = true;
                closed = true;
            }
            else if (closed)
            {
                closed = false;
                //m_rbChildRb.isKinematic = false;
                m_rbChildRb.AddForce(
                    releaseForce, ForceMode.Impulse);
            }
            counter = 0;
        }

        if (move)
        {
            m_rbChildRb.AddForce(tarEuler, ForceMode.Impulse);

            float tarMove = Quaternion.Angle(m_tTransCache.localRotation, tarRot);
            if (Mathf.Abs(tarMove) < closeSpeed)
            {
                m_tTransCache.localRotation = tarRot;
                move = false;
            }
            else
            {
                m_tTransCache.localRotation = Quaternion.RotateTowards(
                    m_tTransCache.localRotation, tarRot, closeSpeed);
            }
        }
    }
}