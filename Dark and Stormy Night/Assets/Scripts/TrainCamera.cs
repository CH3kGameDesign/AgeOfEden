using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCamera : MonoBehaviour {

    private Transform cameraTrans;
    private Transform playerTrans;
    private float extents;

    public BezierSpline line;
    public BezierSpline line2;
    public BezierSpline endLine;
    [Space (10)]
    public Vector3 startPos;
    public Vector3 endPos;
    
    private float endTimer;

	// Use this for initialization
	void Start () {
        startPos += transform.position;
        endPos += transform.position;
        cameraTrans = CameraMovement.s_CameraObject.transform;
        playerTrans = Movement.m_goPlayerObject.transform;
        extents = Mathf.Abs(startPos.z - endPos.z);
        cameraTrans.GetComponent<CameraMovement>().m_tCameraHook = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Movement.canMove == false)
            endingMoments();
        else
        {
            float tarPos = 0;
            if (playerTrans.position.z > startPos.z && playerTrans.position.z < endPos.z)
                tarPos = Mathf.Abs(playerTrans.position.z - startPos.z) / extents * 2;
            if (playerTrans.position.z > endPos.z)
                tarPos = 2;
            if (tarPos >= 1)
            {
                tarPos -= 1;
                cameraTrans.position = Vector3.Lerp(cameraTrans.position, line2.GetPoint(tarPos), Time.deltaTime * 2);
            }
            else
                cameraTrans.position = Vector3.Lerp(cameraTrans.position, line.GetPoint(tarPos), Time.deltaTime * 2);
            
        }
	}

    void endingMoments ()
    {
        endTimer = Mathf.Lerp(endTimer, 1, Time.deltaTime / 2);
        cameraTrans.position = endLine.GetPoint(endTimer);
    }
}
