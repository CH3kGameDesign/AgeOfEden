using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEnding : MonoBehaviour
{
    public bool onAwake;
    public int endingNo;

    public enum option { save, load, delete}

    public option choice;

	// Use this for initialization
	private void Awake()
    {
        if (onAwake)
            DoThing();
	}

    public void DoThing()
    {
        if (PermanentData.saveInfo.endingsAchieved.Count < endingNo + 1)
        {
            for (int i = 0; i < (endingNo + 1) - PermanentData.saveInfo.endingsAchieved.Count; i++)
            {
                PermanentData.saveInfo.endingsAchieved.Add(false);
            }
        }

        if (choice == option.save)
        {
            PermanentData.saveInfo.endingsAchieved[endingNo] = true;
            PermanentData.saveInfo.lastEndingAchieved = endingNo;
            SaveLoad.Save();
        }
        if (choice == option.load)
            SaveLoad.Load();
        if (choice == option.delete)
            SaveLoad.ResetProgress();
    }
}