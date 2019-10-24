using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TypeWriter : MonoBehaviour
{
    [System.Serializable]
    internal class EndingData
    {
        [Tooltip("Each list item is the amount of characters the player can type" +
            " before the script writes in the next line item")]
        public List<int> m_iCharsUntilScript = new List<int>();
        [Tooltip("Text already printed onto the typewriter when the player loads in")]
        public List<string> m_sStartLines = new List<string>();

        public List<bool> m_bStrikeThrough = new List<bool>();
        public List<bool> m_bPlayerType = new List<bool>();
        public List<bool> m_bNewLineAfter = new List<bool>();

        [Tooltip("What the script will write into the text when it takes control")]
        public List<string> m_sScriptLines = new List<string>();
        [Tooltip("Final line printed onto the text as the player is kicked back")]
        public string m_sLastLine;
        [Tooltip("A gameobject activated along with a specific ending")]
        public GameObject m_goActivateOnStart;
    }

    [Tooltip("Whether or not the player is currently typing")]
    public bool m_bIsTyping = true;

    [SerializeField, Tooltip("Allows the player to type independant of the script")]
    private bool m_bNoScriptInterference = false;

    [SerializeField, Tooltip("Half the width of the paper")]
    private float halfLength;
    // The length per character
    private float letterWidth;
    private int letterPerRow;
    // The current row the typewriter is on
    private int row;
    private bool makeRowSound = false;

    // Whether the script should attempt to animate arms to their idle position
    private bool armMove = false;
    // List of arm rotation angles used to animate them to idle position
    private List<float> armMoveRotation = new List<float>();
    private List<bool> armMoveUpNow = new List<bool>();
    private float spaceKeyRotation;

    // The most recent ending completed
    private int endingNo;
    // The script lines from the currently selected ending
    private List<string> scriptLines = new List<string>();
    private List<bool> strikeThrough = new List<bool>();
    private List<bool> playerType = new List<bool>();
    private List<bool> newLineAfter = new List<bool>();
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
    
    [SerializeField, Tooltip("The different scritps written to the paper")]
    private List<EndingData> m_edEndingChanges = new List<EndingData>();
    [SerializeField, Tooltip("A list of all the typewriter arm objects")]
    private List<Transform> m_tTypeWriterArms = new List<Transform>();
    [SerializeField, Tooltip("A list of all the typewriter key objects")]
    private List<Transform> m_tTypeWriterKeys = new List<Transform>();
    [SerializeField, Tooltip("A reference to the space key")]
    private Transform m_tSpaceKey;
    [SerializeField, Tooltip("A list of sounds for when keys are pressed")]
    private List<GameObject> m_goClickSounds = new List<GameObject>();
    [SerializeField, Tooltip("The sound played when the spacebar is pressed")]
    private GameObject m_goSpaceSound;
    [SerializeField, Tooltip("The sound played when a line ends")]
    private GameObject m_goEnterSound;
    [SerializeField, Tooltip("The sound played when the event ends")]
    private GameObject m_goExitSound;

    [Space(5)]
    [SerializeField, Tooltip("A reference to the transform of the paperHolder")]
    private Transform m_tPaperHolder;
    [SerializeField, Tooltip("A reference to the transform of the paper")]
    private Transform m_tPaper;
    [SerializeField, Tooltip("A reference to the transform of the text object")]
    private Transform m_tText;
    [SerializeField, Tooltip("A reference to the transforms of the paper meshes")]
    private List<GameObject> m_tPaperMeshes = new List<GameObject>();

    [Space(10)]
    [SerializeField, Tooltip("A list of gameobejcts to be enabled when the sequence ends")]
    private List<GameObject> m_goActivateOnFinish = new List<GameObject>();
    [SerializeField, Tooltip("A list of gameobjects to be disabled when the sequence ends")]
    private List<GameObject> m_goDeactivateOnFinish = new List<GameObject>();

    [Space(10)]
    [SerializeField, Tooltip("Used to call certain scripts when the typewriter sequence ends")]
    private UnityEvent m_ueVoidOnFinish;

    private float selfTypeTimer;
    public Vector2 selfTypeBounds;

    private float enterTimer = 100000;
    private bool activatedEnding = false;
    public GameObject activateOnAllEndings;
    private int endNo= 0;

    private Transform m_tTransCache;
    private TextMeshPro m_tmpTextFirstChildTmp;

    // Called once before the first frame
    private void Start()
    {
        m_tTransCache = transform;
        m_tmpTextFirstChildTmp = m_tText.GetChild(0).GetComponent<TextMeshPro>();

        // Gets the most recent ending completed
        endingNo = PermanentData.saveInfo.lastEndingAchieved;

        // Loads all the text data
        scriptLines = m_edEndingChanges[endingNo].m_sScriptLines;
        strikeThrough = m_edEndingChanges[endingNo].m_bStrikeThrough;
        playerType = m_edEndingChanges[endingNo].m_bPlayerType;
        newLineAfter = m_edEndingChanges[endingNo].m_bNewLineAfter;
        lastLine = m_edEndingChanges[endingNo].m_sLastLine;
        freeCharsBeforeScript = m_edEndingChanges[endingNo].m_iCharsUntilScript;
        if (endingNo == 0)
            PermanentData.saveInfo.name = "";

        // Loads the start text onto the typewriter
        for (int i = 0; i < m_edEndingChanges[endingNo].m_sStartLines.Count; i++)
        {
            m_tmpTextFirstChildTmp.text =
                m_edEndingChanges[endingNo].m_sStartLines[i];
            NextRow();
        }
        for (int i = 0; i < scriptLines.Count; i++)
        {
            if (playerType.Count == i)
                playerType.Add(true);
            if (newLineAfter.Count == i)
                newLineAfter.Add(false);
        }
        

        // Sets a desired gameobject as active if there is one
        if (m_edEndingChanges[endingNo].m_goActivateOnStart)
            m_edEndingChanges[endingNo].m_goActivateOnStart.SetActive(true);

        // Sets all the arms to their idle position
        for (int i = 0; i < 36; i++)
        {
            armMoveRotation.Add(0);
            armMoveUpNow.Add(false);
        }

        // Gets the total amount of characters that can fit on one line
        letterPerRow = Mathf.RoundToInt(0.166f *
            (m_tText.GetChild(0).GetComponent<RectTransform>().rect.width * 100)
            / m_tmpTextFirstChildTmp.fontSize);
        // Uses the paper width and the letters per row to find the letter width
        letterWidth = (halfLength * 2) / letterPerRow;
        // Offsets the paper so the first character lines up with the arm
        m_tPaper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
        selfTypeTimer = Random.Range(selfTypeBounds.x, selfTypeBounds.y);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_bIsTyping && SceneManager.sceneCount == 1)
        {
            // Resets the update letter
            updateLetter = null;

            // Moves to the next row if the max letters per row is reached
            if (m_tmpTextFirstChildTmp.text.Length >= letterPerRow)
                NextRow();

            /*
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
            */

            // Initiates script typing once the conditions are met
            if (charScriptTakesOverCounter >= freeCharsBeforeScript[scriptLineCounter]
                && freeCharsBeforeScript[scriptLineCounter] != -1)
                scriptTyping = true;

            ///////////////////////////// MOVED UP A BIT
            // Moves the paper to the new position
            m_tPaperHolder.transform.localPosition = Vector3.Lerp(
                m_tPaperHolder.transform.localPosition, new Vector3(-0.121f,
                0.247f,
                halfLength - (letterWidth * m_tmpTextFirstChildTmp.text.Length))
                / 100, Time.deltaTime * 8);
            m_tPaper.transform.localPosition = new Vector3(0,
                0.025f * (row + 1),
                0)
                / 100;
            /////////////////////////////

            if (playerType[scriptLineCounter] == true || scriptTyping == false)
            {
                KeyPress();
                if (freeCharsBeforeScript[scriptLineCounter] == -1)
                {
                    if (Input.GetKeyDown(KeyCode.Return) || enterTimer <= 0 || charScriptTakesOverCounter > 12)
                    {
                        scriptTyping = true;
                        Type("");
                    }
                    enterTimer -= Time.deltaTime;
                }
            }
            else
            {
                if (selfTypeTimer < 0)
                {
                    selfTypeTimer = Random.Range(selfTypeBounds.x, selfTypeBounds.y);
                    Type("");
                }
                selfTypeTimer -= Time.deltaTime;
            }

            // As long as a valid key was pressed, attempt to type onto the paper
            if (updateLetter != null)
                Type(updateLetter);
        }

        // Calls the arm animation function
        if (armMove)
        {
            ArmMoveUp();
            ArmMoveBack();
        }
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
        if (freeCharsBeforeScript[scriptLineCounter] == -1)
        {
            PermanentData.saveInfo.name += pLetter;
            if (enterTimer > 10000)
                enterTimer = 10000;
            else
                enterTimer = 2.5f;
        }
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
            m_tmpTextFirstChildTmp.text += pLetter;
            m_sMessageLine += pLetter;
            charScriptTakesOverCounter++;
        }

        // Random click noise
        int ranInt = Random.Range(0, m_goClickSounds.Count);

        if (textSound != " ")
        {
            ArmMove(textSound);
            Instantiate(m_goClickSounds[ranInt], m_tTransCache.position, m_tTransCache.rotation);
        }
        else
        {
            Instantiate(m_goSpaceSound, m_tTransCache.position, m_tTransCache.rotation);
            spaceKeyRotation = -5;
            m_tSpaceKey.localEulerAngles = new Vector3(0, 0, -5);
        }
    }

    /// <summary>
    /// Function for when the script takes control of whats being typed
    /// </summary>
    private void ScriptType()
    {
        char scriptLetter = ' ';
        string st = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
        if (scriptLines[scriptLineCounter][scriptCharCounter] == '|')
            scriptLetter = st[Random.Range(0, 61)];
        else
            scriptLetter = scriptLines[scriptLineCounter][scriptCharCounter];

        // Enters the letter into the typewriter text
        m_tmpTextFirstChildTmp.text +=
            scriptLetter;
        m_sMessageLine += scriptLetter;

        scriptCharCounter++;

        if (scriptCharCounter >= scriptLines[scriptLineCounter].Length)
        {
            endNo = 0;
            for (int i = 0; i < PermanentData.saveInfo.endingsAchieved.Count; i++)
            {
                if (PermanentData.saveInfo.endingsAchieved[i])
                    endNo++;
            }

            if (scriptLineCounter < scriptLines.Count - 1)
            {
                if (newLineAfter[scriptLineCounter] == true)
                    NextRow();
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
            else if (endNo >= PermanentData.saveInfo.endingsAchieved.Count - 2)
            {
                if (activatedEnding == true)
                    activateOnAllEndings.SetActive(true);
                else
                {
                    activatedEnding = true;
                    scriptLines.Add("Wait.");
                    scriptLines.Add("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                    playerType.Add(false);
                    playerType.Add(false);
                    freeCharsBeforeScript.Add(0);
                    freeCharsBeforeScript.Add(0);
                    newLineAfter.Add(true);
                    newLineAfter.Add(false);
                    NextRow();
                    NextRow();
                    scriptLineCounter++;
                    scriptCharCounter = 0;
                    scriptTyping = false;
                    selfTypeBounds *= 0.5f;
                    charScriptTakesOverCounter = 0;
                }
            }
            else
            {
                makeRowSound = false;
                // Leaves some room and enters the final line down
                if (lastLine != "")
                {
                    NextRow();
                    NextRow();
                    m_tmpTextFirstChildTmp.text += lastLine;
                    m_sMessageLine += lastLine;
                }
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
                Instantiate(m_goExitSound, m_tTransCache.position, m_tTransCache.rotation);
                OnHopefullyDisable();
            }
        }
    }
    
    /// <summary>
    /// Increments the row counter and translates the paper
    /// </summary>
    private void NextRow()
    {
        if (makeRowSound)
            Instantiate(m_goEnterSound, m_tTransCache.position, m_tTransCache.rotation);
        for (int i = 0; i < m_tText.childCount; i++)
            m_tText.GetChild(i).GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
        for (int i = 0; i < strikeThrough.Count; i++)
        {
            int choice = row - i + 1;
            if (choice >= 0)
            {
                if (strikeThrough[i] && choice < m_tText.childCount)
                    m_tText.GetChild(choice).GetComponent<TextMeshPro>().fontStyle
                        = FontStyles.Strikethrough;
            }
        }
        row++;
        
        if (row != m_tText.childCount || endNo >= PermanentData.saveInfo.endingsAchieved.Count - 2)
            m_tPaper.transform.localPosition = new Vector3(0, 0.025f * (row + 1), 0) / 100;
        m_sFullMessage.Add(m_sMessageLine);
        m_sMessageLine = "";
        if (row >= m_tPaperMeshes.Count)
            m_tPaperMeshes[m_tPaperMeshes.Count - 1].SetActive(true);
        else
        {
            for (int i = 0; i < m_tPaperMeshes.Count; i++)
            {
                if (i == row)
                    m_tPaperMeshes[i].SetActive(true);
                else
                    m_tPaperMeshes[i].SetActive(false);
            }
        }
        for (int i = m_tText.childCount - 1; i > 0; i--)
        {
            m_tText.GetChild(i).GetComponent<TextMeshPro>().text
                = m_tText.GetChild(i - 1).GetComponent<TextMeshPro>().text;
        }
        m_tmpTextFirstChildTmp.text = "";
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
        if (pLetter == "0" || pLetter == ")")
            choice = 26;
        if (pLetter == "1" || pLetter == "!")
            choice = 27;
        if (pLetter == "2" || pLetter == "@")
            choice = 28;
        if (pLetter == "3" || pLetter == "#")
            choice = 29;
        if (pLetter == "4" || pLetter == "$")
            choice = 30;
        if (pLetter == "5" || pLetter == "%")
            choice = 31;
        if (pLetter == "6" || pLetter == "^")
            choice = 32;
        if (pLetter == "7" || pLetter == "&")
            choice = 33;
        if (pLetter == "8" || pLetter == "*")
            choice = 34;
        if (pLetter == "9" || pLetter == "(")
            choice = 35;

        if (pLetter == "[" || pLetter == "{")
            choice = 15;
        if (pLetter == "]" || pLetter == "}")
            choice = 15;
        if (pLetter == "\\" || pLetter == "|")
            choice = 15;

        if (pLetter == ";" || pLetter == ":")
            choice = 11;
        if (pLetter == "'" || pLetter == "\"")
            choice = 11;

        if (pLetter == "," || pLetter == "<")
            choice = 12;
        if (pLetter == "." || pLetter == ">")
            choice = 12;
        if (pLetter == "/" || pLetter == "?")
            choice = 12;

        // As long as a valid key is pressed, the typewriter arm is moved into position
        if (choice != -1)
        {
            m_tTypeWriterArms[choice].localEulerAngles = new Vector3(0, 0, 90);
            m_tTypeWriterKeys[choice].localEulerAngles = new Vector3(0, 0, -30);
            armMove = true;
            armMoveUpNow[choice] = true;
        }
            
    }

    /// <summary>
    /// Moves the arm to its press state
    /// </summary>
    private void ArmMoveUp()
    {
        armMove = false;
        for (int i = 0; i < armMoveRotation.Count; i++)
        {
            if (armMoveUpNow[i])
            {
                if (armMoveRotation[i] < 90)
                {
                    armMove = true;
                    armMoveRotation[i] += 30;
                    m_tTypeWriterArms[i].localEulerAngles = new Vector3(
                        0, m_tTypeWriterArms[i].localEulerAngles.y, armMoveRotation[i]);
                    m_tTypeWriterKeys[i].localEulerAngles = new Vector3(
                        0, 0, -armMoveRotation[i] / 3);
                }
                else
                {
                    armMoveUpNow[i] = false;
                    armMoveRotation[i] = 90;
                    m_tTypeWriterArms[i].localEulerAngles = new Vector3(
                        0, m_tTypeWriterArms[i].localEulerAngles.y, 0);
                    m_tTypeWriterKeys[i].localEulerAngles = new Vector3(
                        0, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// Returns the arm to its idle state
    /// </summary>
    private void ArmMoveBack()
    {
        for (int i = 0; i < armMoveRotation.Count; i++)
        {
            if (!armMoveUpNow[i])
            {
                if (armMoveRotation[i] > 0)
                {
                    armMove = true;
                    armMoveRotation[i] -= 5;
                    m_tTypeWriterArms[i].localEulerAngles = new Vector3(
                        0, m_tTypeWriterArms[i].localEulerAngles.y, armMoveRotation[i]);
                    m_tTypeWriterKeys[i].localEulerAngles = new Vector3(
                        0, 0, -armMoveRotation[i] / 3);
                }
                else
                {
                    armMoveRotation[i] = 0;
                    m_tTypeWriterArms[i].localEulerAngles = new Vector3(
                        0, m_tTypeWriterArms[i].localEulerAngles.y, 0);
                    m_tTypeWriterKeys[i].localEulerAngles = new Vector3(
                        0, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// Loads the most recently pressed key into memory
    /// </summary>
    private void KeyPress()
    {
        makeRowSound = true;
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

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            updateLetter = "[";
        if (Input.GetKeyDown(KeyCode.RightBracket))
            updateLetter = "]";
        if (Input.GetKeyDown(KeyCode.Backslash))
            updateLetter = "\\";
        if (Input.GetKeyDown(KeyCode.Semicolon))
            updateLetter = ";";
        if (Input.GetKeyDown(KeyCode.Quote))
            updateLetter = "'";
        if (Input.GetKeyDown(KeyCode.Comma))
            updateLetter = ",";
        if (Input.GetKeyDown(KeyCode.Period))
            updateLetter = ".";
        if (Input.GetKeyDown(KeyCode.Slash))
            updateLetter = "/";

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                updateLetter = "!";
            if (Input.GetKeyDown(KeyCode.Alpha2))
                updateLetter = "@";
            if (Input.GetKeyDown(KeyCode.Alpha3))
                updateLetter = "#";
            if (Input.GetKeyDown(KeyCode.Alpha4))
                updateLetter = "$";
            if (Input.GetKeyDown(KeyCode.Alpha5))
                updateLetter = "%";
            if (Input.GetKeyDown(KeyCode.Alpha6))
                updateLetter = "^";
            if (Input.GetKeyDown(KeyCode.Alpha7))
                updateLetter = "&";
            if (Input.GetKeyDown(KeyCode.Alpha8))
                updateLetter = "*";
            if (Input.GetKeyDown(KeyCode.Alpha9))
                updateLetter = "(";
            if (Input.GetKeyDown(KeyCode.Alpha0))
                updateLetter = ")";

            if (Input.GetKeyDown(KeyCode.LeftBracket))
                updateLetter = "{";
            if (Input.GetKeyDown(KeyCode.RightBracket))
                updateLetter = "}";
            if (Input.GetKeyDown(KeyCode.Backslash))
                updateLetter = "|";
            if (Input.GetKeyDown(KeyCode.Semicolon))
                updateLetter = ":";
            if (Input.GetKeyDown(KeyCode.Quote))
                updateLetter = "\"";
            if (Input.GetKeyDown(KeyCode.Comma))
                updateLetter = "<";
            if (Input.GetKeyDown(KeyCode.Period))
                updateLetter = ">";
            if (Input.GetKeyDown(KeyCode.Slash))
                updateLetter = "?";
        }
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

        // Stops the scripts from running to reduce cpu load
        enabled = false;
    }
}