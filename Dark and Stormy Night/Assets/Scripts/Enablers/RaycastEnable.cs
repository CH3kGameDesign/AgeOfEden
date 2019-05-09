using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEnable : MonoBehaviour {

    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();

    [Space(10)]

    public List<GameObject> Collider = new List<GameObject>();
    public List<GameObject> ColliderDisable = new List<GameObject>();

    public float Timer;

    public Transform CameraLookAt;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane)), out hit, 100))
            {
            if (hit.transform.gameObject == this.gameObject)
                Invoke ("EnableObjects", Timer);
        }
	}

    private void EnableObjects()
    {
        if (CameraLookAt != null)
        {
            CameraMovement.cameraObject.GetComponent<SmoothCameraMovement>().enabled = false;
            Vector3 relPos = CameraLookAt.position - CameraMovement.cameraObject.transform.position;
            Quaternion tarRot = Quaternion.LookRotation(relPos, Vector3.up);
            CameraMovement.cameraObject.transform.rotation = Quaternion.Slerp(CameraMovement.cameraObject.transform.rotation, tarRot, Time.deltaTime * 0.5f);

            Movement.canMove = false;
        }
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(true);
        }
        for (int i = 0; i < GODisable.Count; i++)
        {
            GODisable[i].SetActive(false);
        }

        for (int i = 0; i < Collider.Count; i++)
        {
            for (int j = 0; j < Collider[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                Collider[i].GetComponentsInChildren<Collider>()[j].enabled = true;
            }

        }
        for (int i = 0; i < ColliderDisable.Count; i++)
        {
            for (int j = 0; j < ColliderDisable[i].GetComponentsInChildren<Collider>().Length; j++)
            {
                ColliderDisable[i].GetComponentsInChildren<Collider>()[j].enabled = false;
            }
        }
    }
}
