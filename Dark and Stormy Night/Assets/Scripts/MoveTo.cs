using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {

    public float metresPerSecond;
    public float lerpSpeed;

    public Transform Movee;

    public Vector3 tarPos;

    
    public bool localPos;
    public bool relativePos;

    public bool lerp = false;
    public bool moveOnStart = true;

    public bool disableAfterFinish = false;
    public GameObject activateOnFinish;

	// Use this for initialization
	void Start () {
        if (Movee == null)
            Movee = this.transform;
        if (relativePos == true)
        {
            if (localPos == true)
                tarPos += Movee.localPosition;
            else
                tarPos += Movee.position;
        }
	}
	
    void Update ()
    {
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
            if (Vector3.Distance(Movee.position, Movee.parent.position + tarPos) < speed)
            {
                Movee.localPosition = tarPos;
                if (disableAfterFinish)
                    Finish();
            }
            else
            {
                Vector3 direction = tarPos - Movee.localPosition;
                direction = Vector3.ClampMagnitude(direction, 1);

                Movee.localPosition += direction * speed;
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
                Movee.localPosition = Vector3.Lerp(Movee.localPosition, tarPos, lerpSpeed * Time.deltaTime);
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
                Movee.position = Vector3.Lerp(Movee.position, tarPos, lerpSpeed * Time.deltaTime);
            }
        }
    }

    void Finish ()
    {
        moveOnStart = false;
        activateOnFinish.SetActive(true);
    }
}
