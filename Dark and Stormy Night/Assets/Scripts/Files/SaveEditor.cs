using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveEditor : MonoBehaviour
{
    public List<Text> buttons = new List<Text>();
    public TextMeshProUGUI lastEnding;

    public bool PAXPresent;

	// Use this for initialization
	private void Start()
    {
        if (PAXPresent)
            PAXDisplay();
        else
            UpdateDisplay();
    }
	
	// Update is called once per frame
	private void Update()
    {
        if (!PAXPresent)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
	}

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < PermanentData.saveInfo.endingsAchieved.Count; i++)
        {
            if (i < buttons.Count)
            {
                if (PermanentData.saveInfo.endingsAchieved[i])
                    buttons[i].text = "O";
                else
                    buttons[i].text = "";
            }
        }
        lastEnding.text = PermanentData.saveInfo.lastEndingAchieved.ToString();
    }

    public void ChangeValues(int choice)
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
            PermanentData.saveInfo.endingsAchieved[choice] = !PermanentData.saveInfo.endingsAchieved[choice];
            if (PermanentData.saveInfo.endingsAchieved[choice] == true)
                PermanentData.saveInfo.lastEndingAchieved = choice;
            else
                PermanentData.saveInfo.lastEndingAchieved = 0;
        }
        UpdateDisplay();
        SaveLoad.Save();
    }

    public void PAXDisplay()
    {
        if (PermanentData.saveInfo.endingsAchieved.Count < 10)
        {
            for (int i = PermanentData.saveInfo.endingsAchieved.Count; i < 3; i++)
                PermanentData.saveInfo.endingsAchieved.Add(false);
        }
        PermanentData.saveInfo.endingsAchieved[0] = true;
        PermanentData.saveInfo.endingsAchieved[1] = false;
        PermanentData.saveInfo.endingsAchieved[2] = false;
        PermanentData.saveInfo.endingsAchieved[3] = false;
        PermanentData.saveInfo.endingsAchieved[4] = false;
        PermanentData.saveInfo.endingsAchieved[5] = false;
        PermanentData.saveInfo.endingsAchieved[6] = false;
        PermanentData.saveInfo.endingsAchieved[7] = false;
        PermanentData.saveInfo.endingsAchieved[8] = false;
        PermanentData.saveInfo.endingsAchieved[9] = false;
        PermanentData.saveInfo.lastEndingAchieved = 0;
        SaveLoad.Save();
    }
}