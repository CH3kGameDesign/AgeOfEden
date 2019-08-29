using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseShrink : MonoBehaviour {
    
    private Transform playerTrans;
    private float extents;

    public Vector2 minMaxSizes;
    [Space (10)]
    public Vector3 startPos;
    public Vector3 endPos;
    
    private float endTimer;

	// Use this for initialization
	void Start () {
        startPos += transform.position;
        endPos += transform.position;
        playerTrans = Movement.m_goPlayerObject.transform;
        extents = Mathf.Abs(startPos.x - endPos.x);
	}

    // Update is called once per frame
    void Update()
    {
        float tarPos = 0;
        if (playerTrans.position.x > startPos.x && playerTrans.position.x < endPos.x)
            tarPos = Mathf.Abs(playerTrans.position.x - startPos.x) / extents;
        if (playerTrans.position.x > endPos.x)
            tarPos = 0.9f;

        //playerTrans.localScale = ((extents * tarPos) + minMaxSizes.x) * Vector3.one;
        playerTrans.localScale = Vector3.Lerp(playerTrans.localScale, (tarPos + minMaxSizes.x) * Vector3.one, Time.deltaTime * 2);
        if (playerTrans.localScale.x >= 1)
            playerTrans.localScale = Vector3.one;
    }
}
