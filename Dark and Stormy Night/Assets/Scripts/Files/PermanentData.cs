using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentData
{
    [System.Serializable]
    public class info
    {
        public List<bool> endingsAchieved = new List<bool>();
        public int lastEndingAchieved;
    }
    [SerializeField]
    public static info saveInfo = new info();
}
