using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {

    public float metresPerSecond;
    public float lerpSpeed;
    private float lerpSpeedActual = 0;

    public Transform Movee;

    public bool movePlayer;

    public Vector3 tarPos;
    public Transform TarTransPos;

    
    public bool localPos;
    public Transform localTo;

    public bool relativePos;

    public bool lerp = false;
    public bool moveOnStart = true;

    public bool disableAfterFinish = false;
    public GameObject activateOnFinish;

	// Use this for initialization
	void Start () {
        if (TarTransPos != null)
            tarPos = TarTransPos.position;
        if (Movee == null)
        {
            if (movePlayer == true)
                Movee = Movement.player.transform;
            else
                Movee = this.transform;
        }
        if (localPos == true && localTo == null)
            localTo = Movee;
        if (relativePos == true)
        {
            if (localPos == true)
                tarPos += localTo.localPosition;
            else
                tarPos += Movee.position;
        }
	}
	
    void Update ()
    {
        if (TarTransPos != null)
            tarPos = TarTransPos.position;
        if (moveOnStart == true && lerp == false)
            MoveByMetre();
        if (moveOnStart == true && lerp == true)
            MoveByLerp();
    }

	// Update is called once per frame
	void MoveByMetre () {
        float speed = metresPerSecond * Time.deltaTime;

        if (localPos == true)
        {
            Vector3 finalPos = localTo.parent.position + tarPos;
            if (Vector3.Distance(Movee.position, finalPos) < speed)
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

            if (Vector3.Distance(Movee.position, tarPos) < speed)
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

    void MoveByLerp ()
    {
        lerpSpeedActual = Mathf.Lerp(lerpSpeedActual, lerpSpeed, Time.deltaTime);
        if (localPos == true)
        {
            if (Vector3.Distance(Movee.position, Movee.parent.position + tarPos) < 0.5f)
            {
                Movee.localPosition = tarPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Movee.localPosition = Vector3.Lerp(Movee.localPosition, tarPos, lerpSpeedActual * Time.deltaTime);
            }
        }
        else
        {

            if (Vector3.Distance(Movee.position, tarPos) < 0.5f)
            {
                Movee.position = tarPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Movee.position = Vector3.Lerp(Movee.position, tarPos, lerpSpeedActual * Time.deltaTime);
            }
        }
    }

    void Finish ()
    {
        moveOnStart = false;
        if (activateOnFinish != null)
        activateOnFinish.SetActive(true);
    }

    public void ChangeTransPos(Transform trans)
    {
        TarTransPos = trans;
    }
    public void ChangeTarPos(Vector3 tar)
    {
        tarPos = tar;
        TarTransPos = null;
    }
}
