using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [HideInInspector]
    public static GameObject player;
    public static Vector3 faceDirection;

    [Header("GameObjects")]
    public GameObject cameraHolder;

    [Header("Variables")]
    public float rotSpeed;

    private float desiredYRotation;

    private Transform m_tTransCache;
    private Transform m_tCameraTrans;

    // Use this for initialization
    private void Start()
    {
        faceDirection = Vector3.zero;
        player = gameObject;
        m_tTransCache = transform;
        m_tCameraTrans = cameraHolder.transform;
    }
	
	// Update is called once per frame
	private void Update()
    {
        //desiredYRotation = Mathf.Lerp(desiredYRotation,
        //    m_tCameraTrans.rotation.eulerAngles.y, rotSpeed);

        if (Movement.s_bCanMove)
        {
            if (faceDirection == Vector3.zero)
                m_tTransCache.rotation = Quaternion.Euler(0, m_tCameraTrans.rotation.eulerAngles.y,
                    m_tCameraTrans.rotation.eulerAngles.z);
            else
                m_tTransCache.LookAt(new Vector3(m_tTransCache.position.x + faceDirection.x,
                    m_tTransCache.position.y, m_tTransCache.position.z + faceDirection.z));
        }
	}
}