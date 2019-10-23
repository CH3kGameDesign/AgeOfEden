using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;

public class HideAndOpen : MonoBehaviour {

    public bool activateOnStart;
    public bool hide = true;
    public bool openDirectory = false;
    public int openAmount = 1;
    private string fileName;
    private string fileDirectory;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;

    // Use this for initialization
    void Start () {
        Application.runInBackground = true;
        if (activateOnStart)
            Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide()
    {
        string path = "";
        for (int j = 0; j < openAmount; j++)
        {
            for (int i = 0; i < GetComponent<CreateTextFile>().m_oOutputs.Length; i++)
            {
                if (GetComponent<CreateTextFile>().m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Desktop)
                    path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                        + "\\";
                else if (GetComponent<CreateTextFile>().m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Documents)
                    path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                        + "\\";
                else if (GetComponent<CreateTextFile>().m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Music)
                    path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)
                        + "\\";
                else if (GetComponent<CreateTextFile>().m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Pictures)
                    path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)
                        + "\\";
                for (int k = 0; k < GetComponent<CreateTextFile>().m_oOutputs[i].folderPath.Count; k++)
                {
                    path += GetComponent<CreateTextFile>().m_oOutputs[i].folderPath[k];
                    path += "\\";
                }
                if (!openDirectory)
                    path += GetComponent<CreateTextFile>().m_oOutputs[i].m_sFileName + ".txt";
                
                if (File.Exists(path) && !openDirectory)
                    Application.OpenURL(path);
                if (Directory.Exists(path) && openDirectory)
                    Application.OpenURL(path);
            }
        }


        if (hide)
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_HIDE);
        }
    }
}
