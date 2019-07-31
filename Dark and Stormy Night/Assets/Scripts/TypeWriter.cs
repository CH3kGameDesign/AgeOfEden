using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class TypeWriter : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
        endingNo = PermanentData.saveInfo.lastEndingAchieved;
        for (int i = 0; i < EndingChanges[endingNo].startLines.Count; i++)
        {
            text.GetChild(i).GetComponent<TextMeshPro>().text = EndingChanges[endingNo].startLines[i];
        }
        row = EndingChanges[endingNo].startLines.Count;

        scriptLines = EndingChanges[endingNo].scriptLines;
        lastLine = EndingChanges[endingNo].lastLine;
        freeCharsBeforeScript = EndingChanges[endingNo].freeCharsBeforeScript;

        if (EndingChanges[endingNo].activateOnStart != null)
            EndingChanges[endingNo].activateOnStart.SetActive(true);

        for (int i = 0; i < 36; i++)
        {
            armMoveRotation.Add(-300);
        }
        letterPerRow = Mathf.RoundToInt((0.166f * (text.GetChild(0).GetComponent<RectTransform>().rect.width * 100))/ text.GetChild(0).GetComponent<TextMeshPro>().fontSize);
        paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
        letterWidth = (halfLength * 2) / letterPerRow;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTyping == true)
        {
            updateLetter = null;
            if (row >= text.childCount)
                return;
            if (text.GetChild(row).GetComponent<TextMeshPro>().text.Length >= letterPerRow)
                NextRow();
            if (row >= text.childCount)
            {
                if (activateOnFinish.Count != 0)
                {
                    for (int i = 0; i < activateOnFinish.Count; i++)
                    {
                        activateOnFinish[i].SetActive(true);
                    }
                }
                if (deActivateOnFinish.Count != 0)
                {
                    for (int i = 0; i < deActivateOnFinish.Count; i++)
                    {
                        deActivateOnFinish[i].SetActive(false);
                    }
                }
                voidOnFinish.Invoke();
                startTyping = false;
                return;
            }
            if (charScriptTakesOverCounter >= freeCharsBeforeScript[scriptLineCounter])
                scriptTyping = true;
            KeyPress();
            paper.transform.localPosition = new Vector3(halfLength - (letterWidth * text.GetChild(row).GetComponent<TextMeshPro>().text.Length), 0.025f * (row + 1), 0);
            if (updateLetter != null)
                Type(updateLetter);
        }
        if (armMove == true)
            ArmMoveBack();
    }






    /// <summary>
    /// /////////////////////////////////////////////////////////////////////
    /// </summary>


    void NextRow ()
    {
        row++;
        if (row != text.childCount)
            paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
    }

    void AnyKeyScript()
    {
        text.GetChild(row).GetComponent<TextMeshPro>().text += scriptLines[scriptLineCounter][scriptCharCounter];
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
                        {
                            activateOnFinish[i].SetActive(true);
                        }
                    }
                    if (deActivateOnFinish.Count != 0)
                    {
                        for (int i = 0; i < deActivateOnFinish.Count; i++)
                        {
                            deActivateOnFinish[i].SetActive(false);
                        }
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

    void Type(string letter)
    {
        string textSound = "";
        if (scriptTyping == true)
        {
            textSound = scriptLines[scriptLineCounter][scriptCharCounter].ToString().ToLower();
            AnyKeyScript();
        }
        else
        {
            textSound = letter;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                letter = letter.ToUpper();
            text.GetChild(row).GetComponent<TextMeshPro>().text += letter;
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

    void ArmMove (string letter)
    {
        int choice = -1;

        if (letter == "a")
            choice = 0;
        if (letter == "b")
            choice = 1;
        if (letter == "c")
            choice = 2;
        if (letter == "d")
            choice = 3;
        if (letter == "e")
            choice = 4;
        if (letter == "f")
            choice = 5;
        if (letter == "g")
            choice = 6;
        if (letter == "h")
            choice = 7;
        if (letter == "i")
            choice = 8;
        if (letter == "j")
            choice = 9;
        if (letter == "k")
            choice = 10;
        if (letter == "l")
            choice = 11;
        if (letter == "m")
            choice = 12;
        if (letter == "n")
            choice = 13;
        if (letter == "o")
            choice = 14;
        if (letter == "p")
            choice = 15;
        if (letter == "q")
            choice = 16;
        if (letter == "r")
            choice = 17;
        if (letter == "s")
            choice = 18;
        if (letter == "t")
            choice = 19;
        if (letter == "u")
            choice = 20;
        if (letter == "v")
            choice = 21;
        if (letter == "w")
            choice = 22;
        if (letter == "x")
            choice = 23;
        if (letter == "y")
            choice = 24;
        if (letter == "z")
            choice = 25;
        if (letter == "0")
            choice = 26;
        if (letter == "1")
            choice = 27;
        if (letter == "2")
            choice = 28;
        if (letter == "3")
            choice = 29;
        if (letter == "4")
            choice = 30;
        if (letter == "5")
            choice = 31;
        if (letter == "6")
            choice = 32;
        if (letter == "7")
            choice = 33;
        if (letter == "8")
            choice = 34;
        if (letter == "9")
            choice = 35;
        if (letter == ".")
            choice = 28;

        if (choice != -1)
        {
            arms[choice].GetChild(0).localEulerAngles = new Vector3(0, 0, -130);
            armMove = true;
            armMoveRotation[choice] = -130;
        }
    }

    void ArmMoveBack ()
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


    void KeyPress ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            updateLetter = " ";
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            updateLetter = "a";
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            updateLetter = "b";
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            updateLetter = "c";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            updateLetter = "d";
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            updateLetter = "e";
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            updateLetter = "f";
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            updateLetter = "g";
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            updateLetter = "h";
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            updateLetter = "i";
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            updateLetter = "j";
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            updateLetter = "k";
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            updateLetter = "l";
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            updateLetter = "m";
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            updateLetter = "n";
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            updateLetter = "o";
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            updateLetter = "p";
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            updateLetter = "q";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            updateLetter = "r";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            updateLetter = "s";
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            updateLetter = "t";
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            updateLetter = "u";
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            updateLetter = "v";
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            updateLetter = "w";
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            updateLetter = "x";
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            updateLetter = "y";
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            updateLetter = "z";
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            updateLetter = "1";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            updateLetter = "2";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            updateLetter = "3";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            updateLetter = "4";
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            updateLetter = "5";
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            updateLetter = "6";
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            updateLetter = "7";
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            updateLetter = "8";
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            updateLetter = "9";
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            updateLetter = "0";
        }
    }
}
