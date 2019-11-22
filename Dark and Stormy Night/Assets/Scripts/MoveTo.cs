using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveTo : MonoBehaviour
{
    public float metresPerSecond;
    public bool scaleSpeed;
    public float lerpSpeed;
    private float lerpSpeedActual = 0;

    public Transform Movee;

    public bool movePlayer;
    public bool moveCamera = false;

    public Vector3 tarPos;
    public Transform TarTransPos;
    
    public bool localPos;
    public Transform localTo;

    public bool relativePos;

    public bool lerp = false;
    public bool moveOnStart = true;
    public bool jumpOnFinish = true;

    public bool disableAfterFinish = false;
    public GameObject activateOnFinish;
    public GameObject deActivateOnFinish;
    public UnityEvent eventOnFinish = new UnityEvent();

	// Called once before the first frame
	private void Start()
    {
        if (TarTransPos)
            tarPos = TarTransPos.position;

        if (!Movee)
        {
            if (movePlayer)
                Movee = Movement.s_goPlayerObject.transform;
            else if (moveCamera)
                Movee = CameraMovement.s_CameraObject.transform;
            else
                Movee = transform;
        }

        if (localPos && !localTo)
            localTo = Movee;

        if (relativePos)
        {
            if (localPos)
                tarPos += localTo.localPosition;
            else
                tarPos += Movee.position;
        }
        if (scaleSpeed == true)
            metresPerSecond *= Movee.lossyScale.x;
	}
	
    // Called once per frame
    private void Update()
    {
        if (TarTransPos)
            tarPos = TarTransPos.position;

        if (moveOnStart && !lerp)
            MoveByMetre();

        if (moveOnStart && lerp)
            MoveByLerp();
    }

    /// <summary>
    /// Moves the object at a constant speed
    /// </summary>
    private void MoveByMetre()
    {
        float speed = metresPerSecond * Time.deltaTime;

        if (localPos)
        {
            Vector3 finalPos = localTo.parent.position + tarPos;

            if (Vector3.Distance(Movee.position, finalPos) < speed * Movee.lossyScale.x)
            {
                Movee.position = finalPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Vector3 direction = finalPos - Movee.position;
                direction = Vector3.ClampMagnitude(direction, 1);

                Movee.position += direction * speed;
            }
        }
        else
        {
            if (Vector3.Distance(Movee.position, tarPos) < speed * Movee.lossyScale.x)
            {
                Movee.position = tarPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Vector3 direction = tarPos - Movee.position;
                direction *= 1000;
                direction = Vector3.ClampMagnitude(direction, 1);

                Movee.position += direction * speed;
            }
        }
	}

    /// <summary>
    /// Moves the object with the use of linear interpolation
    /// </summary>
    private void MoveByLerp()
    {
        lerpSpeedActual = Mathf.Lerp(lerpSpeedActual, lerpSpeed, Time.deltaTime);

        if (localPos)
        {
            if (Vector3.Distance(Movee.position, Movee.parent.position + tarPos) < 0.5f / transform.lossyScale.x)
            {
                if (jumpOnFinish)
                    Movee.localPosition = tarPos;
                else
                    Movee.localPosition = Vector3.Lerp(
                        Movee.localPosition, tarPos, lerpSpeedActual * Time.deltaTime);
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Movee.localPosition = Vector3.Lerp(
                    Movee.localPosition, tarPos, lerpSpeedActual * Time.deltaTime);
            }
        }
        else
        {
            if (Vector3.Distance(Movee.position, tarPos) < 0.5f / transform.lossyScale.x)
            {
                Movee.position = tarPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Movee.position = Vector3.Lerp(
                    Movee.position, tarPos, lerpSpeedActual * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Called when an action is completed
    /// </summary>
    private void Finish()
    {
        moveOnStart = false;

        if (activateOnFinish)
            activateOnFinish.SetActive(true);
        if (deActivateOnFinish)
            deActivateOnFinish.SetActive(false);
        eventOnFinish.Invoke();
    }

    public void ChangeTransPos(Transform pTransform)
    {
        TarTransPos = pTransform;
    }

    /// <summary>
    /// Changes the target position
    /// </summary>
    /// <param name="pTarget">The desired target position</param>
    public void ChangeTarPos(Vector3 pTarget)
    {
        tarPos = pTarget;
        TarTransPos = null;
    }
}