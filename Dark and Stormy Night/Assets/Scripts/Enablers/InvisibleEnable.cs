using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVisibleEnable : MonoBehaviour
{
    public List<GameObject> GOOnView = new List<GameObject>();
    public List<GameObject> GODisableOnView = new List<GameObject>();

    [Space(10)]

    public List<GameObject> GOOnNotView = new List<GameObject>();
    public List<GameObject> GODisableOnNotView = new List<GameObject>();

    private int visible = -1;

    // Memory storage for the camera frustum
    private Plane[] m_pPlanes = new Plane[6];
    // The outer bounds for the render plane
    private Bounds m_bBounds;
    // Local reference to the players camera
    private Camera m_cPlayerCamera;

    // Use this for initialization
    private void Start()
    {
        m_bBounds = GetComponent<Renderer>().bounds;
        m_cPlayerCamera = CameraMovement.s_CameraObject.transform.GetChild(0)
            .GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_pPlanes = GeometryUtility.CalculateFrustumPlanes(m_cPlayerCamera);

        if (GeometryUtility.TestPlanesAABB(m_pPlanes, m_bBounds))
            EnableObjects();
        else
            DisableObjects();
    }

    private void EnableObjects()
    {
        if (visible != 0)
        {
            for (int i = 0; i < GOOnView.Count; i++)
            {
                GOOnView[i].SetActive(true);
            }
            for (int i = 0; i < GODisableOnView.Count; i++)
            {
                GODisableOnView[i].SetActive(false);
            }
        }
        visible = 0;
    }

    private void DisableObjects()
    {
        if (visible != 1)
        {
            for (int i = 0; i < GOOnNotView.Count; i++)
            {
                GOOnNotView[i].SetActive(true);
            }
            for (int i = 0; i < GODisableOnNotView.Count; i++)
            {
                GODisableOnNotView[i].SetActive(false);
            }
        }
        visible = 1;
    }
}