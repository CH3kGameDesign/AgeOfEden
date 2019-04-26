using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraMovement : MonoBehaviour {

    [Header("CameraSpeed")]
	public float camRunShakeSpeed;

    [Header("CameraLimits")]
	public float camRunShakeMax;

    [Header("Transforms")]
    public Transform cameraHook;
    public Transform aimPoint;
    public Transform reticle;

    [Header("GameObjects")]
    public List<GameObject> enableObjects;

	private float camRunShakeAmount;
	private float camRunShake;

    public static bool snapToPlayer;
    public static bool goToPlayer;
    public static bool canMove = true;

    public static GameObject cameraObject;


	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        snapToPlayer = true;
        goToPlayer = true;
        cameraObject = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (snapToPlayer == true)
            transform.position = cameraHook.position;
           
		RunShake ();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            aimPoint.position = hit.point;
        else
            aimPoint.position = transform.position + (transform.forward * 100);

        transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, Vector3.zero, 0.3f);

        if (goToPlayer == true)
        {
            transform.position = Vector3.Lerp(transform.position, cameraHook.position, 1.5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, cameraHook.position) < 0.3f)
            {
                snapToPlayer = true;
                Movement.canMove = true;
                goToPlayer = false;
                for (int i = 0; i < enableObjects.Count; i++)
                {
                    enableObjects[i].SetActive(true);
                }
            }
        }
    }

	void RunShake()
	{
		float horSpeed = PlayerModel.player.GetComponent<Movement> ().horSpeed;
		float verSpeed = PlayerModel.player.GetComponent<Movement> ().verSpeed;
		float sprint = PlayerModel.player.GetComponent<Movement> ().sprint;

		if (horSpeed < 0)
			horSpeed = -horSpeed;
		if (verSpeed < 0)
			verSpeed = -verSpeed;

		if (horSpeed > verSpeed)
			camRunShakeAmount = horSpeed;
		else
			camRunShakeAmount = verSpeed;
		
		if (sprint > 1)
			sprint = (sprint + 0.4f) / 2;
		
		float cameraRunBound = camRunShakeMax * sprint;

		if (camRunShake > cameraRunBound && camRunShakeSpeed > 0)
			camRunShakeSpeed = -camRunShakeSpeed;
		if (camRunShake < -cameraRunBound && camRunShakeSpeed < 0)
			camRunShakeSpeed = -camRunShakeSpeed;
		if (camRunShakeAmount != 0)
			camRunShake += camRunShakeAmount * camRunShakeSpeed;
		else
			camRunShake = 0;
	}
}
