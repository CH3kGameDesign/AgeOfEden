using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TypeWriter : MonoBehaviour
{
    public Transform m_tPaper;
    public Transform m_tText;
    public List<Transform> m_lArms = new List<Transform>();
    private List<float> m_lArmMoveRotation = new List<float>();
    private bool m_bArmMove = false;

    public float m_fHalfLength;
    private float m_fLetterWidth;

    public int m_iRow;

    public bool m_bStartTyping = true;
    public bool m_bReturnToTyping = false;
    public List<GameObject> m_lActivateOnFinish = new List<GameObject>();
    public List<GameObject> m_lDeactivateOnFinish = new List<GameObject>();
    public UnityEvent m_ueVoidOnFinish;

    private int m_iLetterPerRow;

    private int m_iEndingNo;
    
    [Serializable]
    public class Ending
    {
        public List<string> startLines = new List<string>();
        public List<string> scriptLines = new List<string>();
        public List<int> freeCharsBeforeScript = new List<int>();
        public GameObject activateOnStart;
        public string lastLine;
    }

    [SerializeField]
    public List<Ending> m_lEndingChanges = new List<Ending>();

    private List<string> m_lScriptLines = new List<string>();
    private string m_sLastLine;
    private List<int> m_lFfreeCharsBeforeScript;

    [Space(20)]
    public List<GameObject> m_goClickSounds = new List<GameObject>();
    public GameObject m_goSpaceSound;
    public GameObject m_goExitSound;

    private int m_iScriptLineCounter = 0;
    private int m_iCharScriptTakesOverCounter;
    
    private bool m_bScriptTyping = false;
    private int m_iScriptCharCounter = 0;
    private string m_sUpdateLetter;

	// Called once before the first frame
	private void Start ()
    {
        m_iEndingNo = PermanentData.saveInfo.lastEndingAchieved;
        for (int i = 0; i < m_lEndingChanges[m_iEndingNo].startLines.Count; i++)
        {
            m_tText.GetChild(i).GetComponent<TextMeshPro>().text = m_lEndingChanges[m_iEndingNo].startLines[i];
        }
        m_iRow = m_lEndingChanges[m_iEndingNo].startLines.Count;

        m_lScriptLines = m_lEndingChanges[m_iEndingNo].scriptLines;
        m_sLastLine = m_lEndingChanges[m_iEndingNo].lastLine;
        m_lFfreeCharsBeforeScript = m_lEndingChanges[m_iEndingNo].freeCharsBeforeScript;

        if (m_lEndingChanges[m_iEndingNo].activateOnStart != null)
            m_lEndingChanges[m_iEndingNo].activateOnStart.SetActive(true);

        for (int i = 0; i < 36; i++)
        {
            m_lArmMoveRotation.Add(-300);
        }

        m_iLetterPerRow = Mathf.RoundToInt((0.166f * (m_tText.GetChild(0).GetComponent<RectTransform>().rect.width * 100))/ m_tText.GetChild(0).GetComponent<TextMeshPro>().fontSize);
        m_tPaper.transform.localPosition = new Vector3(m_fHalfLength, 0.025f * (m_iRow + 1), 0);
        m_fLetterWidth = (m_fHalfLength * 2) / m_iLetterPerRow;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (m_bStartTyping)
        {
            if (m_iRow >= m_tText.childCount)
                return;

            m_sUpdateLetter = null;

            // If max letters on one row reached, move to next row
            if (m_tText.GetChild(m_iRow).GetComponent<TextMeshPro>().text.Length >= m_iLetterPerRow)
                NextRow();

            // Once max row count is reached, end typewriter sequence
            if (m_iRow >= m_tText.childCount)
            {
                if (m_lActivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_lActivateOnFinish.Count; i++)
                    {
                        m_lActivateOnFinish[i].SetActive(true);
                    }
                }

                if (m_lDeactivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_lDeactivateOnFinish.Count; i++)
                    {
                        m_lDeactivateOnFinish[i].SetActive(false);
                    }
                }

                m_ueVoidOnFinish.Invoke();
                m_bStartTyping = false;
                return;
            }

            if (m_iCharScriptTakesOverCounter >= m_lFfreeCharsBeforeScript[m_iScriptLineCounter])
                m_bScriptTyping = true;

            KeyPress();
            m_tPaper.transform.localPosition = new Vector3(m_fHalfLength - (m_fLetterWidth * m_tText.GetChild(m_iRow).GetComponent<TextMeshPro>().text.Length), 0.025f * (m_iRow + 1), 0);

            if (m_sUpdateLetter != null)
                Type(m_sUpdateLetter);
        }
        if (m_bArmMove == true)
            ArmMoveBack();
    }
    
    /// <summary>
    /// Increments the row count
    /// </summary>
    private void NextRow ()
    {
        m_iRow++;
        if (m_iRow != m_tText.childCount)
            m_tPaper.transform.localPosition = new Vector3(m_fHalfLength, 0.025f * (m_iRow + 1), 0);
    }

    /// <summary>
    /// Function that takes over the players input
    /// </summary>
    private void AnyKeyScript ()
    {
        m_tText.GetChild(m_iRow).GetComponent<TextMeshPro>().text += m_lScriptLines[m_iScriptLineCounter][m_iScriptCharCounter];
        m_iScriptCharCounter++;

        if (m_iScriptCharCounter >= m_lScriptLines[m_iScriptLineCounter].Length)
        {
            if (m_iScriptLineCounter < m_lScriptLines.Count - 1)
            {
                m_iScriptLineCounter++;
                m_iScriptCharCounter = 0;
                m_bScriptTyping = false;
                m_iCharScriptTakesOverCounter = 0;
            }
            else
            {
                if (m_bReturnToTyping)
                {
                    m_iCharScriptTakesOverCounter = -1000;
                    m_bScriptTyping = false;
                }
                else
                {
                    if (m_lActivateOnFinish.Count != 0)
                    {
                        for (int i = 0; i < m_lActivateOnFinish.Count; i++)
                        {
                            m_lActivateOnFinish[i].SetActive(true);
                        }
                    }
                    if (m_lDeactivateOnFinish.Count != 0)
                    {
                        for (int i = 0; i < m_lDeactivateOnFinish.Count; i++)
                        {
                            m_lDeactivateOnFinish[i].SetActive(false);
                        }
                    }
                    NextRow();
                    NextRow();
                    m_tText.GetChild(m_iRow).GetComponent<TextMeshPro>().text += m_sLastLine;
                    m_ueVoidOnFinish.Invoke();
                    m_bStartTyping = false;
                    Instantiate(m_goExitSound, transform.position, transform.rotation);
                }
            }
        }
    }

    /// <summary>
    /// Adds the pressed key onto the onscreen textbox
    /// </summary>
    /// <param name="pLetter"></param>
    private void Type (string pLetter)
    {
        string textSound = "";

        if (m_bScriptTyping)
        {
            textSound = m_lScriptLines[m_iScriptLineCounter][m_iScriptCharCounter].ToString().ToLower();
            AnyKeyScript();
        }
        else
        {
            textSound = pLetter;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                pLetter = pLetter.ToUpper();

            m_tText.GetChild(m_iRow).GetComponent<TextMeshPro>().text += pLetter;
            m_iCharScriptTakesOverCounter++;
        }

        int ranInt = UnityEngine.Random.Range(0, m_goClickSounds.Count);

        if (textSound != " ")
        {
            ArmMove(textSound);
            Instantiate(m_goClickSounds[ranInt], transform.position, transform.rotation);
        }
        else
            Instantiate(m_goSpaceSound, transform.position, transform.rotation);
    }

    /// <summary>
    /// Moves the arm of the pressed key forward
    /// </summary>
    /// <param name="pLetter"></param>
    private void ArmMove (string pLetter)
    {
        int choice = -1;

        if (pLetter == "a")
            choice = 0;
        if (pLetter == "b")
            choice = 1;
        if (pLetter == "c")
            choice = 2;
        if (pLetter == "d")
            choice = 3;
        if (pLetter == "e")
            choice = 4;
        if (pLetter == "f")
            choice = 5;
        if (pLetter == "g")
            choice = 6;
        if (pLetter == "h")
            choice = 7;
        if (pLetter == "i")
            choice = 8;
        if (pLetter == "j")
            choice = 9;
        if (pLetter == "k")
            choice = 10;
        if (pLetter == "l")
            choice = 11;
        if (pLetter == "m")
            choice = 12;
        if (pLetter == "n")
            choice = 13;
        if (pLetter == "o")
            choice = 14;
        if (pLetter == "p")
            choice = 15;
        if (pLetter == "q")
            choice = 16;
        if (pLetter == "r")
            choice = 17;
        if (pLetter == "s")
            choice = 18;
        if (pLetter == "t")
            choice = 19;
        if (pLetter == "u")
            choice = 20;
        if (pLetter == "v")
            choice = 21;
        if (pLetter == "w")
            choice = 22;
        if (pLetter == "x")
            choice = 23;
        if (pLetter == "y")
            choice = 24;
        if (pLetter == "z")
            choice = 25;
        if (pLetter == "0")
            choice = 26;
        if (pLetter == "1")
            choice = 27;
        if (pLetter == "2")
            choice = 28;
        if (pLetter == "3")
            choice = 29;
        if (pLetter == "4")
            choice = 30;
        if (pLetter == "5")
            choice = 31;
        if (pLetter == "6")
            choice = 32;
        if (pLetter == "7")
            choice = 33;
        if (pLetter == "8")
            choice = 34;
        if (pLetter == "9")
            choice = 35;
        if (pLetter == ".")
            choice = 28;

        // As long as a valid key is pressed, move the arm into position
        if (choice != -1)
        {
            m_lArms[choice].GetChild(0).localEulerAngles = new Vector3(0, 0, -130);
            m_bArmMove = true;
            m_lArmMoveRotation[choice] = -130;
        }
    }

    /// <summary>
    /// Returns the typewriter arm to its original position
    /// </summary>
    private void ArmMoveBack ()
    {
        m_bArmMove = false;
        for (int i = 0; i < m_lArmMoveRotation.Count; i++)
        {
            if (m_lArmMoveRotation[i] != -300)
            {
                m_bArmMove = true;
                m_lArmMoveRotation[i] -= 10;
                m_lArms[i].GetChild(0).localEulerAngles = new Vector3(0, 0, m_lArmMoveRotation[i]);
            }
        }
    }

    /// <summary>
    /// Loads the most recent key pressed into memory
    /// </summary>
    private void KeyPress ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_sUpdateLetter = " ";

        if (Input.GetKeyDown(KeyCode.A))
            m_sUpdateLetter = "a";

        if (Input.GetKeyDown(KeyCode.B))
            m_sUpdateLetter = "b";
        
        if (Input.GetKeyDown(KeyCode.C))
            m_sUpdateLetter = "c";
        
        if (Input.GetKeyDown(KeyCode.D))
            m_sUpdateLetter = "d";
        
        if (Input.GetKeyDown(KeyCode.E))
            m_sUpdateLetter = "e";
        
        if (Input.GetKeyDown(KeyCode.F))
            m_sUpdateLetter = "f";
        
        if (Input.GetKeyDown(KeyCode.G))
            m_sUpdateLetter = "g";
        
        if (Input.GetKeyDown(KeyCode.H))
            m_sUpdateLetter = "h";
        
        if (Input.GetKeyDown(KeyCode.I))
            m_sUpdateLetter = "i";
        
        if (Input.GetKeyDown(KeyCode.J))
            m_sUpdateLetter = "j";
        
        if (Input.GetKeyDown(KeyCode.K))
            m_sUpdateLetter = "k";
        
        if (Input.GetKeyDown(KeyCode.L))
            m_sUpdateLetter = "l";
        
        if (Input.GetKeyDown(KeyCode.M))
            m_sUpdateLetter = "m";
        
        if (Input.GetKeyDown(KeyCode.N))
            m_sUpdateLetter = "n";
        
        if (Input.GetKeyDown(KeyCode.O))
            m_sUpdateLetter = "o";
        
        if (Input.GetKeyDown(KeyCode.P))
            m_sUpdateLetter = "p";
        
        if (Input.GetKeyDown(KeyCode.Q))
            m_sUpdateLetter = "q";
        
        if (Input.GetKeyDown(KeyCode.R))
            m_sUpdateLetter = "r";
        
        if (Input.GetKeyDown(KeyCode.S))
            m_sUpdateLetter = "s";
        
        if (Input.GetKeyDown(KeyCode.T))
            m_sUpdateLetter = "t";
        
        if (Input.GetKeyDown(KeyCode.U))
            m_sUpdateLetter = "u";
        
        if (Input.GetKeyDown(KeyCode.V))
            m_sUpdateLetter = "v";
        
        if (Input.GetKeyDown(KeyCode.W))
            m_sUpdateLetter = "w";
        
        if (Input.GetKeyDown(KeyCode.X))
            m_sUpdateLetter = "x";
        
        if (Input.GetKeyDown(KeyCode.Y))
            m_sUpdateLetter = "y";
        
        if (Input.GetKeyDown(KeyCode.Z))
            m_sUpdateLetter = "z";
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            m_sUpdateLetter = "1";
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            m_sUpdateLetter = "2";
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            m_sUpdateLetter = "3";
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
            m_sUpdateLetter = "4";
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
            m_sUpdateLetter = "5";
        
        if (Input.GetKeyDown(KeyCode.Alpha6))
            m_sUpdateLetter = "6";
        
        if (Input.GetKeyDown(KeyCode.Alpha7))
            m_sUpdateLetter = "7";
        
        if (Input.GetKeyDown(KeyCode.Alpha8))
            m_sUpdateLetter = "8";
        
        if (Input.GetKeyDown(KeyCode.Alpha9))
            m_sUpdateLetter = "9";
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
            m_sUpdateLetter = "0";
    }
}