using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVisibleEnable : MonoBehaviour {
    public List<GameObject> GOOnView = new List<GameObject>();
    public List<GameObject> GODisableOnView = new List<GameObject>();

    [Space(10)]

    public List<GameObject> GOOnNotView = new List<GameObject>();
    public List<GameObject> GODisableOnNotView = new List<GameObject>();

    private int visible = -1;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CameraMovement.s_CameraObject.transform.GetChild(0).GetComponent<Camera>());

        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds))
        {
            EnableObjects();
        }
        else
        {
            DisableObjects();
        }
    }

    private void EnableObjects ()
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

    private void DisableObjects ()
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
