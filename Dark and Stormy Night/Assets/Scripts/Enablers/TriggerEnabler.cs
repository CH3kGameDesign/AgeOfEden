using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TriggerEnabler : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("GO")]
    private List<GameObject> m_LgoEnableObjects = new List<GameObject>();
    [SerializeField, FormerlySerializedAs("GODisable")]
    private List<GameObject> m_LgoDisableObjects = new List<GameObject>();

    [Space (10)]

    [SerializeField, FormerlySerializedAs("Collider")]
    private List<GameObject> m_LgoColliderEnable = new List<GameObject>();
    [SerializeField, FormerlySerializedAs("ColliderDisable")]
    private List<GameObject> m_LgoColliderDisable = new List<GameObject>();

    [Space(10)]

    [SerializeField, FormerlySerializedAs("Gravity")]
    private List<Rigidbody> m_LrbPhysicsEnable = new List<Rigidbody>();
    [SerializeField, FormerlySerializedAs("GravityDisable")]
    private List<Rigidbody> m_LrbPhysicsDisable = new List<Rigidbody>();

    [Space(10)]

    [SerializeField, FormerlySerializedAs("SoftRotation")]
    private List<SoftRotation> m_LsrSoftRotationEnable = new List<SoftRotation>();
    [SerializeField, FormerlySerializedAs("SoftRotationDisable")]
    private List<SoftRotation> m_LsrSoftRotationDisable = new List<SoftRotation>();

    [Space(10)]

    [SerializeField, FormerlySerializedAs("FadeOut")]
    private List<FadeOut> m_LfoFadeOutList = new List<FadeOut>();

	[Space(10)]

    [SerializeField, FormerlySerializedAs("Timer")]
    private float m_fDelay;

    [SerializeField, FormerlySerializedAs("activateEvent")]
    private UnityEvent m_ueActivateEvent;

    private enum CollisionType { OnEnter, OnExit };
    [SerializeField, FormerlySerializedAs("collision")]
    private CollisionType m_ctCollision;

	// Use this for initialization
	private void Start()
    {
        if (m_ueActivateEvent == null)
            m_ueActivateEvent = new UnityEvent();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_ctCollision == CollisionType.OnEnter)
        {
            Invoke("EnableObjects", m_fDelay);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && m_ctCollision == CollisionType.OnExit)
        {
            Invoke("EnableObjects", m_fDelay);
        }
    }

    private void EnableObjects()
    {
        m_ueActivateEvent.Invoke();
        for (int i = 0; i < m_LgoEnableObjects.Count; i++)
        {
            m_LgoEnableObjects[i].SetActive(true);
        }
        for (int i = 0; i < m_LgoDisableObjects.Count; i++)
        {
            m_LgoDisableObjects[i].SetActive(false);
        }

        for (int i = 0; i < m_LgoColliderEnable.Count; i++)
        {
            for (int j = 0; j < m_LgoColliderEnable[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                m_LgoColliderEnable[i].GetComponentsInChildren<Collider>()[j].enabled = true;
            }
            
        }
        for (int i = 0; i < m_LgoColliderDisable.Count; i++)
        {
            for (int j = 0; j < m_LgoColliderDisable[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                m_LgoColliderDisable[i].GetComponentsInChildren<Collider>()[j].enabled = false;
            }
        }

        for (int i = 0; i < m_LrbPhysicsEnable.Count; i++)
        {
            m_LrbPhysicsEnable[i].useGravity = true;
            m_LrbPhysicsEnable[i].isKinematic = false;
        }
        for (int i = 0; i < m_LrbPhysicsDisable.Count; i++)
        {
            m_LrbPhysicsEnable[i].useGravity = false;
            m_LrbPhysicsEnable[i].isKinematic = true;
        }

        for (int i = 0; i < m_LsrSoftRotationEnable.Count; i++)
        {
            m_LsrSoftRotationEnable[i].enabled = true;
        }
        for (int i = 0; i < m_LsrSoftRotationDisable.Count; i++)
        {
            m_LsrSoftRotationDisable[i].enabled = false;
        }

		for (int i = 0; i < m_LfoFadeOutList.Count; i++)
		{
			m_LfoFadeOutList[i].enabled = true;
		}
    }
}