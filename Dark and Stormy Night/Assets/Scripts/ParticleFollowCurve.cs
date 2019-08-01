﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowCurve : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public BezierCurve bezier;

    private ParticleSystem.Particle[] particles;

	// Called once before the first frame
	private void Start ()
    {
        if (!particleSystem)
            particleSystem = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < particleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }
	
    /// <summary>
    /// A delayed update function
    /// </summary>
    private void LateUpdate ()
    {
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            float lerp = particles[i].remainingLifetime / particles[i].startLifetime;
            lerp = 1 - lerp;
            Vector3 newPosition = bezier.GetPoint(lerp);
            particles[i].position = newPosition;
            particles[i].startSize = particles[i].startLifetime / 100;
        }

        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}
