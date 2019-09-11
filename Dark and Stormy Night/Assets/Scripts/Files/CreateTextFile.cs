using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateTextFile : MonoBehaviour
{
    private readonly KeyCode m_kcResetFirstTime = KeyCode.PageDown;

    public enum Location
    {
        Desktop,
        Documents,
        Music,
        Pictures
    }

    public enum State
    {
        First,
        Create,
        Standard
    }

    [System.Serializable]
    public class Outputs
    {
        [Tooltip("The output location")]
        public Location m_olOutputLocation = Location.Desktop;
        [Tooltip("How the file will be created and dealt with, ordering must be: First, Create, Standard")]
        public State m_osOutputState = State.Standard;
        [Space(3)]
        [Tooltip("Only has an impact on standard outputs")]
        public bool m_bOverwrite = true;
        [Tooltip("Whether the message is replaced with the typewritter text")]
        public bool m_bUseTypeWriter = false;
        [Space(3)]
        [Tooltip("The desired file name (file will always be a text document)")]
        public string m_sFileName = "I forgot a file name";
        [Tooltip("The message printed into the text file")]
        [TextArea]
        public string m_sMessage = @"I forgot a message";
    }

    // Prevents standard output from overwriting create output in same generation
    private bool m_bRecreated = false;

    [Tooltip("The char that will be used to signify an enter press")]
    [SerializeField]
    private char m_cEnterChar = '|';

    [Tooltip("A resizable list of outputs editable in the inspector, outputs are done in ascending order")]
    public Outputs[] m_oOutputs;

    private static string m_sTypeWritterMessage;

    public static void SetMessage(List<string> pLines)
    {
        for (int j = 0; j < pLines.Count; j++)
        {
            m_sTypeWritterMessage += pLines[j] + "|";
        }
    }

    // Called once before the first frame
    private void Start()
    {
        // Verifys the list is not emtpy
        if (m_oOutputs.Length == 0)
            return;

        // Initialises the file path
        string path = "";

        for (int i = 0; i < m_oOutputs.Length; i++)
        {
            if (m_oOutputs[i].m_bUseTypeWriter)
                m_oOutputs[i].m_sMessage = m_sTypeWritterMessage;

            m_oOutputs[i].m_sMessage = m_oOutputs[i].m_sMessage.Replace(m_cEnterChar.ToString(),
                System.Environment.NewLine);

            // Sorts the message to a desired location
            if (m_oOutputs[i].m_olOutputLocation == Location.Desktop)
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                    + "\\" + m_oOutputs[i].m_sFileName + ".txt";
            else if (m_oOutputs[i].m_olOutputLocation == Location.Documents)
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                    + "\\" + m_oOutputs[i].m_sFileName + ".txt";
            else if (m_oOutputs[i].m_olOutputLocation == Location.Music)
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)
                    + "\\" + m_oOutputs[i].m_sFileName + ".txt";
            else if (m_oOutputs[i].m_olOutputLocation == Location.Pictures)
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)
                    + "\\" + m_oOutputs[i].m_sFileName + ".txt";

            // 1. A message displayed the first time the game is run and a certain ending is reached
            // 2. A message displayed when no message file is found, either deleted or never created
            // 3. A message displayed as a standard output
            
            if (m_oOutputs[i].m_osOutputState == State.First && GetPermanentStorage())
            {
                //Debug.Log("First message");
                WriteMessage(path, m_oOutputs[i].m_sMessage);
                SetPermanentStorage(false);
            }
            else if (m_oOutputs[i].m_osOutputState == State.Create && !DoesFileExist(path))
            {
                //Debug.Log("Created message");
                WriteMessage(path, m_oOutputs[i].m_sMessage);
                m_bRecreated = true;
            }
            else if (m_oOutputs[i].m_osOutputState == State.Standard && !m_bRecreated)
            {
                //Debug.Log("Regular message");
                if (m_oOutputs[i].m_bOverwrite)
                    WriteMessage(path, m_oOutputs[i].m_sMessage);
                else
                    WriteOnNewLine(path, m_oOutputs[i].m_sMessage);
            }
        }
    }

    // Called once per frame
    private void Update()
    {
        // Resets the first time bool
        if (Input.GetKeyDown(m_kcResetFirstTime))
            SetPermanentStorage(true);
    }

    /// <summary>
    /// Checks if a certain file path returns valid
    /// </summary>
    /// <param name="pPath">The file path checked</param>
    /// <returns>The status of the file path</returns>
    private bool DoesFileExist(string pPath)
    {
        // Tries to open the file and returns the success
        try
        {
            StreamReader reader = new StreamReader(pPath);
            string test = reader.ReadToEnd();
            reader.Close();
            if (test == null)
                return false;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Writes a message into a file
    /// </summary>
    /// <param name="pPath">The location written to</param>
    /// <param name="pMessage">The message written to the file</param>
    private void WriteMessage(string pPath, string pMessage)
    {
        // Rewrites the file with new message
        StreamWriter writer = new StreamWriter(pPath, false);
        writer.WriteLine(pMessage);
        writer.Close();
    }

    /// <summary>
    /// Writes a message onto the next line of a file
    /// </summary>
    /// <param name="pPath">The location written to</param>
    /// <param name="pMessage">The message appended to the file</param>
    private void WriteOnNewLine(string pPath, string pMessage)
    {
        // Writes the message on a new file line
        StreamWriter writer = new StreamWriter(pPath, true);
        writer.WriteLine(pMessage);
        writer.Close();
    }

    /// <summary>
    /// Sets the desired state of the permanent storage for firstTime
    /// </summary>
    /// <param name="pState">The desired state of the bool</param>
    private void SetPermanentStorage(bool pState)
    {
        PermanentData.saveInfo.firstTime = pState;
    }

    /// <summary>
    /// Returns the saved data of firstTime
    /// </summary>
    private bool GetPermanentStorage()
    {
        return PermanentData.saveInfo.firstTime;
    }
}