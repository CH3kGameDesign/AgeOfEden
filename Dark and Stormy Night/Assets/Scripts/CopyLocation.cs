using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour
{
    public Transform target;
    public Transform movee;
    public bool copyPlayer = false;
    public bool copyCamera = false;
    public Vector3 direction;
    public bool copyRotation = false;

    private Transform m_tTransCache;

    private void Start()
    {
        if (movee == null)
            movee = transform;
        if (copyPlayer)
            target = Movement.s_goPlayerObject.transform;
        if (copyCamera)
            target = CameraMovement.s_CameraObject.transform;

        m_tTransCache = transform;
    }


    // Update is called once per frame
    private void LateUpdate ()
    {
        if (direction.x != 0)
            movee.position = new Vector3(target.position.x, m_tTransCache.position.y,
                m_tTransCache.position.z);

        if (direction.y != 0)
            movee.position = new Vector3(m_tTransCache.position.x, target.position.y,
                m_tTransCache.position.z);

        if (direction.z != 0)
            movee.position = new Vector3(m_tTransCache.position.x,
                m_tTransCache.position.y, target.position.z);
                
        if (copyRotation == true)
            movee.rotation = target.rotation;
    }
}