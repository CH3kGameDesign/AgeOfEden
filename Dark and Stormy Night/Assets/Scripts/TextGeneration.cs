using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class TextGeneration : MonoBehaviour
{
    [HideInInspector]
    public bool m_bPlayed = false;

    [SerializeField]
    private bool m_bBlowAway = false;

    [SerializeField, FormerlySerializedAs("playOnAwake")]
    private bool m_bPlayOnAwake = false;

    [FormerlySerializedAs("textSpeed")]
    public float m_fTextSpeed = 0.1f;

    [SerializeField, FormerlySerializedAs("destroyTimer")]
    private float m_fDestroyDelay = 0f;
    
    [SerializeField, FormerlySerializedAs("randomPositioning")]
    private float m_fRandomOffsetStrength;

    [SerializeField, FormerlySerializedAs("direction")]
    private Vector3 m_v3TextDirection;

    [Header("References")]
    [SerializeField, FormerlySerializedAs("textObject")]
    private GameObject m_goTextObject;

    [SerializeField, FormerlySerializedAs("spaceKeySound")]
    private GameObject m_goSpaceKeySound;
    [SerializeField, FormerlySerializedAs("singleKeySounds")]
    private List<GameObject> m_LgoSingleKeySounds = new List<GameObject>();

    [Header("Activations")]
    [FormerlySerializedAs("activateOnFinish")]
    public GameObject[] m_AgoActivateOnFinish;
    [FormerlySerializedAs("activateOnDestroy")]
    public GameObject[] m_AgoActivateOnDestroy;
    
    private float m_fTimer;

    private string m_sMessage;

    private Transform m_tTransCache;

    private List<Transform> m_LtCharacters = new List<Transform>();

    // Called once before the first frame
    private void Start()
    {
        m_tTransCache = transform;
        
        if (m_goTextObject == null)
            m_goTextObject = m_tTransCache.GetChild(0).gameObject;

        // Loads the message into local storage then clears it
        TextMeshPro textObject = m_goTextObject.GetComponent<TextMeshPro>();
        m_sMessage = textObject.text;
        textObject.text = "";
        
        TextGenerate();

        Destroy(m_tTransCache.GetChild(0).gameObject);

        if (m_bPlayOnAwake)
        {
            StartCoroutine("PresentText");
        }
    }

    // Called once per frame
    private void FixedUpdate()
    {
        // Full text list
        if (m_bPlayed && m_LtCharacters.Count == m_sMessage.Length)
        {
            if (m_fTimer >= m_fDestroyDelay)
            {
                if (m_bBlowAway)
                    StartCoroutine("BlowUsAllAway");
                else
                    StartCoroutine("TextDestroy");
            }
            else
                m_fTimer += Time.deltaTime;
        }

        if (m_fTimer >= m_fDestroyDelay && m_LtCharacters.Count == 0)
        {
            for (int i = 0; i < m_AgoActivateOnDestroy.Length; i++)
                m_AgoActivateOnDestroy[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ONLY WORKS IF A TRIGGER IS ATTACHED TO THIS OBJECT
        // OTHERWISE LOOK INTO TextGenerationCollider
        if (other.tag == "Player" && !m_bPlayed)
        {
            StartCoroutine("PresentText");
        }
    }

    /// <summary>
    /// Generates text after a delay
    /// </summary>
    private void TextGenerate()
    {
        int letterCount = 0;
        while (letterCount < m_sMessage.Length)
        {
            // Creates the character object
            GameObject newChar = Instantiate(
                m_goTextObject, m_tTransCache.position, m_tTransCache.rotation, m_tTransCache);

            TextMeshPro charTMP = newChar.GetComponent<TextMeshPro>();

            // Offsets position
            newChar.transform.localPosition = new Vector3(
                charTMP.fontSize / 17 * letterCount * m_v3TextDirection.x,
                charTMP.fontSize / 13 * letterCount * m_v3TextDirection.y,
                charTMP.fontSize / 17 * letterCount * m_v3TextDirection.z);

            // Apply randomness
            newChar.transform.position += Random.insideUnitSphere * m_fRandomOffsetStrength;

            // Sets the specific character
            charTMP.text = m_sMessage.ToCharArray()[letterCount].ToString();

            // Adds the fadeout script to the object
            newChar.AddComponent<FadeOut>();

            m_LtCharacters.Add(newChar.transform);

            newChar.SetActive(false);

            letterCount++;
        }
    }

    public void PresentTextCoroutine()
    {
        StartCoroutine(PresentText());
    }
    
    private IEnumerator PresentText()
    {
        int letterCount = 0;
        while (letterCount < m_sMessage.Length)
        {
            yield return new WaitForSeconds(m_fTextSpeed);
      
            m_LtCharacters[letterCount].gameObject.SetActive(true);

            // If the key list isnt empty and the character isn't a space
            if (m_LgoSingleKeySounds.Count != 0
                && m_sMessage[letterCount].ToString() != " ")
            {
                // Play a random key noise
                Instantiate(m_LgoSingleKeySounds[Random.Range(0, m_LgoSingleKeySounds.Count)],
                    m_LtCharacters[letterCount].transform.position, m_tTransCache.rotation);
            }
            else
            {
                Instantiate(m_goSpaceKeySound, m_LtCharacters[letterCount].transform.position,
                    m_tTransCache.rotation);
            }

            letterCount++;
        }

        for (int i = 0; i < m_AgoActivateOnFinish.Length; i++)
            m_AgoActivateOnFinish[i].SetActive(true);

        m_bPlayed = true;
    }
    
    private IEnumerator TextDestroy()
    {
        int letterCount = 0;
        while (letterCount < m_sMessage.Length)
        {
            yield return new WaitForSeconds(m_fTextSpeed);

            m_LtCharacters[letterCount].GetComponent<FadeOut>().fade = true;
            //m_LtCharacters[letterCount].SetParent(m_tTransCache.parent);

            letterCount++;
        }
    }
    
    private IEnumerator BlowUsAllAway()
    {
        int letterCount = 0;
        while (letterCount < m_sMessage.Length)
        {
            yield return new WaitForSeconds(m_fTextSpeed);

            m_LtCharacters[letterCount].GetComponent<FadeOut>().fade = true;
            m_LtCharacters[letterCount].GetComponent<Rigidbody>().AddForce(
                Random.insideUnitSphere, ForceMode.Impulse);

            letterCount++;
        }
    }
}