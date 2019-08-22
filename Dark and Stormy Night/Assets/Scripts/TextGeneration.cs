using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextGeneration : MonoBehaviour
{
    [Header("Child GameObjects")]
    public GameObject textObject;
    [Header("Variables")]
    public float textSpeed = 0.1f;
    public float destroyTimer;
    public Vector3 direction;
    public float randomPositioning;

    public List<GameObject> singleKeySounds = new List<GameObject>();
    public GameObject spaceKeySound;

    [Tooltip("0: Delete || 1: BlowAway")]
    public int disperseOption;

    public bool playOnAwake = false;

    [Space(20)]

    [Header("Activate")]
    public GameObject[] activateOnFinish;
    public GameObject[] activateOnDestroy;

    private float destroyTime;

    private int textCount;

    [HideInInspector]
    public bool played = false;
    [HideInInspector]
    public string textString;

    // Called once before the first frame
    private void Start()
    {
        textString = textObject.GetComponent<TextMeshPro>().text;
        textObject.GetComponent<TextMeshPro>().text = "";

        if (playOnAwake)
        {
            StartCoroutine("TextGenerate", textSpeed);
            played = true;
        }
    }

    // Called once per frame
    private void Update()
    {
        if (transform.childCount == textString.Length + 1)
        {
            for (int i = 0; i < activateOnFinish.Length; i++)
                activateOnFinish[i].SetActive(true);

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
                activateOnDestroy[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !played)
        {
            StartCoroutine("TextGenerate", textSpeed);
            played = true;
        }
    }

    /// <summary>
    /// Generates text after a delay
    /// </summary>
    /// <param name="pWaitTime">The delay before generating</param>
    /// <returns></returns>
    private IEnumerator TextGenerate(float pWaitTime)
    {
        while (textCount < textString.Length)
        {
            yield return new WaitForSeconds(pWaitTime);

            GameObject GOText = Instantiate(
                textObject, transform.position, transform.rotation, transform);

            GOText.transform.localPosition = new Vector3(
                GOText.GetComponent<TextMeshPro>().fontSize / 17 * textCount * direction.x,
                GOText.GetComponent<TextMeshPro>().fontSize / 13 * textCount * direction.y,
                GOText.GetComponent<TextMeshPro>().fontSize / 17 * textCount * direction.z);

            GOText.transform.position += Random.insideUnitSphere * randomPositioning;

            GOText.GetComponent<TextMeshPro>().text =
                textString.ToCharArray()[textCount].ToString();

            if (singleKeySounds.Count != 0)
            {
                if (textString.ToCharArray()[textCount].ToString() == " ")
                    Instantiate(
                        spaceKeySound, GOText.transform.position, transform.rotation);
                else
                {
                    int no = Random.Range(0, singleKeySounds.Count);
                    Instantiate(
                        singleKeySounds[no], GOText.transform.position, transform.rotation);
                }
            }

            GOText.AddComponent<FadeOut>();

            textCount++;
        }
    }

    /// <summary>
    /// Destroys the text after a delay
    /// </summary>
    /// <param name="pWaitTime">The delay before destroying</param>
    /// <returns></returns>
    private IEnumerator TextDestroy(float pWaitTime)
    {
        while (textCount < textString.Length)
        {
            yield return new WaitForSeconds(pWaitTime);

            //Destroy(transform.GetChild(0).gameObject);
            transform.GetChild(0).GetComponent<FadeOut>().fade = true;
            transform.GetChild(0).SetParent(transform.parent);

            textCount++;
        }
    }

    /// <summary>
    /// Blows the text away
    /// </summary>
    /// <param name="pWaitTime">The delay before animating</param>
    /// <returns></returns>
    private IEnumerator BlowUsAllAway(float pWaitTime)
    {
        while (textCount < textString.Length)
        {
            yield return new WaitForSeconds(pWaitTime);

            transform.GetChild(textCount).GetComponent<Rigidbody>().AddForce(
                Random.insideUnitSphere, ForceMode.Impulse);

            textCount++;
        }

        while (textCount == textString.Length)
        {
            textCount = 0;
            yield return StartCoroutine("TextDestroy", pWaitTime);
        }
    }
}