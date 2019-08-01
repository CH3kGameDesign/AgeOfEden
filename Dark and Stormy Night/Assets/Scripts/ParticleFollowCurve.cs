using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowCurve : MonoBehaviour {

    public ParticleSystem parSystem;

    public BezierCurve bezier;

    public float sizeDivide = 100f;

    private ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () {
        if (parSystem == null)
            parSystem = GetComponent<ParticleSystem>();
        if (particles == null || particles.Length < parSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[parSystem.main.maxParticles];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        int numParticlesAlive = parSystem.GetParticles(particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            float lerp = particles[i].remainingLifetime / particles[i].startLifetime;
            lerp = 1 - lerp;
            Vector3 newPosition = bezier.GetPoint(lerp);
            particles[i].position = newPosition;
            particles[i].startSize = particles[i].startLifetime / sizeDivide;
        }
        parSystem.SetParticles(particles, numParticlesAlive);
    }
}
