using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseShrink : MonoBehaviour
{
    private Transform playerTrans;
    private float extents;

    public Vector2 minMaxSizes;
    [Space (10)]
    public Vector3 startPos;
    public Vector3 endPos;
    
    private float endTimer;
    private Camera playerCamera;
    private float tarClipPlane = 0;

	// Use this for initialization
	private void Start ()
    {
        if (playerCamera == null)
            playerCamera = CameraMovement.s_CameraObject.GetComponentInChildren<Camera>();

        if (tarClipPlane == 0)
            tarClipPlane = playerCamera.nearClipPlane;

        startPos += transform.position;
        endPos += transform.position;
        playerTrans = Movement.s_goPlayerObject.transform;
        extents = Mathf.Abs(startPos.z - endPos.z);
	}

    // Update is called once per frame
    private void Update()
    {
        float tarPos = 0;

        if (playerTrans.position.z > startPos.z && playerTrans.position.z < endPos.z)
            tarPos = Mathf.Abs(playerTrans.position.z - startPos.z) / extents;

        if (playerTrans.position.z > endPos.z || tarPos >= 0.9f)
            tarPos = 0.9f;

        //playerTrans.localScale = ((extents * tarPos) + minMaxSizes.x) * Vector3.one;
        playerTrans.localScale = Vector3.Lerp(playerTrans.localScale,
            (tarPos + minMaxSizes.x) * Vector3.one, Time.deltaTime * 2);

        if (playerTrans.localScale.x >= 1)
            playerTrans.localScale = Vector3.one;

        playerCamera.nearClipPlane = tarClipPlane * (tarPos + minMaxSizes.x);
    }

    private void OnDisable()
    {
        playerTrans.localScale = Vector3.one;
        playerCamera.nearClipPlane = tarClipPlane;
    }
}
