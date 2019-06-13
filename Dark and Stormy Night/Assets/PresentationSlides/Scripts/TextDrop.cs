using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDrop : MonoBehaviour {

    public enum styles { drop, twist, fade}
    public styles transistionStyle;
    public bool onOut = true;
    public bool down = true;
    public Vector2 timeForFallBounds;

    public string changeTo;
    public float tarAlphaValue;

    public int tarSlide;
    private float timeForFall;
    private float timer;
    private float speed;


    private float startYValue;


    private Quaternion faceRotation;
    private Quaternion startRotation;

    // Use this for initialization
    void Start()
    {
        if (tarAlphaValue == 0)
            tarAlphaValue = 1;
        if (timeForFallBounds != Vector2.zero)
        {
            timeForFall = Random.Range(timeForFallBounds.x, timeForFallBounds.y);
        }

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
                GetComponent<Renderer>().enabled = false;
            }
        }

        if (transistionStyle == styles.fade)
        {
            if (onOut == false)
            {
                GetComponent<Renderer>().material.color = Color.clear;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PresentationManager.currentSlide >= tarSlide)
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
                        GetComponent<Renderer>().enabled = true;
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




                if (transistionStyle == styles.fade)
                {
                    speed = 1;
                    if (onOut == true)
                    {
                        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.clear, Time.deltaTime * speed);
                    }
                    else
                    {
                        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, new Color(1, 1, 1, tarAlphaValue), Time.deltaTime * speed);
                    }
                }

            }
            timer += Time.deltaTime;
        }
    }
}
