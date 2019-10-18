using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastEnable : MonoBehaviour
{
    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    [Space(10)]

    public List<GameObject> GOOnNotView = new List<GameObject>();
    public List<GameObject> GODisableOnNotView = new List<GameObject>();

    [Space(20)]

    public List<GameObject> Collider = new List<GameObject>();
    public List<GameObject> ColliderDisable = new List<GameObject>();

    public UnityEvent activateEvent;

    public float Timer;

    public bool throughObjects = false;
    public bool onClick = false;

    [SerializeField]
    private Transform CameraLookAt;
    [SerializeField]
    private Transform MoveToHitPoint;

    // Use this for initialization
    private void Start()
    {
        if (activateEvent == null)
            activateEvent = new UnityEvent();
	}
	
	// Update is called once per frame
	private void Update()
    {
        if (!onClick)
            RaycastyGoodness();
        else
        {
            if (Input.GetMouseButtonDown(0))
                RaycastyGoodness();
        }
	}

    private void RaycastyGoodness()
    {
        RaycastHit hit;
        if (!throughObjects)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(
                Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane)), out hit, 100))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    if (MoveToHitPoint)
                    {
                        MoveToHitPoint.gameObject.SetActive(true);
                        MoveToHitPoint.position = hit.point;
                    }
                    Invoke("EnableObjects", Timer);
                    activateEvent.Invoke();
                }
                else
                {
                    if (MoveToHitPoint)
                        MoveToHitPoint.gameObject.SetActive(false);
                    DisableObjects();
                }
            }
            else
            {
                if (MoveToHitPoint)
                    MoveToHitPoint.gameObject.SetActive(false);
                DisableObjects();
            }
        }
        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(
                Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane)),
                out hit, 100, 1 << gameObject.layer))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    if (MoveToHitPoint)
                    {
                        MoveToHitPoint.gameObject.SetActive(true);
                        MoveToHitPoint.position = hit.point;
                    }
                    Invoke("EnableObjects", Timer);
                    activateEvent.Invoke();
                }
                else
                {
                    if (MoveToHitPoint)
                        MoveToHitPoint.gameObject.SetActive(false);
                    DisableObjects();
                }
            }
            else
            {
                if (MoveToHitPoint)
                    MoveToHitPoint.gameObject.SetActive(false);
                DisableObjects();
            }
        }
    }

    private void EnableObjects()
    {
        // Potentially causing problems?
        if (CameraLookAt != null)
        {
            //Debug.Log("Raycast Enabled");
            CameraMovement.s_CanMove = false;
            Vector3 relPos = CameraLookAt.position
                - CameraMovement.s_CameraObject.transform.position;

            Quaternion tarRot = Quaternion.LookRotation(relPos, -Vector3.up);

            CameraMovement.s_CameraObject.transform.rotation = Quaternion.Slerp(
                CameraMovement.s_CameraObject.transform.rotation, tarRot, Time.deltaTime * 0.5f);

            Movement.s_bCanMove = false;
        }

        for (int i = 0; i < GO.Count; i++)
            GO[i].SetActive(true);

        for (int i = 0; i < GODisable.Count; i++)
            GODisable[i].SetActive(false);

        for (int i = 0; i < Collider.Count; i++)
        {
            for (int j = 0; j < Collider[i].GetComponentsInChildren<Collider>().Length; j++)
                Collider[i].GetComponentsInChildren<Collider>()[j].enabled = true;

        }
        for (int i = 0; i < ColliderDisable.Count; i++)
        {
            for (int j = 0; j < ColliderDisable[i].GetComponentsInChildren<Collider>().Length; j++)
                ColliderDisable[i].GetComponentsInChildren<Collider>()[j].enabled = false;
        }
    }

    private void DisableObjects()
    {
        for (int i = 0; i < GOOnNotView.Count; i++)
            GOOnNotView[i].SetActive(true);

        for (int i = 0; i < GODisableOnNotView.Count; i++)
            GODisableOnNotView[i].SetActive(false);
    }
}
