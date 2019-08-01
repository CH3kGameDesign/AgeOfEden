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
    
    // Update is called once per frame
    private void Update()
    {
        if (fadeWait <= 0)
            fade = true;

        if (fade)
        {
            if (GetComponent<TextMeshPro>())
            {
                GetComponent<TextMeshPro>().color = Color.Lerp(
                    GetComponent<TextMeshPro>().color, Color.clear,
                    fadeSpeed * Time.deltaTime); 

                if (GetComponent<TextMeshPro>().color.a == 0)
                    Destroy(gameObject);
            }
            else
            {
                if (GetComponent<MeshRenderer>())
                {
                    GetComponent<MeshRenderer>().material.color = Color.Lerp(
                        GetComponent<MeshRenderer>().material.color,
                        Color.clear, fadeSpeed * Time.deltaTime);

                    if (GetComponent<MeshRenderer>().material.color.a == 0)
                        Destroy(gameObject);
                }
                else
                {
                    if (GetComponent<RawImage>())
                    {
                        GetComponent<RawImage>().color = Color.Lerp(
                            GetComponent<RawImage>().color, Color.clear,
                            fadeSpeed * Time.deltaTime);

                        if (GetComponent<RawImage>().color.a <= 0.05)
                            Destroy(gameObject);
                    }
                }
            }
        }
        fadeWait -= Time.deltaTime;
    }
}
