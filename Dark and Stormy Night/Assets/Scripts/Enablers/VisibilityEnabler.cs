using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class VisibilityEnabler : MonoBehaviour
{
    private bool visible = false;

    [SerializeField, Tooltip("Scales the distance at which the plane will no longer be active")]
    private float m_fFarClipDistance = 10.0f;

    [SerializeField, FormerlySerializedAs("GOOnView"),
        Tooltip("A list of objects that are enabled when the object comes into view")]
    private List<GameObject> m_LgoEnableOnView = new List<GameObject>();
    [SerializeField, FormerlySerializedAs("GODisableOnView"),
        Tooltip("A list of objects that are disabled when the object comes into view")]
    private List<GameObject> m_LgoDisableOnView = new List<GameObject>();

    [Space(10)]

    [SerializeField, FormerlySerializedAs("GOOnNotView"),
        Tooltip("A list of objects that are enabled when the object leaves the view")]
    private List<GameObject> m_LgoEnableOnHide = new List<GameObject>();
    [SerializeField, FormerlySerializedAs("GODisableOnNotView"),
        Tooltip("A list of objects that are disabled when the object leaves the view")]
    private List<GameObject> m_LgoDisableOnHide = new List<GameObject>();

    [SerializeField, FormerlySerializedAs("activateEvent")]
    private UnityEvent m_ueActivateEvent;
    [SerializeField, FormerlySerializedAs("deactivateEvent")]
    private UnityEvent m_ueDeactivateEvent;

    // Memory storage for the camera frustum
    private Plane[] m_pPlanes = new Plane[6];
    // The outer bounds for the render plane
    private Bounds m_bBounds;
    // Local reference to the players camera
    private Camera m_cPlayerCamera;

    // Use this for initialization
    private void Start()
    {
        if (m_ueActivateEvent == null)
            m_ueActivateEvent = new UnityEvent();
        if (m_ueDeactivateEvent == null)
            m_ueDeactivateEvent = new UnityEvent();

        m_bBounds = GetComponent<Renderer>().bounds;
        m_cPlayerCamera = CameraMovement.s_CameraObject.transform.GetChild(0)
            .GetComponent<Camera>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        m_pPlanes = GeometryUtility.CalculateFrustumPlanes(m_cPlayerCamera);
        m_pPlanes[5].Translate(m_pPlanes[5].normal * (m_cPlayerCamera.farClipPlane
            * m_fFarClipDistance));
        
        if (GeometryUtility.TestPlanesAABB(m_pPlanes, m_bBounds))
        {
            OnView();
        }
        else
        {
            OnHide();
        }
    }

    private void OnView()
    {
        if (!visible)
        {
            m_ueActivateEvent.Invoke();

            for (int i = 0; i < m_LgoEnableOnView.Count; i++)
                m_LgoEnableOnView[i].SetActive(true);

            for (int i = 0; i < m_LgoDisableOnView.Count; i++)
                m_LgoDisableOnView[i].SetActive(false);
        }
        visible = true;
    }

    private void OnHide()
    {
        if (visible)
        {
            m_ueDeactivateEvent.Invoke();

            for (int i = 0; i < m_LgoEnableOnHide.Count; i++)
                m_LgoEnableOnHide[i].SetActive(true);

            for (int i = 0; i < m_LgoDisableOnHide.Count; i++)
                m_LgoDisableOnHide[i].SetActive(false);
        }
        visible = false;
    }
}