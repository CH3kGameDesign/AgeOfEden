using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour {

    public SaveEditor SE;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PermanentData.saveInfo.endingsAchieved.Count < SE.buttons.Count)
        {
            for (int i = 0; i < SE.buttons.Count - PermanentData.saveInfo.endingsAchieved.Count; i++)
            {
                PermanentData.saveInfo.endingsAchieved.Add(false);
            }
        }
    }
}
