using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics;


public class KeepFolderOpen : MonoBehaviour
{
    public List<string> m_folderList = new List<string>();
    public float waitBeforePopUp = 1;
    private float waitTimer = 0;

    public UnityEvent activateOnClose = new UnityEvent();

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

    }

    private void Update()
    {
        waitTimer += Time.deltaTime;
        //var collection = new List<string>();
        EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
        {
            
            StringBuilder strbTitle = new StringBuilder(255);
            int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
            string strTitle = strbTitle.ToString();
            if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
            {
                for (int i = 0; i < m_folderList.Count; i++)
                {

                    if (strTitle == m_folderList[i])
                    {
                        waitTimer = 0;
                    }

                }
            }
            return true;
        };

        if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
        {
            //
        }
        if (waitTimer > waitBeforePopUp)
        {
            activateOnClose.Invoke();
            waitTimer = 0;
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
