using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PortalTeleporter : MonoBehaviour
{
    [FormerlySerializedAs("player")]
	public Transform m_tPlayer;
    [FormerlySerializedAs("reciever")]
    public Transform m_tReciever;

    [FormerlySerializedAs("activateOnTeleport")]
    public GameObject[] m_goActivateOnTeleport;
    [FormerlySerializedAs("DeActivateOnTeleport")]
    public GameObject[] m_goDeactivateOnTeleport;

    public enum PortalType { NotMenu, PlayerPortal, MenuPortal };

    public PortalType portalType;

    private bool playerIsOverlapping = false;

    private void Start()
    {
        if (Movement.s_goPlayerObject)
            m_tPlayer = Movement.s_goPlayerObject.transform;
    }

    // Update is called once per frame
    private void Update()
    {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = m_tPlayer.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, m_tReciever.rotation);
				rotationDiff += 180;
                SmoothCameraMovement.s_fTurnAroundValue += rotationDiff;

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				m_tPlayer.position = m_tReciever.position + positionOffset;

				playerIsOverlapping = false;

                for (int i = 0; i < m_goActivateOnTeleport.Length; i++)
                    m_goActivateOnTeleport[i].SetActive(true);

                for (int i = 0; i < m_goDeactivateOnTeleport.Length; i++)
                    m_goDeactivateOnTeleport[i].SetActive(false);

                if (portalType == PortalType.PlayerPortal)
                    Menu.inRoom = true;

                if (portalType == PortalType.MenuPortal)
                    Menu.inRoom = false;
            }
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}