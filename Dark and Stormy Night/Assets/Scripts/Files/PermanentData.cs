using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentData
{
    [System.Serializable]
    public class Info
    {
        public List<bool> endingsAchieved = new List<bool>();
        public int lastEndingAchieved;
        public bool firstTime = true;

        // Constructor
        public Info()
        {
            int endingCount = 5;
            for (int i = 0; i <= endingCount; i++)
                endingsAchieved.Add(false);
        }
    }
    
    public static Info saveInfo = new Info();
}