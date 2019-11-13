using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class JumpBackIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.runInBackground = false;
        ShowWindow(HideAndOpen.editorWindow, SW_SHOW);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;
}
