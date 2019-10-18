using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class CheckChangeInFile : MonoBehaviour {

    [System.Serializable]
    public class Choice
    {
        public string requiredText;
        public GameObject activateOnYes;
    }

    public CreateTextFile tar;

    private string path;
    private string insideText;

    public List<Choice> m_cChoices = new List<Choice>();

	// Use this for initialization
	void Start () {
        if (tar == null)
            tar = GetComponent<CreateTextFile>();


        if (tar.m_oOutputs[0].m_olOutputLocation == CreateTextFile.Location.Desktop)
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                + "\\";
        else if (tar.m_oOutputs[0].m_olOutputLocation == CreateTextFile.Location.Documents)
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                + "\\";
        else if (tar.m_oOutputs[0].m_olOutputLocation == CreateTextFile.Location.Music)
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)
                + "\\";
        else if (tar.m_oOutputs[0].m_olOutputLocation == CreateTextFile.Location.Pictures)
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)
                + "\\";
        for (int j = 0; j < tar.m_oOutputs[0].folderPath.Count; j++)
        {
            path += tar.m_oOutputs[0].folderPath[j];
            path += "\\";
        }
        path += tar.m_oOutputs[0].m_sFileName;
        if (tar.m_oOutputs[0].txtExtension)
            path += ".txt";

        
    }
	
	// Update is called once per frame
	void LateUpdate () {
        insideText = tar.m_oOutputs[0].m_sMessage;
        string insideTextCheck = insideText + System.Environment.NewLine;

        StreamReader sr = new StreamReader(path);
        string insideText2 = sr.ReadToEnd();
        sr.Close();
        if (insideTextCheck != insideText2)
        {
            if (insideText2.Contains(insideText))
                insideText2 = insideText2.Replace(insideText, "");
            for (int i = 0; i < m_cChoices.Count; i++)
            {
                if (insideText2.ToLower().Contains(m_cChoices[i].requiredText))
                {
                    m_cChoices[i].activateOnYes.SetActive(true);
                    GetComponent<CheckChangeInFile>().enabled = false;
                }
            }
        }

	}
}
