using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour {

    public Transform paper;
    public Transform text;

    public float halfLength;
    private float letterWidth;

    public int row;

    private int letterPerRow;

    [Header("Strings")]
    public List<string> scriptLines = new List<string>();
    [Space(10)]
    public int lineScriptTakesOver;
    private bool scriptTyping = false;
    private int scriptCharCounter = 0;

	// Use this for initialization
	void Start () {
        letterPerRow = Mathf.RoundToInt((0.166f * (text.GetChild(0).GetComponent<RectTransform>().rect.width * 100))/ text.GetChild(0).GetComponent<TextMeshPro>().fontSize);
        paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
        letterWidth = (halfLength * 2) / letterPerRow;
    }

    // Update is called once per frame
    void Update()
    {
        if (row >= text.childCount)
            return;
        if (text.GetChild(row).GetComponent<TextMeshPro>().text.Length >= letterPerRow)
            NextRow();
        if (row >= text.childCount)
            return;
        if (lineScriptTakesOver == row)
            scriptTyping = true;
        KeyPress();
        paper.transform.localPosition = new Vector3(halfLength - (letterWidth * text.GetChild(row).GetComponent<TextMeshPro>().text.Length), 0.025f * (row + 1), 0);
    }






    /// <summary>
    /// /////////////////////////////////////////////////////////////////////
    /// </summary>


    void NextRow ()
    {
        row++;
        paper.transform.localPosition = new Vector3(halfLength, 0.025f * (row + 1), 0);
    }

    void AnyKeyScript()
    {
        text.GetChild(row).GetComponent<TextMeshPro>().text += scriptLines[0][scriptCharCounter];
        Debug.Log(scriptCharCounter);
        scriptCharCounter++;
    }

    void Type(string letter)
    {
        if (scriptTyping == true)
            AnyKeyScript();
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                letter = letter.ToUpper();
            text.GetChild(row).GetComponent<TextMeshPro>().text += letter;
        }
    }


    void KeyPress ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Type(" ");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Type("a");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Type("b");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Type("c");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Type("d");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Type("e");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Type("f");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Type("g");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Type("h");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Type("i");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Type("j");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Type("k");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Type("l");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Type("m");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Type("n");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Type("o");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Type("p");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Type("q");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Type("r");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Type("s");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Type("t");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Type("u");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Type("v");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Type("w");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Type("x");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Type("y");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Type("z");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Type("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Type("2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Type("3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Type("4");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Type("5");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Type("6");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Type("7");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Type("8");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Type("9");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Type("0");
        }
    }
}
