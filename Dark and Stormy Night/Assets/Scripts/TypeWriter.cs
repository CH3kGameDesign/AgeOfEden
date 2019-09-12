using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TypeWriter : MonoBehaviour
{
    [System.Serializable]
    internal class EndingData
    {
        [Tooltip("Each list item is the amount of characters the player can type" +
            " before the script writes in the next line item")]
        [FormerlySerializedAs("freeCharsBeforeScript")]
        public List<int> m_iCharsUntilScript = new List<int>();
        [Tooltip("Text already printed onto the typewriter when the player loads in")]
        [FormerlySerializedAs("startLines")]
        public List<string> m_sStartLines = new List<string>();

        public List<bool> m_bStrikeThrough = new List<bool>();

        [Tooltip("What the script will write into the text when it takes control")]
        [FormerlySerializedAs("scriptLines")]
        public List<string> m_sScriptLines = new List<string>();
        [Tooltip("Final line printed onto the text as the player is kicked back")]
        [FormerlySerializedAs("lastLine")]
        public string m_sLastLine;
        [Tooltip("A gameobject activated along with a specific ending")]
        [FormerlySerializedAs("activateOnStart")]
        public GameObject m_goActivateOnStart;
    }

    [Tooltip("Whether or not the player is currently typing")]
    [FormerlySerializedAs("startTyping")]
    public bool m_bIsTyping = true;

    [Tooltip("Allows the player to type independant of the script")]
    [FormerlySerializedAs("returnToTyping")]
    [SerializeField]
    private bool m_bNoScriptInterference = false;

    [Tooltip("Half the width of the paper")]
    [FormerlySerializedAs("halfLength")]
    [SerializeField]
    private float halfLength;
    // The length per character
    private float letterWidth;
    private int letterPerRow;
    // The current row the typewriter is on
    private int row;

    // Whether the script should attempt to animate arms to their idle position
    private bool armMove = false;
    // List of arm rotation angles used to animate them to idle position
    private List<float> armMoveRotation = new List<float>();
    private float spaceKeyRotation;

    // The most recent ending completed
    private int endingNo;
    // The script lines from the currently selected ending
    private List<string> scriptLines = new List<string>();
    private List<bool> strikeThrough = new List<bool>();
    // The final line written as the player is kicked off the typewriter
    private string lastLine;
    // Each list item is the amount of characters the player can type before the
    // script takes control
    private List<int> freeCharsBeforeScript;

    private int scriptLineCounter = 0;
    private int charScriptTakesOverCounter;
    
    private bool scriptTyping = false;
    private int scriptCharCounter = 0;
    private string updateLetter;

    private List<string> m_sFullMessage = new List<string>();
    private string m_sMessageLine;

    [Header("References")]
    
    [Tooltip("The different scritps written to the paper")]
    [FormerlySerializedAs("EndingChanges")]
    [SerializeField]
    private List<EndingData> m_edEndingChanges = new List<EndingData>();
    [Tooltip("A list of all the typewriter arm objects")]
    [FormerlySerializedAs("arms")]
    [SerializeField]
    private List<Transform> m_tTypeWriterArms = new List<Transform>();
    [Tooltip("A list of all the typewriter key objects")]
    [FormerlySerializedAs("arms")]
    [SerializeField]
    private List<Transform> m_tTypeWriterKeys = new List<Transform>();
    [SerializeField]
    private Transform m_tSpaceKey;
    [Tooltip("A list of sounds for when keys are pressed")]
    [FormerlySerializedAs("clickSounds")]
    [SerializeField]
    private List<GameObject> m_goClickSounds = new List<GameObject>();
    [Tooltip("The sound played when the spacebar is pressed")]
    [FormerlySerializedAs("spaceSound")]
    [SerializeField]
    private GameObject m_goSpaceSound;
    [Tooltip("The sound played when a line ends")]
    [FormerlySerializedAs("exitSound")]
    [SerializeField]
    private GameObject m_goExitSound;

    [Space(5)]
    [Tooltip("A reference to the transform of the paperHolder")]
    [FormerlySerializedAs("paperHolder")]
    [SerializeField]
    private Transform m_tPaperHolder;
    [Tooltip("A reference to the transform of the paper")]
    [FormerlySerializedAs("paper")]
    [SerializeField]
    private Transform m_tPaper;
    [Tooltip("A reference to the transform of the text object")]
    [FormerlySerializedAs("text")]
    [SerializeField]
    private Transform m_tText;
    [Tooltip("A reference to the transforms of the paper meshes")]
    [FormerlySerializedAs("paperMeshes")]
    [SerializeField]
    private List<GameObject> m_tPaperMeshes = new List<GameObject>();

    [Space(10)]
    [Tooltip("A list of gameobejcts to be enabled when the sequence ends")]
    [FormerlySerializedAs("activateOnFinish")]
    [SerializeField]
    private List<GameObject> m_goActivateOnFinish = new List<GameObject>();
    [Tooltip("A list of gameobjects to be disabled when the sequence ends")]
    [FormerlySerializedAs("deActivateOnFinish")]
    [SerializeField]
    private List<GameObject> m_goDeactivateOnFinish = new List<GameObject>();

    [Space(10)]
    [Tooltip("Used to call certain scripts when the typewriter sequence ends")]
    [FormerlySerializedAs("voidOnFinish")]
    [SerializeField]
    private UnityEvent m_ueVoidOnFinish;

	// Called once before the first frame
	private void Start()
    {
        // Gets the most recent ending completed
        endingNo = PermanentData.saveInfo.lastEndingAchieved;

        // Loads all the text data
        scriptLines = m_edEndingChanges[endingNo].m_sScriptLines;
        strikeThrough = m_edEndingChanges[endingNo].m_bStrikeThrough;
        lastLine = m_edEndingChanges[endingNo].m_sLastLine;
        freeCharsBeforeScript = m_edEndingChanges[endingNo].m_iCharsUntilScript;

        // Loads the start text onto the typewriter
        for (int i = 0; i < m_edEndingChanges[endingNo].m_sStartLines.Count; i++)
        {
            m_tText.GetChild(0).GetComponent<TextMeshPro>().text =
                m_edEndingChanges[endingNo].m_sStartLines[i];
            NextRow();
        }
        

        // Sets a desired gameobject as active if there is one
        if (m_edEndingChanges[endingNo].m_goActivateOnStart)
            m_edEndingChanges[endingNo].m_goActivateOnStart.SetActive(true);

        // Sets all the arms to their idle position
        for (int i = 0; i < 36; i++)
            armMoveRotation.Add(-300);

        // Gets the total amount of characters that can fit on one line
        letterPerRow = Mathf.RoundToInt(0.166f *
            (m_tText.GetChild(0).GetComponent<RectTransform>().rect.width * 100)
            / m_tText.GetChild(0).GetComponent<TextMeshPro>().fontSize);
        // Uses the paper width and the letters per row to find the letter width
        letterWidth = (halfLength * 2) / letterPerRow;
        // Offsets the paper so the first character lines up with the arm
        m_tPaper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bIsTyping && SceneManager.sceneCount == 1)
        {
            // Resets the update letter
            updateLetter = null;

            // Moves to the next row if the max letters per row is reached
            if (m_tText.GetChild(0).GetComponent<TextMeshPro>().text.Length >= letterPerRow)
                NextRow();

            // Once max rows have been reached, end typewriter sequence
            if (row >= m_tText.childCount)
            {
                // Finalisation stuff
                if (m_goActivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_goActivateOnFinish.Count; i++)
                        m_goActivateOnFinish[i].SetActive(true);
                }

                if (m_goDeactivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_goDeactivateOnFinish.Count; i++)
                        m_goDeactivateOnFinish[i].SetActive(false);
                }

                CreateTextFile.SetMessage(m_sFullMessage);
                m_ueVoidOnFinish.Invoke();
                m_bIsTyping = false;
                OnHopefullyDisable();
                return;
            }

            // Initiates script typing once the conditions are met
            if (charScriptTakesOverCounter >= freeCharsBeforeScript[scriptLineCounter])
                scriptTyping = true;

            // Loads a valid key press into update letter
            KeyPress();

            // Moves the paper to the new position
            m_tPaperHolder.transform.localPosition = new Vector3(-0.121f,
                0.247f,
                halfLength - (letterWidth * m_tText.GetChild(0).GetComponent<TextMeshPro>().text.Length))
                / 100;
            m_tPaper.transform.localPosition = new Vector3(0,
                0.025f * (row + 1), 
                0)
                /100;

            // As long as a valid key was pressed, attempt to type onto the paper
            if (updateLetter != null)
                Type(updateLetter);
        }

        // Calls the arm animation function
        if (armMove)
            ArmMoveBack();
        if (spaceKeyRotation < 0)
        {
            spaceKeyRotation += 0.5f;
            m_tSpaceKey.localEulerAngles = new Vector3(0, 0, spaceKeyRotation);
        }
    }

    /// <summary>
    /// Either types the pressed letter or uses the one from the script, also makes a noise
    /// </summary>
    /// <param name="pLetter">The letter used</param>
    private void Type(string pLetter)
    {
        // String used to test if the character inputted was a space
        string textSound = "";
        if (scriptTyping)
        {
            textSound = scriptLines[scriptLineCounter][scriptCharCounter].ToString().ToLower();
            ScriptType();
        }
        else
        {
            textSound = pLetter;

            // Shifts the letter
            if (!(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
                pLetter = pLetter.ToUpper();

            // Applies the letter to the paper text
            m_tText.GetChild(0).GetComponent<TextMeshPro>().text += pLetter;
            m_sMessageLine += pLetter;
            charScriptTakesOverCounter++;
        }

        // Random click noise
        int ranInt = Random.Range(0, m_goClickSounds.Count);

        if (textSound != " ")
        {
            ArmMove(textSound);
            Instantiate(m_goClickSounds[ranInt], transform.position, transform.rotation);
        }
        else
        {
            Instantiate(m_goSpaceSound, transform.position, transform.rotation);
            spaceKeyRotation = -5;
            m_tSpaceKey.localEulerAngles = new Vector3(0, 0, -5);
        }
    }

    /// <summary>
    /// Function for when the script takes control of whats being typed
    /// </summary>
    private void ScriptType()
    {
        // Enters the letter into the typewriter text
        m_tText.GetChild(0).GetComponent<TextMeshPro>().text +=
            scriptLines[scriptLineCounter][scriptCharCounter];
        m_sMessageLine += scriptLines[scriptLineCounter][scriptCharCounter];

        scriptCharCounter++;

        if (scriptCharCounter >= scriptLines[scriptLineCounter].Length)
        {
            if (scriptLineCounter < scriptLines.Count - 1)
            {
                scriptLineCounter++;
                scriptCharCounter = 0;
                scriptTyping = false;
                charScriptTakesOverCounter = 0;
            }
            else if (m_bNoScriptInterference == true)
            {
                charScriptTakesOverCounter = -1000;
                scriptTyping = false;
            }
            else
            {
                // Leaves some room and enters the final line down
                NextRow();
                NextRow();
                m_tText.GetChild(0).GetComponent<TextMeshPro>().text += lastLine;
                m_sMessageLine += lastLine;
                m_sFullMessage.Add(m_sMessageLine);

                // Finalisation stuff
                if (m_goActivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_goActivateOnFinish.Count; i++)
                        m_goActivateOnFinish[i].SetActive(true);
                }

                if (m_goDeactivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < m_goDeactivateOnFinish.Count; i++)
                        m_goDeactivateOnFinish[i].SetActive(false);
                }

                CreateTextFile.SetMessage(m_sFullMessage);
                m_ueVoidOnFinish.Invoke();
                m_bIsTyping = false;
                Instantiate(m_goExitSound, transform.position, transform.rotation);
                OnHopefullyDisable();
            }
        }
    }
    
    /// <summary>
    /// Increments the row counter and translates the paper
    /// </summary>
    private void NextRow()
    {
        for (int i = 0; i < m_tText.childCount; i++)
            m_tText.GetChild(i).GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
        for (int i = 0; i < strikeThrough.Count; i++)
        {
            int choice = row - i + 1;
            if (choice >= 0)
            {
                if (strikeThrough[i] && choice < m_tText.childCount)
                    m_tText.GetChild(choice).GetComponent<TextMeshPro>().fontStyle = FontStyles.Strikethrough;
                
            }
        }
        row++;
        if (row != m_tText.childCount)
            m_tPaper.transform.localPosition = new Vector3(0, 0.025f * (row + 1), 0) / 100;
        m_sFullMessage.Add(m_sMessageLine);
        m_sMessageLine = "";
        for (int i = 0; i < m_tPaperMeshes.Count; i++)
        {
            if (i == row)
                m_tPaperMeshes[i].SetActive(true);
            else
                m_tPaperMeshes[i].SetActive(false);
        }
        for (int i = m_tText.childCount - 1; i > 0; i--)
        {
            m_tText.GetChild(i).GetComponent<TextMeshPro>().text = m_tText.GetChild(i - 1).GetComponent<TextMeshPro>().text;
        }
        m_tText.GetChild(0).GetComponent<TextMeshPro>().text = "";
    }

    /// <summary>
    /// Moves the correct arm into place based on what key was pressed
    /// </summary>
    /// <param name="pLetter">The letter used</param>
    private void ArmMove(string pLetter)
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
        // As long as a valid key is pressed, the typewriter arm is moved into position
        if (choice != -1)
        {
            m_tTypeWriterArms[choice].localEulerAngles = new Vector3(0, 0, 90);
            m_tTypeWriterKeys[choice].localEulerAngles = new Vector3(0, 0, -30);
            armMove = true;
            armMoveRotation[choice] = 90;
        }
            
    }

    /// <summary>
    /// Returns the arm to its idle state
    /// </summary>
    private void ArmMoveBack()
    {
        armMove = false;
        for (int i = 0; i < armMoveRotation.Count; i++)
        {
            if (armMoveRotation[i] > 0)
            {
                armMove = true;
                armMoveRotation[i] -= 5;
                m_tTypeWriterArms[i].localEulerAngles = new Vector3(0, m_tTypeWriterArms[i].localEulerAngles.y, armMoveRotation[i]);
                m_tTypeWriterKeys[i].localEulerAngles = new Vector3(0, 0, -armMoveRotation[i] / 3);
            }
            else
            {
                armMoveRotation[i] = 0;
                m_tTypeWriterArms[i].localEulerAngles = new Vector3(0, m_tTypeWriterArms[i].localEulerAngles.y, 0);
                m_tTypeWriterKeys[i].localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// Loads the most recently pressed key into memory
    /// </summary>
    private void KeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            updateLetter = " ";
        if (Input.GetKeyDown(KeyCode.A))
            updateLetter = "a";
        if (Input.GetKeyDown(KeyCode.B))
            updateLetter = "b";
        if (Input.GetKeyDown(KeyCode.C))
            updateLetter = "c";
        if (Input.GetKeyDown(KeyCode.D))
            updateLetter = "d";
        if (Input.GetKeyDown(KeyCode.E))
            updateLetter = "e";
        if (Input.GetKeyDown(KeyCode.F))
            updateLetter = "f";
        if (Input.GetKeyDown(KeyCode.G))
            updateLetter = "g";
        if (Input.GetKeyDown(KeyCode.H))
            updateLetter = "h";
        if (Input.GetKeyDown(KeyCode.I))
            updateLetter = "i";
        if (Input.GetKeyDown(KeyCode.J))
            updateLetter = "j";
        if (Input.GetKeyDown(KeyCode.K))
            updateLetter = "k";
        if (Input.GetKeyDown(KeyCode.L))
            updateLetter = "l";
        if (Input.GetKeyDown(KeyCode.M))
            updateLetter = "m";
        if (Input.GetKeyDown(KeyCode.N))
            updateLetter = "n";
        if (Input.GetKeyDown(KeyCode.O))
            updateLetter = "o";
        if (Input.GetKeyDown(KeyCode.P))
            updateLetter = "p";
        if (Input.GetKeyDown(KeyCode.Q))
            updateLetter = "q";
        if (Input.GetKeyDown(KeyCode.R))
            updateLetter = "r";
        if (Input.GetKeyDown(KeyCode.S))
            updateLetter = "s";
        if (Input.GetKeyDown(KeyCode.T))
            updateLetter = "t";
        if (Input.GetKeyDown(KeyCode.U))
            updateLetter = "u";
        if (Input.GetKeyDown(KeyCode.V))
            updateLetter = "v";
        if (Input.GetKeyDown(KeyCode.W))
            updateLetter = "w";
        if (Input.GetKeyDown(KeyCode.X))
            updateLetter = "x";
        if (Input.GetKeyDown(KeyCode.Y))
            updateLetter = "y";
        if (Input.GetKeyDown(KeyCode.Z))
            updateLetter = "z";
        if (Input.GetKeyDown(KeyCode.Alpha1))
            updateLetter = "1";
        if (Input.GetKeyDown(KeyCode.Alpha2))
            updateLetter = "2";
        if (Input.GetKeyDown(KeyCode.Alpha3))
            updateLetter = "3";
        if (Input.GetKeyDown(KeyCode.Alpha4))
            updateLetter = "4";
        if (Input.GetKeyDown(KeyCode.Alpha5))
            updateLetter = "5";
        if (Input.GetKeyDown(KeyCode.Alpha6))
            updateLetter = "6";
        if (Input.GetKeyDown(KeyCode.Alpha7))
            updateLetter = "7";
        if (Input.GetKeyDown(KeyCode.Alpha8))
            updateLetter = "8";
        if (Input.GetKeyDown(KeyCode.Alpha9))
            updateLetter = "9";
        if (Input.GetKeyDown(KeyCode.Alpha0))
            updateLetter = "0";
    }

    private void OnHopefullyDisable()
    {
        bool doThing = true;
        for (int i = 0; i < m_tPaperMeshes.Count; i++)
        {
            if (m_tPaperMeshes[i].activeInHierarchy == true)
                doThing = false;
        }
        if (doThing)
            m_tPaperMeshes[8].SetActive(true);
    }
}