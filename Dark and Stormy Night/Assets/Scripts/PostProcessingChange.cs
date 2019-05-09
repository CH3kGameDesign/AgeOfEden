using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingChange : MonoBehaviour {

    public PostProcessingProfile tarProfile;
    public PostProcessingBehaviour tarCamera;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        tarCamera.profile = tarProfile;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
