using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Diagnostics;


public class ChangeTextRuntimeTest : MonoBehaviour
{
    public string m_fileName;
    public string m_message;
    public TextAsset fileToCopy;

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
        m_fileName += ".txt - Notepad";
        if (fileToCopy != null)
            m_message = fileToCopy.text;
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
                if (strTitle == m_fileName)
                {
                    IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, m_fileName);
                    IntPtr windowPtrTextArea = FindWindowEx(windowPtr, IntPtr.Zero, "Edit", null);
                    if (windowPtr != IntPtr.Zero)
                    {
                        SendMessage(windowPtrTextArea, WM_SETTITLETEXT, 0, m_message);
                    }
                    GetComponent<ChangeTextRuntimeTest>().enabled = false;

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

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    const UInt32 WM_CLOSE = 0x0010;
    const int WM_SETTITLETEXT = 0X000C;
    const int WM_GETTEXT = 0X000D;
}
