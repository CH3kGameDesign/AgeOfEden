using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public static class SaveLoad
{
    /// <summary>
    /// Saves the game progress
    /// </summary>
    public static void Save()
    {
        // Verifies the file path exists and creates one if not
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");

        // Creates a filestream to the desired file path
        FileStream fs = new FileStream(Application.persistentDataPath
            + "/SaveData/SuperSecretSaveData.dat", FileMode.Create);

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            bf.Serialize(fs, PermanentData.saveInfo);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    /// <summary>
    /// Loads saved progress
    /// </summary>
    public static void Load()
    {
        // Verifies the file path exists and creates one if not
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");

        if (File.Exists(Application.persistentDataPath + "/SaveData/SuperSecretSaveData.dat"))
        {
            using (Stream stream = File.Open(Application.persistentDataPath
                + "/SaveData/SuperSecretSaveData.dat", FileMode.Open))
            {
                //Debug.Log(Application.persistentDataPath);
                BinaryFormatter bformatter = new BinaryFormatter();
                PermanentData.saveInfo = (PermanentData.Info)bformatter.Deserialize(stream);
            }
        }
    }

    /// <summary>
    /// Resets saved progress
    /// </summary>
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