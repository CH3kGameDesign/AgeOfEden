using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
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
    public void CloseSpecificWindow (string win)
    {
        IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, win);
        if (windowPtr != IntPtr.Zero)
        {
            SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    const UInt32 WM_CLOSE = 0x0010;
}
