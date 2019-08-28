using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateTextFile : MonoBehaviour
{
    private KeyCode m_kcResetFirstTime = KeyCode.PageDown;
    [SerializeField]
    private string m_sFileName = "Surprise";
    [SerializeField]
    private string m_sMessage = "You're the one that broke";

	// Use this for initialization
	private void Start()
    {
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
            + "/" + m_sFileName +".txt";

        
        if (GetPermanentStorage())
        {
            //Debug.Log("First message");
            WriteMessage(path, m_sMessage);
            SetPermanentStorage(false);
        }
        else
        {
            if (DoesFileExist(path))
            {
                //Debug.Log("Regular message");
                WriteMessage(path, m_sMessage);
            }
            else
            {
                //Debug.Log("File doesnt exist");
                //Debug.Log("Deleted message");
                WriteMessage(path, "You cannot get rid of me");
                WriteOnNewLine(path, m_sMessage);
            }
        }
    }

    private void Update()
    {
        // Resets the first time bool
        if (Input.GetKeyDown(m_kcResetFirstTime))
            SetPermanentStorage(true);
    }

    // Checks if a certain file paht returns valid
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
	
    // Writes a message into a file
    private void WriteMessage(string pPath, string pMessage)
    {
        // Rewrites the file with new message
        StreamWriter writer = new StreamWriter(pPath);
        writer.WriteLine(pMessage);
        writer.Close();
    }

    // Writes a message onto the next line of a file
    private void WriteOnNewLine(string pPath, string pMessage)
    {
        // Writes the message on a new file line
        StreamWriter writer = new StreamWriter(pPath, true);
        writer.WriteLine(writer.NewLine + pMessage);
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