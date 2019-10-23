using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class LoadingText : MonoBehaviour {

    public float timeBetween;
    public float timeOnLast;
    private float timer;

    private int percent;

    public GameObject activateOnFinish;

    [TextArea]
    public string m_sFinalMessage = @"I forgot a message";

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (timer > timeBetween)
        {
            string updateText = "";
            if (percent < 100)
                timer = 0;
            if (percent < 100)
            {
                percent += Random.Range(1, 2);
                if (percent > 100)
                    percent = 100;
            }
            else
                percent = 101;
            if (percent < 100)
            {

                string st = "1234567890-=qwertyuiop[]\asdfghjkl;';zxcvbnm,./!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?";
                for (int j = 0; j < 8; j++)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        updateText += st[Random.Range(0, st.Length)];
                    }
                    updateText += System.Environment.NewLine;
                    updateText += System.Environment.NewLine;
                }
                
                if (percent < 100)
                {
                    if (percent < 10)
                    {
                        updateText += "- - - - - - - - - - - - - - - - - - - - - - - - " + percent + "  - - - - - - - - - - - - - - - - - - - - - - -";
                    }
                    else
                    {
                        updateText += "- - - - - - - - - - - - - - - - - - - - - - - - " + percent + " - - - - - - - - - - - - - - - - - - - - - - - -";
                    }
                }
                else
                {
                    updateText += "100100100";
                }
                for (int j = 0; j < 8; j++)
                {
                    updateText += System.Environment.NewLine;
                    updateText += System.Environment.NewLine;
                    for (int i = 0; i < 100; i++)
                    {
                        updateText += st[Random.Range(0, st.Length)];
                    }
                    
                }
            }
            else
                updateText = m_sFinalMessage;
            
            

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\" + "loadingloadingloading.txt";
            WriteMessage(path, updateText);

            if (updateText == m_sFinalMessage)
            {
                foreach (Process p in Process.GetProcessesByName("notepad.exe"))
                {
                    p.CloseMainWindow();
                }
            }

            if (percent <= 100)
                Application.OpenURL(path);
            else
            {
                if (timer < timeOnLast)
                {
                    activateOnFinish.SetActive(true);
                    GetComponent<LoadingText>().enabled = false;
                }
            }
        }
        timer += Time.deltaTime;
    }

    private void WriteMessage(string pPath, string pMessage)
    {
        // Rewrites the file with new message
        StreamWriter writer = new StreamWriter(pPath, false);
        writer.WriteLine(pMessage);
        writer.Close();
    }
}
