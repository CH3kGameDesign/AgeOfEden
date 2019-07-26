using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextGeneration : MonoBehaviour {

    [Header ("Child GameObjects")]
    public GameObject textObject;
    [Header("Variables")]
    public float textSpeed = 0.1f;
    public float destroyTimer;
    public Vector3 direction;
    public float randomPositioning;
    [Tooltip ("0: Delete || 1: BlowAway")]
    public int disperseOption;

    public bool playOnAwake = false;

    [Space(20)]

    [Header ("Activate")]
    public GameObject[] activateOnFinish;
    public GameObject[] activateOnDestroy;

    private float destroyTime;

    private int textCount;

    [HideInInspector]
    public bool played = false;
    [HideInInspector]
    public string textString;

    // Use this for initialization
    void Start () {
        textString = textObject.GetComponent<TextMeshPro>().text;
        textObject.GetComponent<TextMeshPro>().text = "";

        if (playOnAwake == true)
        {
            StartCoroutine("TextGenerate", textSpeed);
            played = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == textString.Length + 1)
        {
            for (int i = 0; i < activateOnFinish.Length; i++)
            {
                activateOnFinish[i].SetActive(true);
            }
            if (destroyTime >= destroyTimer)
            {
                Destroy(transform.GetChild(0).gameObject);
                textCount = 0;
                if (disperseOption == 0)
                    StartCoroutine("TextDestroy", textSpeed);
                if (disperseOption == 1)
                    StartCoroutine("BlowUsAllAway", textSpeed);
            }
            else
                destroyTime += Time.deltaTime;
        }
        if (destroyTime >= destroyTimer && transform.childCount == 0)
        {
            for (int i = 0; i < activateOnDestroy.Length; i++)
            {
                activateOnDestroy[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && played == false)
        {
            StartCoroutine("TextGenerate", textSpeed);
            played = true;
        }
    }

    private IEnumerator TextGenerate (float waitTime)
    {
        while (textCount < textString.Length)
        { 
            yield return new WaitForSeconds(waitTime);

            GameObject GOText = Instantiate(textObject, transform.position, transform.rotation, transform);
            GOText.transform.localPosition = new Vector3(((GOText.GetComponent<TextMeshPro>().fontSize / 17) * textCount) * direction.x, ((GOText.GetComponent<TextMeshPro>().fontSize / 13) * textCount) * direction.y, ((GOText.GetComponent<TextMeshPro>().fontSize / 17) * textCount) * direction.z);
            GOText.transform.position += Random.insideUnitSphere * randomPositioning;
            GOText.GetComponent<TextMeshPro>().text = textString.ToCharArray()[textCount].ToString();
            GOText.AddComponent<FadeOut>();

            textCount++;
        }
    }

    private IEnumerator TextDestroy(float waitTime)
    {
        while (textCount < textString.Length)
        {
            yield return new WaitForSeconds(waitTime);

            //Destroy(transform.GetChild(0).gameObject);
            transform.GetChild(0).GetComponent<FadeOut>().fade = true;
            transform.GetChild(0).SetParent(transform.parent);

            textCount++;
        }
    }

    private IEnumerator BlowUsAllAway(float waitTime)
    {
        while (textCount < textString.Length)
        {
            yield return new WaitForSeconds(waitTime);

            transform.GetChild(textCount).GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere, ForceMode.Impulse);

            textCount++;
        }
        while (textCount == textString.Length)
        {
            textCount = 0;
            yield return StartCoroutine("TextDestroy", waitTime);
        }
    }

}
