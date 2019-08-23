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
    }

    [SerializeField]
    public static Info saveInfo = new Info();
}