﻿using UnityEngine;
using System.Collections;

public class SwappableMaterial : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ChangeMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }
}
