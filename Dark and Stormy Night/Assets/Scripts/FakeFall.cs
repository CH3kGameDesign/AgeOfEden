using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFall : MonoBehaviour
{
    public Vector3 PlayerResetPos;

    public NeverEndingCorridor NECorridor;

    private Transform m_tNECorridorChild;

    private void Start()
    {
        m_tNECorridorChild = NECorridor.transform.GetChild(0).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = PlayerResetPos;
            if (NECorridor)
                m_tNECorridorChild.position += new Vector3(10000, 0, 0);
        }
    }
}