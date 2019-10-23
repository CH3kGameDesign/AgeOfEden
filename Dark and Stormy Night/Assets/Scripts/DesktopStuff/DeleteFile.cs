using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteFile : MonoBehaviour {

    public List<CreateTextFile> CTF = new List<CreateTextFile>();
    public List<bool> deleteDirectory = new List<bool>();

    public List<string> path = new List<string>();

	// Use this for initialization
	void Start () {
		if (CTF == null && path == null)
        {
            if (GetComponent<CreateTextFile>() != null)
                CTF.Add(GetComponent<CreateTextFile>());
        }
        for (int j = 0; j < CTF.Count; j++)
        {
            for (int i = 0; i < CTF[j].m_oOutputs.Length; i++)
            {
                if (CTF[j].m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Desktop)
                    path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                        + "\\");
                else if (CTF[j].m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Documents)
                    path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                        + "\\");
                else if (CTF[j].m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Music)
                    path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)
                        + "\\");
                else if (CTF[j].m_oOutputs[i].m_olOutputLocation == CreateTextFile.Location.Pictures)
                    path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)
                        + "\\");
                for (int k = 0; k < CTF[j].m_oOutputs[i].folderPath.Count; k++)
                {
                    path[path.Count - 1] += CTF[j].m_oOutputs[i].folderPath[k];
                    path[path.Count - 1] += "\\";
                }
                if (!deleteDirectory[path.Count - 1])
                    path[path.Count - 1] += CTF[j].m_oOutputs[i].m_sFileName + ".txt";
            }
        }

        for (int i = 0; i < path.Count; i++)
        {
            if (!deleteDirectory[path.Count - 1])
            {
                if (File.Exists(path[i]))
                    File.Delete(path[i]);
            }
            else
            {
                if (Directory.Exists(path[i]))
                    Directory.Delete(path[i], true);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
