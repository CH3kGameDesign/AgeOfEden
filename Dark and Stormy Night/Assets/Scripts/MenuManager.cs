using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject portal;
    public GameObject room;

    public static bool inRoom = false;

    private Transform m_tPortalTrans;
    private Transform m_tRoomTrans;
    private Transform m_tPlayerTrans;

    private void Start()
    {
        m_tPortalTrans = portal.transform;
        m_tRoomTrans = room.transform;
        m_tPlayerTrans = Movement.s_goPlayerObject.transform;
    }

    // Update is called once per frame
    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1) && !inRoom)
        {
            m_tPortalTrans.position = m_tPlayerTrans.position + m_tPlayerTrans.forward;

            m_tPortalTrans.LookAt(m_tPlayerTrans);

            if (InvertGravity.invertedGravity)
                m_tPortalTrans.eulerAngles = new Vector3(0, m_tPortalTrans.eulerAngles.y, 180);

            m_tRoomTrans.rotation = m_tPortalTrans.rotation;
        }
	}
}