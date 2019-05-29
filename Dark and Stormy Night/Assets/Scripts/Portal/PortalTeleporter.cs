using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

	public Transform player;
	public Transform reciever;
    public GameObject[] activateOnTeleport;
    public GameObject[] DeActivateOnTeleport;

    public enum PortalType {NotMenu, PlayerPortal, MenuPortal}

    public PortalType portalType;

    private bool playerIsOverlapping = false;

	// Update is called once per frame
	void Update () {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
                SmoothCameraMovement.turnAroundValue += rotationDiff;

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = reciever.position + positionOffset;

				playerIsOverlapping = false;
                for (int i = 0; i < activateOnTeleport.Length; i++)
                {
                    activateOnTeleport[i].SetActive(true);
                }
                for (int i = 0; i < DeActivateOnTeleport.Length; i++)
                {
                    DeActivateOnTeleport[i].SetActive(false);
                }

                if (portalType == PortalType.PlayerPortal)
                {
                    Menu.inRoom = true;
                }
                if (portalType == PortalType.MenuPortal)
                {
                    Menu.inRoom = false;
                }
            }
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}
