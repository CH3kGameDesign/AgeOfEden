using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateTextFile : MonoBehaviour {

    public string fileName;
    public string fileText;

	// Use this for initialization
	void Start () {
        CreateFile();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateFile ()
    {
        
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/" + fileName +".txt";
        FileStream file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        File.WriteAllText(path, fileText);
    }
}
