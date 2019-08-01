using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveRelativeOnTrain : MonoBehaviour {

    public Transform Movee;
    public List<Vector3> tarPos = new List<Vector3>();
    private int currentTar;
    private Vector3 currentDistMoved;

    public float MetresPerSecond;
    [Space(20)]
    public bool faceDirection;
    [Space(20)]
    public UnityEvent onFinishEvents;
    public List<GameObject> GO = new List<GameObject>();
    public List<GameObject> GODisable = new List<GameObject>();


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (currentTar < tarPos.Count)
        {
            float speed = Time.deltaTime * MetresPerSecond;

            if (faceDirection)
            {
                var rotation = Quaternion.LookRotation(tarPos[currentTar]);
                Movee.transform.rotation = Quaternion.RotateTowards(Movee.transform.rotation, rotation, 180 * Time.deltaTime);
            }
            //Movee.LookAt(tarPos[currentTar] + Movee.transform.position);


            if (Vector3.Distance(currentDistMoved, tarPos[currentTar]) < speed)
            {
                Vector3 direction = tarPos[currentTar] - currentDistMoved;
                direction *= 100;
                direction = Vector3.ClampMagnitude(direction, Vector3.Distance(currentDistMoved, tarPos[currentTar]));

                Movee.localPosition += direction;

                currentDistMoved = Vector3.zero;
                currentTar += 1;

                if (currentTar == tarPos.Count)
                    Finish();

            }
            else
            {
                Vector3 direction = tarPos[currentTar] - currentDistMoved;
                direction *= 100;
                direction = Vector3.ClampMagnitude(direction, 1);

                

                Movee.localPosition += direction * speed;
                currentDistMoved += direction * speed;
            }
        }
    }

    void Finish ()
    {
        onFinishEvents.Invoke();
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(true);
        }
        for (int i = 0; i < GODisable.Count; i++)
        {
            GODisable[i].SetActive(false);
        }
    }
}
