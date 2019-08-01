using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBounce : MonoBehaviour
{
    [Header("Translations")]
    public float m_fTranslation_Amplitude_X1 = 0.1f;
    public float m_fTranslation_Frequency_X1 = 1;
    [Space]
    public float m_fTranslation_Amplitude_Y1 = 0.3f;
    public float m_fTranslation_Frequency_Y1 = 1;
    [Space]
    public float m_fTranslation_Amplitude_Y2 = 0.1f;
    public float m_fTranslation_Frequency_Y2 = 5;

    [Header("Rotations")]
    public float m_fRotation_Amplitude_Pitch1 = 0.6f;
    public float m_fRotation_Frequency_Pitch1 = 1;
    [Space]
    public float m_fRotation_Amplitude_Pitch2 = 0.3f;
    public float m_fRotation_Frequency_Pitch2 = 5;
    [Space]
    public float m_fRotation_Amplitude_Roll1 = 0.4f;
    public float m_fRotation_Frequency_Roll1 = 5;
    [Space]
    public float m_fRotation_Amplitude_Roll2 = 0.2f;
    public float m_fRotation_Frequency_Roll2 = 9;
    [Space]
    public float m_fRotation_Amplitude_Roll3 = 0.35f;
    public float m_fRotation_Frequency_Roll3 = 12;
    [Space]
    public float m_fRotation_Amplitude_Roll4 = 0.35f;
    public float m_fRotation_Frequency_Roll4 = 15;

    private float m_fTimeStep = 0;

    private Vector3 m_v3DefaultPos;
    private Quaternion m_qDefaultRot;

    // Start is called before the first frame update
    private void Start ()
    {
        m_v3DefaultPos = transform.position;
        m_qDefaultRot = transform.rotation;
    }

    // Update is called once per frame
    private void Update ()
    {
        m_fTimeStep += Time.deltaTime;

        transform.position = m_v3DefaultPos + new Vector3(
            /*Oscillates in the x direction*/
            m_fTranslation_Amplitude_X1 * Mathf.Sin(m_fTranslation_Frequency_X1 * m_fTimeStep),
            /*Oscillates in the y direction*/
            m_fTranslation_Amplitude_Y1 * Mathf.Sin(m_fTranslation_Frequency_Y1 * m_fTimeStep) +
            m_fTranslation_Amplitude_Y2 * Mathf.Sin(m_fTranslation_Frequency_Y2 * m_fTimeStep),
            0);

        transform.rotation = m_qDefaultRot *
            /*Oscillates foward and backward (pitch)*/
            Quaternion.AngleAxis(m_fRotation_Amplitude_Pitch1 *
            Mathf.Sin(m_fRotation_Frequency_Pitch1 * m_fTimeStep), Vector3.right) *
            Quaternion.AngleAxis(m_fRotation_Amplitude_Pitch2 *
            Mathf.Sin(m_fRotation_Frequency_Pitch2 * m_fTimeStep), Vector3.right) *
            /*Oscillates side to side (roll)*/
            Quaternion.AngleAxis(m_fRotation_Amplitude_Roll1 *
            Mathf.Sin(m_fRotation_Frequency_Roll1 * m_fTimeStep), Vector3.forward) *
            Quaternion.AngleAxis(m_fRotation_Amplitude_Roll2 *
            Mathf.Sin(m_fRotation_Frequency_Roll2 * m_fTimeStep), Vector3.forward) *
            Quaternion.AngleAxis(m_fRotation_Amplitude_Roll3 *
            Mathf.Sin(m_fRotation_Frequency_Roll3 * m_fTimeStep), Vector3.forward) *
            Quaternion.AngleAxis(m_fRotation_Amplitude_Roll4 *
            Mathf.Sin(m_fRotation_Frequency_Roll4 * m_fTimeStep), Vector3.forward);
    }
}