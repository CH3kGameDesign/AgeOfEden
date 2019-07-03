using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisibilityEnabler : MonoBehaviour
{
    public List<GameObject> GOOnView = new List<GameObject>();
    public List<GameObject> GODisableOnView = new List<GameObject>();

    [Space(10)]

    public List<GameObject> GOOnNotView = new List<GameObject>();
    public List<GameObject> GODisableOnNotView = new List<GameObject>();

    public UnityEvent activateEvent;
    public UnityEvent deactivateEvent;

    private int visible = -1;

    // Use this for initialization
    void Start()
    {
        if (activateEvent == null)
            activateEvent = new UnityEvent();
        if (deactivateEvent == null)
            deactivateEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CameraMovement.cameraObject.transform.GetChild(0).GetComponent<Camera>());

        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds))
        {
            EnableObjects();
        }
        else
        {
            DisableObjects();
        }
    }

    private void EnableObjects()
    {
        if (visible != 0)
        {
            activateEvent.Invoke();
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
            deactivateEvent.Invoke();
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