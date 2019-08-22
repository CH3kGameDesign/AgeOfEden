using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class TypeWriter : MonoBehaviour
{
    public Transform paper;
    public Transform text;
    public List<Transform> arms = new List<Transform>();
    private List<float> armMoveRotation = new List<float>();
    private bool armMove = false;

    public float halfLength;
    private float letterWidth;

    public int row;

    public bool startTyping = true;
    public bool returnToTyping = false;
    public List<GameObject> activateOnFinish = new List<GameObject>();
    public List<GameObject> deActivateOnFinish = new List<GameObject>();
    public UnityEvent voidOnFinish;

    private int letterPerRow;

    private int endingNo;
    
    [Serializable]
    public class ending
    {
        public List<string> startLines = new List<string>();
        public List<string> scriptLines = new List<string>();
        public List<int> freeCharsBeforeScript = new List<int>();
        public GameObject activateOnStart;
        public string lastLine;
    }

    [SerializeField]
    public List<ending> EndingChanges = new List<ending>();

    private List<string> scriptLines = new List<string>();
    private string lastLine;
    private List<int> freeCharsBeforeScript;

    [Space(20)]
    public List<GameObject> clickSounds = new List<GameObject>();
    public GameObject spaceSound;
    public GameObject exitSound;

    private int scriptLineCounter = 0;
    private int charScriptTakesOverCounter;
    
    private bool scriptTyping = false;
    private int scriptCharCounter = 0;
    private string updateLetter;

	// Called once before the first frame
	private void Start()
    {
        endingNo = PermanentData.saveInfo.lastEndingAchieved;

        for (int i = 0; i < EndingChanges[endingNo].startLines.Count; i++)
            text.GetChild(i).GetComponent<TextMeshPro>().text =
                EndingChanges[endingNo].startLines[i];

        row = EndingChanges[endingNo].startLines.Count;

        scriptLines = EndingChanges[endingNo].scriptLines;
        lastLine = EndingChanges[endingNo].lastLine;
        freeCharsBeforeScript = EndingChanges[endingNo].freeCharsBeforeScript;

        if (EndingChanges[endingNo].activateOnStart)
            EndingChanges[endingNo].activateOnStart.SetActive(true);

        for (int i = 0; i < 36; i++)
            armMoveRotation.Add(-300);

        letterPerRow = Mathf.RoundToInt(0.166f *
            (text.GetChild(0).GetComponent<RectTransform>().rect.width * 100)
            / text.GetChild(0).GetComponent<TextMeshPro>().fontSize);

        paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
        letterWidth = (halfLength * 2) / letterPerRow;
    }

    // Update is called once per frame
    private void Update()
    {
        if (startTyping && SceneManager.sceneCount == 1)
        {
            updateLetter = null;

            if (row >= text.childCount)
                return;

            if (text.GetChild(row).GetComponent<TextMeshPro>().text.Length >= letterPerRow)
                NextRow();

            // Once max rows have been reached, end typewriter sequence
            if (row >= text.childCount)
            {
                if (activateOnFinish.Count != 0)
                {
                    for (int i = 0; i < activateOnFinish.Count; i++)
                        activateOnFinish[i].SetActive(true);
                }
                if (deActivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < deActivateOnFinish.Count; i++)
                        deActivateOnFinish[i].SetActive(false);
                }
                voidOnFinish.Invoke();
                startTyping = false;
                return;
            }

            if (charScriptTakesOverCounter >= freeCharsBeforeScript[scriptLineCounter])
                scriptTyping = true;

            KeyPress();
            paper.transform.localPosition = new Vector3(halfLength - (letterWidth *
                text.GetChild(row).GetComponent<TextMeshPro>().text.Length),
                0.025f * (row + 1), 0);

            if (updateLetter != null)
                Type(updateLetter);
        }
        if (armMove)
            ArmMoveBack();
    }
    
    /// <summary>
    /// Moves over to the next row of the paper
    /// </summary>
    private void NextRow()
    {
        row++;
        if (row != text.childCount)
            paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
    }

    /// <summary>
    /// Function for when the script takes control of whats being typed
    /// </summary>
    private void AnyKeyScript()
    {
        text.GetChild(row).GetComponent<TextMeshPro>().text +=
            scriptLines[scriptLineCounter][scriptCharCounter];

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
            else
            {
                if (returnToTyping == true)
                {
                    charScriptTakesOverCounter = -1000;
                    scriptTyping = false;
                }
                else
                {
                    if (activateOnFinish.Count != 0)
                    {
                        for (int i = 0; i < activateOnFinish.Count; i++)
                            activateOnFinish[i].SetActive(true);
                    }
                    if (deActivateOnFinish.Count != 0)
                    {
                        for (int i = 0; i < deActivateOnFinish.Count; i++)
                            deActivateOnFinish[i].SetActive(false);
                    }
                    NextRow();
                    NextRow();
                    text.GetChild(row).GetComponent<TextMeshPro>().text += lastLine;
                    voidOnFinish.Invoke();
                    startTyping = false;
                    Instantiate(exitSound, transform.position, transform.rotation);
                }
            }
        }
    }

    /// <summary>
    /// Enters the desired letter onto the paper in game
    /// </summary>
    /// <param name="pLetter">The letter used</param>
    private void Type(string pLetter)
    {
        string textSound = "";
        if (scriptTyping == true)
        {
            textSound = scriptLines[scriptLineCounter][scriptCharCounter].ToString().ToLower();
            AnyKeyScript();
        }
        else
        {
            textSound = pLetter;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                pLetter = pLetter.ToUpper();

            text.GetChild(row).GetComponent<TextMeshPro>().text += pLetter;
            charScriptTakesOverCounter++;
        }

        int ranInt = UnityEngine.Random.Range(0, clickSounds.Count);

        if (textSound != " ")
        {
            ArmMove(textSound);
            Instantiate(clickSounds[ranInt], transform.position, transform.rotation);
        }
        else
            Instantiate(spaceSound, transform.position, transform.rotation);
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
            arms[choice].GetChild(0).localEulerAngles = new Vector3(0, 0, -130);
            armMove = true;
            armMoveRotation[choice] = -130;
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
            if (armMoveRotation[i] != -300)
            {
                armMove = true;
                armMoveRotation[i] -= 10;
                arms[i].GetChild(0).localEulerAngles = new Vector3(0, 0, armMoveRotation[i]);
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
}