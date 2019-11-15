using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFilesToDisk : MonoBehaviour {
    
    public List<TextAsset> fileActual = new List<TextAsset>();
    public List<string> fileExtension = new List<string>();
    public List<string> subDirectory = new List<string>();
    // Use this for initialization
    void Start () {
        for (int i = 0; i < fileActual.Count; i++)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                            + "\\EDEN\\";
            for (int j = 0; j < subDirectory.Count; j++)
            {
                path += subDirectory[j] + "\\";
            }
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += fileActual[i].name;
            if (fileExtension.Count > i)
                path += fileExtension[i];
            var splitFile = new string[] { "\r\n", "\r", "\n" };
            var lines = fileActual[i].text.Split(splitFile, StringSplitOptions.None);

            for (int j = 0; j < lines.Length; j++)
            {
                WriteMessage(path, lines[j]);
                //WriteMessage(path, Environment.NewLine);
            }
            //File.WriteAllBytes(path, fileActual[i].bytes);
            //string message = fileActual[i].text;
            //WriteMessage(path, fileActual[i].text);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void WriteMessage(string pPath, string pMessage)
    {
        // Rewrites the file with new message
        StreamWriter writer = new StreamWriter(pPath, true);
        writer.WriteLine(pMessage);
        writer.Close();
    }
}
