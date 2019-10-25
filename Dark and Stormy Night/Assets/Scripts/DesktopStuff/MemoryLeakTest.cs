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
