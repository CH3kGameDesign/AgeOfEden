using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Diagnostics;


public class MemoryLeakTest : MonoBehaviour
{
    [System.Serializable]
    public class file
    {
        public string fileName;
        public string fileContents;
        public GameObject activateOnYes;
        public bool removeOnFinish;
        [Tooltip ("False for File Explorer")]
        public bool notepad = true;
        public bool closeFile = false;
    }

    public List<file> m_fileList = new List<file>();

    public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "GetWindowText",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);
    

    private void Start()
    {
        for (int i = 0; i < m_fileList.Count; i++)
        {
            if (m_fileList[i].notepad)
                m_fileList[i].fileName += ".txt - Notepad";
            m_fileList[i].fileContents = m_fileList[i].fileContents.Replace("|",
                    System.Environment.NewLine);
        }
    }

    private void Update()
    {
        
        //var collection = new List<string>();
        EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
        {
            
            StringBuilder strbTitle = new StringBuilder(255);
            int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
            string strTitle = strbTitle.ToString();
            if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
            {
                //collection.Add(strTitle);
                if (strTitle.Contains("Notepad"))
                {
                    for (int i = 0; i < m_fileList.Count; i++)
                    {
                        if (strTitle == m_fileList[i].fileName && m_fileList[i].notepad)
                        {
                            string contents = "";
                            if (m_fileList[i].fileContents != "")
                            {
                                IntPtr windowPtr = ChangeTextRuntimeTest.FindWindowByCaption(IntPtr.Zero, strTitle);
                                IntPtr textWindowPtr = ChangeTextRuntimeTest.FindWindowEx(windowPtr, IntPtr.Zero, "Edit", null);
                                contents = GetControlText(textWindowPtr);
                            }
                            if (contents.Contains(m_fileList[i].fileContents))
                            {
                                m_fileList[i].activateOnYes.SetActive(true);

                                if (m_fileList[i].closeFile)
                                {
                                    IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, m_fileList[i].fileName);
                                    if (windowPtr != IntPtr.Zero)
                                    {
                                        SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    }
                                }
                                if (m_fileList[i].removeOnFinish)
                                    m_fileList.RemoveAt(i);
                            }
                        }
                    }

                }
                for (int i = 0; i < m_fileList.Count; i++)
                {
                    if (!m_fileList[i].notepad)
                    {
                        if (strTitle == m_fileList[i].fileName)
                        {
                            m_fileList[i].activateOnYes.SetActive(true);

                            if (m_fileList[i].closeFile)
                            {
                                IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, m_fileList[i].fileName);
                                if (windowPtr != IntPtr.Zero)
                                {
                                    SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                }
                            }
                            if (m_fileList[i].removeOnFinish)
                                m_fileList.RemoveAt(i);
                        }
                    }
                }
            }
            return true;
        };

        if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
        {
            //
        }
    }

    public string GetControlText(IntPtr hWnd)
    {

        // Get the size of the string required to hold the window title (including trailing null.) 
        Int32 titleSize = SendMessage((int)hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

        // If titleSize is 0, there is no title so return an empty string (or null)
        if (titleSize == 0)
            return "null";

        StringBuilder title = new StringBuilder(titleSize + 1);

        SendMessage(hWnd, (int)WM_GETTEXT, title.Capacity, title);

        return title.ToString();
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
    /// </summary>
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

    const UInt32 WM_CLOSE = 0x0010;
    const UInt32 WM_GETTEXT = 0X000D;
    const int WM_GETTEXTLENGTH = 0x000E;
}
