using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Variables")]
    public float forwardSpeed;
    public float strafeSpeed;
    public float backwardSpeed;
    public float sprintMultiplier;

    [Header("GameObjects")]
    public GameObject playerModel;

	[HideInInspector]
	public float verSpeed;
	[HideInInspector]
	public float horSpeed;
	[HideInInspector]
    public float sprint;

    public static bool canMove;

    public static GameObject player;

	// Use this for initialization
	void Start () {
        canMove = true;
        player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove == true)
        {
            playerModel.GetComponent<Animator>().SetBool("Standing", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerModel.GetComponent<Animator>().SetBool("Sprinting", true);
                sprint = Mathf.Lerp(sprint, sprintMultiplier, Time.deltaTime / 0.2f);
            }
            else
            {
                playerModel.GetComponent<Animator>().SetBool("Sprinting", false);
                sprint = Mathf.Lerp(sprint, 1, Time.deltaTime / 0.1f);
            }

            if (Input.GetAxis("Vertical") > 0)
                verSpeed = forwardSpeed * sprint;
            if (Input.GetAxis("Vertical") < 0)
                verSpeed = -backwardSpeed * sprint;
            if (Input.GetAxis("Vertical") == 0)
                verSpeed = 0;
            horSpeed = Input.GetAxis("Horizontal") * strafeSpeed * sprint;

            if (new Vector3 (verSpeed, 0, horSpeed) == Vector3.zero)
                playerModel.GetComponent<Animator>().SetBool("Moving", false);
            else
                playerModel.GetComponent<Animator>().SetBool("Moving", true);

            Vector3 desiredPosition = Vector3.ClampMagnitude(new Vector3(verSpeed, 0, horSpeed), forwardSpeed * Time.deltaTime * sprint);
            transform.position += transform.forward * desiredPosition.x;
            transform.position += transform.right * desiredPosition.z;

            /*
            RaycastHit hit;
            Debug.DrawRay(transform.position + new Vector3 (0, -0.9f, 0), new Vector3(0, -0.2f, 0), Color.red);

            if (Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0), Vector3.down, out hit, 0.2f) && Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
            }
            */
        }
    }
}
