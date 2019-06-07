using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDrop : MonoBehaviour {

    public enum styles { drop, twist}
    public styles transistionStyle;
    public bool onOut = true;
    public bool down = true;

    public string changeTo;

    public int tarSlide;
    private float timeForFall;
    private float timer;
    private float speed;


    private float startYValue;


    private Quaternion faceRotation;
    private Quaternion startRotation;

	// Use this for initialization
	void Start () {
        timeForFall = Random.Range(0f, 1f);
        timer = 0;
        speed = 0;
        startYValue = transform.position.y;

        
        Vector3 dif = Camera.main.transform.position - transform.position;
        faceRotation = Quaternion.LookRotation(dif, Vector3.up);
        faceRotation *= Quaternion.Euler(0, 90, 0);
        startRotation = transform.rotation;


        if (transistionStyle == styles.drop)
        {
            if (onOut == false && down == true)
                transform.position += new Vector3(0, 10, 0);
            if (onOut == false && down == false)
                transform.position -= new Vector3(0, 10, 0);
        }


        if (transistionStyle == styles.twist)
        {
            if (onOut == false)
            {
                transform.rotation = faceRotation;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PresentationManager.currentSlide == tarSlide)
        {
            if (timer >= timeForFall)
            {





                if (transistionStyle == styles.drop)
                {
                    if (onOut == true)
                    {
                        speed = Mathf.Lerp(speed, 15 * Time.deltaTime, Time.deltaTime);
                    }
                    else
                    {
                        if (down == true)
                            speed = (transform.position.y - startYValue) * Time.deltaTime;
                        if (down == false)
                            speed = (startYValue - transform.position.y) * Time.deltaTime;
                    }
                    if (down == true)
                        transform.localPosition += new Vector3(0, -speed, 0);
                    else
                        transform.localPosition += new Vector3(0, speed, 0);
                }







                if (transistionStyle == styles.twist)
                {
                    if (onOut == true)
                    {
                        speed = Mathf.Lerp(speed, 10, Time.deltaTime);
                        if (Quaternion.Angle(transform.rotation, faceRotation) <= 2)
                        {
                            transform.rotation = faceRotation;
                        }
                        transform.rotation = Quaternion.Lerp(transform.rotation, faceRotation, Time.deltaTime * speed);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime);
                    }
                    if (onOut == true && changeTo != null)
                    {
                        if (Quaternion.Angle(transform.rotation, faceRotation) <= 1)
                        {
                            GetComponent<TextMeshPro>().text = changeTo;
                            onOut = false;
                            startRotation *= Quaternion.Euler(0, 360, 0);
                        }
                    }
                }
            }
            timer += Time.deltaTime;
        }
    }
}
