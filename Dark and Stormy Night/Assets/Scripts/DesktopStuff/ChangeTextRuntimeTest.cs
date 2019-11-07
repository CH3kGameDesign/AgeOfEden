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
    public string m_enterChar = "|";

    [Serializable]
    public class textObject
    {
        public string m_message;
        public TextAsset fileToCopy;
        [Space (10)]
        public float m_waitTime;
        public bool replace;
        [Tooltip("0 For All Text At Once")]
        public float oneByOneTimer = 0;
        public int lettersAtATime = 1;
        public bool changeTitle;
    }
    
    public List<textObject> textList = new List<textObject>();
    
    public bool closeOnFileOnFinal;
    public GameObject activateOnFinish;

    private int textLineCounter;
    private float textTimer;

    private int charLineCounter;
    private float charTimer;

    private IntPtr windowPtr = new IntPtr();
    private IntPtr textWindowPtr = new IntPtr();

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
        for (int i = 0; i < textList.Count; i++)
        {
            if (textList[i].fileToCopy != null)
                textList[i].m_message = textList[i].fileToCopy.text;
            textList[i].m_message = textList[i].m_message.Replace(m_enterChar,
                    System.Environment.NewLine);
        }
        
    }

    private void Awake ()
    {
        windowPtr = IntPtr.Zero;
        textWindowPtr = IntPtr.Zero;
    }

    private void Update()
    {
        if (windowPtr != IntPtr.Zero)
        {
            if (textTimer >= textList[textLineCounter].m_waitTime)
            {
                if (textList[textLineCounter].oneByOneTimer == 0)
                {
                    if (textList[textLineCounter].replace == false)
                        textList[textLineCounter].m_message = GetControlText(textWindowPtr) + textList[textLineCounter].m_message;
                    if (textList[textLineCounter].changeTitle)
                        SendMessage(windowPtr, WM_SETTEXT, 0, textList[textLineCounter].m_message);
                    else
                        SendMessage(textWindowPtr, WM_SETTEXT, 0, textList[textLineCounter].m_message);
                    textLineCounter++;
                    textTimer = 0;
                    
                }
                else
                {
                    if (charTimer >= textList[textLineCounter].oneByOneTimer)
                    {
                        int lettersAtATime = textList[textLineCounter].lettersAtATime;
                        if (charLineCounter + lettersAtATime > textList[textLineCounter].m_message.Length)
                            lettersAtATime = textList[textLineCounter].m_message.Length - charLineCounter;
                        string currentLetter = textList[textLineCounter].m_message.Substring(charLineCounter, lettersAtATime);
                        if (charLineCounter == 0)
                        {
                            
                            if (textList[textLineCounter].changeTitle)
                            {
                                if (textList[textLineCounter].replace == false)
                                    currentLetter = GetControlText(windowPtr) + currentLetter;
                                SendMessage(windowPtr, WM_SETTEXT, 0, currentLetter);
                            }
                            else
                            {
                                if (textList[textLineCounter].replace == false)
                                    currentLetter = GetControlText(textWindowPtr) + currentLetter;
                                SendMessage(textWindowPtr, WM_SETTEXT, 0, currentLetter);
                            }
                        }
                        else
                        {
                            
                            if (textList[textLineCounter].changeTitle)
                            {
                                currentLetter = GetControlText(windowPtr) + currentLetter;
                                SendMessage(windowPtr, WM_SETTEXT, 0, currentLetter);
                            }
                            else
                            {
                                currentLetter = GetControlText(textWindowPtr) + currentLetter;
                                SendMessage(textWindowPtr, WM_SETTEXT, 0, currentLetter);
                            }
                        }
                        charTimer = 0;
                        charLineCounter += lettersAtATime;
                        if (charLineCounter >= textList[textLineCounter].m_message.Length)
                        {
                            textLineCounter++;
                            textTimer = 0;
                            charLineCounter = 0;
                        }
                    }
                    
                }
                if (textLineCounter >= textList.Count)
                {
                    if (closeOnFileOnFinal)
                        SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    if (activateOnFinish != null)
                        activateOnFinish.SetActive(true);
                    GetComponent<ChangeTextRuntimeTest>().enabled = false;
                }
            }
            textTimer += Time.deltaTime;
            charTimer += Time.deltaTime;
        }
        else
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
                        windowPtr = FindWindowByCaption(IntPtr.Zero, m_fileName);
                        textWindowPtr = FindWindowEx(windowPtr, IntPtr.Zero, "Edit", null);
                    }
                }
                return true;
            };


            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                //
            }
        }
    }

    public string GetControlText(IntPtr hWnd)
    {

        // Get the size of the string required to hold the window title (including trailing null.) 
        Int32 titleSize = SendMessage((int)hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

        // If titleSize is 0, there is no title so return an empty string (or null)
        if (titleSize == 0)
            return String.Empty;

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
    public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    const UInt32 WM_CLOSE = 0x0010;
    const int WM_SETTEXT = 0X000C;
    const UInt32 WM_GETTEXT = 0X000D;
    const int WM_GETTEXTLENGTH = 0x000E;
}
