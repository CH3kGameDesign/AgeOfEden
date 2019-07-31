using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public enum PortalType
    {
        NotMenu,
        PlayerPortal,
        MenuPortal
    }

    private bool m_bPlayerIsOverlapping = false;

    public PortalType m_ptPortalType;

	public Transform m_tPlayer;
	public Transform m_tReciever;

    public GameObject[] m_goActivateOnTeleport;
    public GameObject[] m_goDeactivateOnTeleport;
    
    // Called before the first frame
    private void Start ()
    {
        if (Movement.player)
            m_tPlayer = Movement.player.transform;
    }

    // Update is called once per frame
    private void Update ()
    {
		if (m_bPlayerIsOverlapping)
		{
			Vector3 portalToPlayer = m_tPlayer.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, m_tReciever.rotation);
				rotationDiff += 180;
                SmoothCameraMovement.turnAroundValue += rotationDiff;

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				m_tPlayer.position = m_tReciever.position + positionOffset;

				m_bPlayerIsOverlapping = false;

                for (int i = 0; i < m_goActivateOnTeleport.Length; i++)
                    m_goActivateOnTeleport[i].SetActive(true);

                for (int i = 0; i < m_goDeactivateOnTeleport.Length; i++)
                    m_goDeactivateOnTeleport[i].SetActive(false);

                if (m_ptPortalType == PortalType.PlayerPortal)
                    Menu.inRoom = true;

                if (m_ptPortalType == PortalType.MenuPortal)
                    Menu.inRoom = false;
            }
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
			m_bPlayerIsOverlapping = true;
	}

	private void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
			m_bPlayerIsOverlapping = false;
	}
}