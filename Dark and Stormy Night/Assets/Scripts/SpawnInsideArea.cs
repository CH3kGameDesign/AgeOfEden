using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInsideArea : MonoBehaviour
{
    public enum area {box, sphere}

    public area areaType;
    public Transform point;
    public float radius;

    public Vector3 pointMax;
    public Vector3 pointMin;
    public Vector2 spawnRateMax;
    private float spawnRate;
    private float spawnRateCounter;

    public List<GameObject> spawnObjects = new List<GameObject>();

	// Called once before the first frame
	private void Start ()
    {
        spawnRate = Random.Range(spawnRateMax.x, spawnRateMax.y);
        spawnRateCounter = 0;
	}
	
	// Called once per frame
	private void Update ()
    {
        if (spawnRateCounter > spawnRate)
        {
            int sel = Random.Range(0, spawnObjects.Count);

            if (areaType == area.sphere)
            {
                Vector3 pos = Random.insideUnitSphere * radius;
                Instantiate(spawnObjects[sel], point.position + pos, spawnObjects[sel].transform.rotation);
            }

            if (areaType == area.box)
            {
                float x = Random.Range(pointMin.x, pointMax.x);
                float y = Random.Range(pointMin.y, pointMax.y);
                float z = Random.Range(pointMin.z, pointMax.z);

                Instantiate(spawnObjects[sel], new Vector3(x,y,z), spawnObjects[sel].transform.rotation);
            }
            spawnRate = Random.Range(spawnRateMax.x, spawnRateMax.y);
            spawnRateCounter = 0;
        }
        spawnRateCounter += Time.deltaTime;
	}
}
