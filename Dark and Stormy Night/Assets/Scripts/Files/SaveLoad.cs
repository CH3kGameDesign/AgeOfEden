using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    /*
        WHAT SCRIPT DOES:
        -   Saves Progress
        -   Loads Progress
        -   Resets Progress
    */

    //SAVE
    public static void Save()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }
        //Inventory
        FileStream fs = new FileStream(Application.persistentDataPath
            + "/SaveData/SuperSecretSaveData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, PermanentData.saveInfo);
        fs.Close();
    }

    //LOAD
    public static void Load()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }

        if (File.Exists(Application.persistentDataPath + "/SaveData/SuperSecretSaveData.dat"))
        {
            using (Stream stream = File.Open(Application.persistentDataPath
                + "/SaveData/SuperSecretSaveData.dat", FileMode.Open))
            {
                Debug.Log(Application.persistentDataPath);
                BinaryFormatter bformatter = new BinaryFormatter();
                PermanentData.saveInfo = (PermanentData.Info)bformatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.Log("CreatedNew");
            FileStream fs = new FileStream(Application.persistentDataPath
            + "/SaveData/SuperSecretSaveData.dat", FileMode.Create);
            PermanentData.saveInfo.endingsAchieved.Add(false);
            PermanentData.saveInfo.lastEndingAchieved = 0;
            PermanentData.saveInfo.firstTime = true;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, PermanentData.saveInfo);
            fs.Close();
        }
    }

    //RESET PROGRESS
    public static void ResetProgress()
    {
        if (Directory.Exists(Application.persistentDataPath + "/SaveData"))
        {
            var hi = Directory.GetFiles(Application.persistentDataPath + "/SaveData");
            for (int i = 0; i < hi.Length; i++)
            {
                File.Delete(hi[i]);
            }
        }
    }
}