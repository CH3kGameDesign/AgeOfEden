using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSpawner : MonoBehaviour
{
    public GameObject hand;
    [Space(10)]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    [Space (20)]

    public Vector2 spawnRateBounds;
    public Vector2 fadeRateBounds;
    public Vector2 fadeTimeBounds;
    private float spawnTimer;

    public bool spawnHands;
    
	// Update is called once per frame
	private void Update ()
    {
		if (spawnHands)
        {
            if (spawnTimer > 0)
            {
                Vector3 spawnLocation = new Vector3(Random.Range(
                    spawnAreaMin.x, spawnAreaMax.x), Random.Range(
                        spawnAreaMin.y, spawnAreaMax.y), 0);

                Quaternion spawnRotation = Quaternion.Euler(
                    new Vector3(0, 0, Random.Range(0, 360)));

                GameObject GO = Instantiate(hand, transform);
                GO.transform.localPosition = spawnLocation;
                GO.transform.rotation = spawnRotation;
                GO.GetComponent<FadeOut>().fadeWait = Random.Range(
                    fadeTimeBounds.x, fadeTimeBounds.y);
                GO.GetComponent<FadeOut>().fadeSpeed = Random.Range(
                    fadeRateBounds.x, fadeRateBounds.y);

                spawnTimer = -(Random.Range(spawnRateBounds.x, spawnRateBounds.y));
            }
            spawnTimer += Time.deltaTime;
        }
	}

    /// <summary>
    /// Assigns a value to a variable
    /// </summary>
    /// <param name="pTruth"></param>
    public void SpawnHands (bool pTruth)
    {
        spawnHands = pTruth;
    }
}
