﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour {

    public static InventoryCanvas instance;

	// Use this for initialization
	void Awake() {

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
