using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    public float forwardSpeed;
    public float strafeSpeed;
    public float backwardSpeed;
    public float sprintMultiplier;

    private float timeBetweenSteps;
    private float stepTimer;
    [HideInInspector]
    public float speedMultiplier = 1;

    [Header("GameObjects")]
    public GameObject playerModel;
    //public FootStepManager footStepManager;

	[HideInInspector]
	public float verSpeed;
	[HideInInspector]
	public float horSpeed;
	[HideInInspector]
    public float sprint;

    public bool canMoveOnStart = true;

    public static bool canMove;

    public static GameObject player;

	// Called once before the first frame
	private void Start ()
    {
        canMove = canMoveOnStart;
        player = gameObject;
        timeBetweenSteps = forwardSpeed / 4;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (canMove)
        {
            // Starts the player as standing
			playerModel.GetComponent<Animator> ().SetBool ("Standing", true);

			if (Input.GetKey (KeyCode.LeftShift))
            {
				playerModel.GetComponent<Animator> ().SetBool ("Sprinting", true);
				sprint = Mathf.Lerp (sprint, sprintMultiplier, Time.deltaTime / 0.2f);
			}
            else
            {
				playerModel.GetComponent<Animator> ().SetBool ("Sprinting", false);
				sprint = Mathf.Lerp (sprint, 1, Time.deltaTime / 0.1f);
			}

			if (Input.GetAxis ("Vertical") > 0)
				verSpeed = forwardSpeed * sprint;

			if (Input.GetAxis ("Vertical") < 0)
				verSpeed = -backwardSpeed * sprint;

			if (Input.GetAxis ("Vertical") == 0)
				verSpeed = 0;

			horSpeed = Input.GetAxis ("Horizontal") * strafeSpeed * sprint;

            if (new Vector3(verSpeed, 0, horSpeed) == Vector3.zero)
            {
                playerModel.GetComponent<Animator>().SetBool("Moving", false);
                if (stepTimer != 0)
                {
                    //footStepManager.MakeSound();
                    stepTimer = 0;
                }
            }
            else
            {
                playerModel.GetComponent<Animator>().SetBool("Moving", true);
                stepTimer += Time.deltaTime;
            }

			Vector3 desiredPosition = Vector3.ClampMagnitude (new Vector3 (verSpeed, 0, horSpeed), forwardSpeed * Time.deltaTime * sprint);
			transform.position += transform.forward * desiredPosition.x * speedMultiplier;
			transform.position += transform.right * desiredPosition.z * speedMultiplier;


            if (stepTimer > timeBetweenSteps / sprint)
            {
                //footStepManager.MakeSound();
                stepTimer = 0;
            }
			/*
            RaycastHit hit;
            Debug.DrawRay(transform.position + new Vector3 (0, -0.9f, 0), new Vector3(0, -0.2f, 0), Color.red);

            if (Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0), Vector3.down, out hit, 0.2f) && Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
            }
            */
		}
        else
        {
			playerModel.GetComponent<Animator>().SetBool("Sprinting", false);
			playerModel.GetComponent<Animator>().SetBool("Moving", false);
            stepTimer = 0;
        }
    }
}
