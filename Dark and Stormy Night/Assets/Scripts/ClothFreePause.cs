﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothFreePause : MonoBehaviour
{
    public Cloth cloth;
    public Vector3 extForce;

	// Use this for initialization
	private void Start()
    {
        ClothSkinningCoefficient[] coefficients = cloth.coefficients;
        for (int i = 0, iend = coefficients.Length; i < iend; ++i)
        {
            coefficients[i].maxDistance = float.MaxValue;
        }
        cloth.coefficients = coefficients;
        cloth.externalAcceleration = extForce;
        //cloth.enabled = false;
        //cloth.enabled = true;
        enabled = false;
    }
}