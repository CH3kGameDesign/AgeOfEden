using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    public bool fade = false;
    public float fadeWait = 100000;

    public float fadeSpeed = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeWait <= 0)
            fade = true;
        if (fade == true)
        {
            if (GetComponent<TextMeshPro>() != null)
            {
                GetComponent<TextMeshPro>().color = Color.Lerp(GetComponent<TextMeshPro>().color, Color.clear, fadeSpeed * Time.deltaTime);


                if (GetComponent<TextMeshPro>().color.a == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (GetComponent<MeshRenderer>() != null)
                {
                    GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, Color.clear, fadeSpeed * Time.deltaTime);


                    if (GetComponent<MeshRenderer>().material.color.a == 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    if (GetComponent<RawImage>() != null)
                    {
                        GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, Color.clear, fadeSpeed * Time.deltaTime);


                        if (GetComponent<RawImage>().color.a <= 0.05)
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
            }
        }
        fadeWait -= Time.deltaTime;
    }
}
