using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteFile : MonoBehaviour {

    public List<CreateTextFile> CTF = new List<CreateTextFile>();
    public List<SaveFilesToDisk> SFTD = new List<SaveFilesToDisk>();
    public List<bool> deleteDirectory = new List<bool>();

    public List<string> path = new List<string>();
    public List<int> deleteSpecific = new List<int>();

	// Use this for initialization
	void Start () {
        DoThing();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoThing ()
    {
        if (CTF == null && path == null)
        {
            if (GetComponent<CreateTextFile>() != null)
                CTF.Add(GetComponent<CreateTextFile>());
        }
        for (int j = 0; j < CTF.Count; j++)
        {
            if (deleteSpecific.Count <= j)
            {
                AddThingy(j);
            }
            else
            {
                if (deleteSpecific[j] <= -1)
                    AddThingy(j);
                else
                    AddSpecificThingy(j, deleteSpecific[j]);
            }

        }

        if (SFTD == null && path == null)
        {
            if (GetComponent<SaveFilesToDisk>() != null)
                SFTD.Add(GetComponent<SaveFilesToDisk>());
        }
        for (int j = 0; j < SFTD.Count; j++)
        {
            for (int i = 0; i < SFTD[j].fileActual.Count; i++)
            {
                path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                    + "\\EDEN\\");
                for (int k = 0; k < SFTD[j].subDirectory.Count; k++)
                {
                    path[path.Count - 1] += SFTD[j].subDirectory[k];
                    path[path.Count - 1] += "\\";
                }
                if (!deleteDirectory[path.Count - 1])
                    path[path.Count - 1] += SFTD[j].fileActual[i].name + SFTD[j].fileExtension[i];
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

    public void AddThingy(int j)
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
    public void AddSpecificThingy(int j, int specific)
    {
        if (CTF[j].m_oOutputs[specific].m_olOutputLocation == CreateTextFile.Location.Desktop)
            path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                + "\\");
        else if (CTF[j].m_oOutputs[specific].m_olOutputLocation == CreateTextFile.Location.Documents)
            path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                + "\\");
        else if (CTF[j].m_oOutputs[specific].m_olOutputLocation == CreateTextFile.Location.Music)
            path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)
                + "\\");
        else if (CTF[j].m_oOutputs[specific].m_olOutputLocation == CreateTextFile.Location.Pictures)
            path.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)
                + "\\");
        for (int k = 0; k < CTF[j].m_oOutputs[specific].folderPath.Count; k++)
        {
            path[path.Count - 1] += CTF[j].m_oOutputs[specific].folderPath[k];
            path[path.Count - 1] += "\\";
        }
        if (!deleteDirectory[path.Count - 1])
            path[path.Count - 1] += CTF[j].m_oOutputs[specific].m_sFileName + ".txt";
    }
}
