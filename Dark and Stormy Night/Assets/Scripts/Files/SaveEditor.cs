using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveEditor : MonoBehaviour {

    public List<Text> buttons = new List<Text>();
    public TextMeshProUGUI lastEnding;

	// Use this for initialization
	void Start () {
        UpdateDisplay();
	}
	
	// Update is called once per frame
	void Update () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateDisplay ()
    {
        for (int i = 0; i < PermanentData.saveInfo.endingsAchieved.Count; i++)
        {
            if (PermanentData.saveInfo.endingsAchieved[i])
                buttons[i].text = "O";
            else
                buttons[i].text = "";
        }
        lastEnding.text = PermanentData.saveInfo.lastEndingAchieved.ToString();
    }

    public void ChangeValues (int choice)
    {
        if (choice == -1)
        {
            for (int i = 0; i < PermanentData.saveInfo.endingsAchieved.Count; i++)
                PermanentData.saveInfo.endingsAchieved[i] = true;
            PermanentData.saveInfo.lastEndingAchieved = PermanentData.saveInfo.endingsAchieved.Count;
        }
        if (choice == -2)
        {
            for (int i = 0; i < PermanentData.saveInfo.endingsAchieved.Count; i++)
                PermanentData.saveInfo.endingsAchieved[i] = false;
            PermanentData.saveInfo.lastEndingAchieved = 0;
        }
        if (choice >= 0)
        {
            if (PermanentData.saveInfo.endingsAchieved.Count < choice + 1)
            {
                for (int i = 0; i < (choice + 1) - PermanentData.saveInfo.endingsAchieved.Count; i++)
                {
                    PermanentData.saveInfo.endingsAchieved.Add(false);
                }
            }

            PermanentData.saveInfo.endingsAchieved[choice] = !PermanentData.saveInfo.endingsAchieved[choice];
            if (PermanentData.saveInfo.endingsAchieved[choice] == true)
                PermanentData.saveInfo.lastEndingAchieved = choice;
            else
                PermanentData.saveInfo.lastEndingAchieved = 0;
        }
        UpdateDisplay();
        SaveLoad.Save();
    }
}
