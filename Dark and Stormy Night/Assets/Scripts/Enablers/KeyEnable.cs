using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyEnable : MonoBehaviour
{
    public string keyPress;
    public bool anyKey = false;
    public List<GameObject> enableObjects = new List<GameObject>();
    public List<GameObject> disableObjects = new List<GameObject>();
    public bool toggle = false;
    public UnityEvent activateOnPress = new UnityEvent();

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
            CheckPress();
    }

    /// <summary>
    /// Enables or disables a list of objects when called based on bool state
    /// </summary>
    private void DoThing()
    {
        activateOnPress.Invoke();
        if (toggle)
        {
            for (int i = 0; i < enableObjects.Count; i++)
                enableObjects[i].SetActive(!enableObjects[i].activeSelf);
            for (int i = 0; i < disableObjects.Count; i++)
                disableObjects[i].SetActive(!disableObjects[i].activeSelf);
        }
        else
        {
            for (int i = 0; i < enableObjects.Count; i++)
                enableObjects[i].SetActive(true);
            for (int i = 0; i < disableObjects.Count; i++)
                disableObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Oh god oh fuck
    /// </summary>
    private void CheckPress()
    {
        if (anyKey)
            DoThing();
        if (keyPress == "")
            DoThing();
        if (Input.GetKeyDown(KeyCode.A) && keyPress.ToUpper() == "A")
            DoThing();
        if (Input.GetKeyDown(KeyCode.B) && keyPress.ToUpper() == "B")
            DoThing();
        if (Input.GetKeyDown(KeyCode.C) && keyPress.ToUpper() == "C")
            DoThing();
        if (Input.GetKeyDown(KeyCode.D) && keyPress.ToUpper() == "D")
            DoThing();
        if (Input.GetKeyDown(KeyCode.E) && keyPress.ToUpper() == "E")
            DoThing();
        if (Input.GetKeyDown(KeyCode.F) && keyPress.ToUpper() == "F")
            DoThing();
        if (Input.GetKeyDown(KeyCode.G) && keyPress.ToUpper() == "G")
            DoThing();
        if (Input.GetKeyDown(KeyCode.H) && keyPress.ToUpper() == "H")
            DoThing();
        if (Input.GetKeyDown(KeyCode.I) && keyPress.ToUpper() == "I")
            DoThing();
        if (Input.GetKeyDown(KeyCode.J) && keyPress.ToUpper() == "J")
            DoThing();
        if (Input.GetKeyDown(KeyCode.K) && keyPress.ToUpper() == "K")
            DoThing();
        if (Input.GetKeyDown(KeyCode.L) && keyPress.ToUpper() == "L")
            DoThing();
        if (Input.GetKeyDown(KeyCode.M) && keyPress.ToUpper() == "M")
            DoThing();
        if (Input.GetKeyDown(KeyCode.N) && keyPress.ToUpper() == "N")
            DoThing();
        if (Input.GetKeyDown(KeyCode.O) && keyPress.ToUpper() == "O")
            DoThing();
        if (Input.GetKeyDown(KeyCode.P) && keyPress.ToUpper() == "P")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Q) && keyPress.ToUpper() == "Q")
            DoThing();
        if (Input.GetKeyDown(KeyCode.R) && keyPress.ToUpper() == "R")
            DoThing();
        if (Input.GetKeyDown(KeyCode.S) && keyPress.ToUpper() == "S")
            DoThing();
        if (Input.GetKeyDown(KeyCode.T) && keyPress.ToUpper() == "T")
            DoThing();
        if (Input.GetKeyDown(KeyCode.U) && keyPress.ToUpper() == "U")
            DoThing();
        if (Input.GetKeyDown(KeyCode.V) && keyPress.ToUpper() == "V")
            DoThing();
        if (Input.GetKeyDown(KeyCode.W) && keyPress.ToUpper() == "W")
            DoThing();
        if (Input.GetKeyDown(KeyCode.X) && keyPress.ToUpper() == "X")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Y) && keyPress.ToUpper() == "Y")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Z) && keyPress.ToUpper() == "Z")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Space) && keyPress.ToUpper() == " ")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha1) && keyPress.ToUpper() == "1")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha2) && keyPress.ToUpper() == "2")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha3) && keyPress.ToUpper() == "3")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha4) && keyPress.ToUpper() == "4")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha5) && keyPress.ToUpper() == "5")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha6) && keyPress.ToUpper() == "6")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha7) && keyPress.ToUpper() == "7")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha8) && keyPress.ToUpper() == "8")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha9) && keyPress.ToUpper() == "9")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Alpha0) && keyPress.ToUpper() == "0")
            DoThing();
        if (Input.GetKeyDown(KeyCode.Delete) && keyPress.ToUpper() == "DELETE")
            DoThing();
    }
}