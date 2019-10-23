using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;

public class CloseNotepad : MonoBehaviour {

    [Tooltip ("-1 Closes EVERY Notepad")]
    public int targetWindow;

	// Use this for initialization
	void Awake () {
        DoThing();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoThing()
    {
        int targetCounter = 0;
        foreach (Process p in Process.GetProcessesByName("notepad.exe"))
        {
            if (targetCounter == targetWindow || targetWindow == -1)
                p.CloseMainWindow();
            targetCounter++;
        }
    }
}
