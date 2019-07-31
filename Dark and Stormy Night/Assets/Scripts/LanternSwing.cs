using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
public class LanternSwing : MonoBehaviour
{
    [Header("Rotations")]
    public float m_fRotation_Amplitude_Pitch1 = 1;
    public float m_fRotation_Frequency_Pitch1 = 5;
    [Space]
    public float m_fRotation_Amplitude_Roll1 = 1;
    public float m_fRotation_Frequency_Roll1 = 1;

    private float m_fTimeStep = 0;

    private Rigidbody m_rbRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_fTimeStep = Random.Range(0, 50);
        m_rbRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fTimeStep += Time.deltaTime;

        // Applies a sine wave of force to the object
        m_rbRigidbody.AddForce(new Vector3(
            /*Roll*/
            m_fRotation_Amplitude_Roll1 * Mathf.Sin(
                m_fRotation_Frequency_Roll1 * m_fTimeStep),
            /*Yaw*/
            0,
            /*Pitch*/
            m_fRotation_Amplitude_Pitch1 * Mathf.Sin(
                m_fRotation_Frequency_Pitch1 * m_fTimeStep)));
    }
}