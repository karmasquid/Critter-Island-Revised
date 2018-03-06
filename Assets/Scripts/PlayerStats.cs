﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    //Skapar stats från stats-scriptet.
    [SerializeField]
    private Stats health;

    [SerializeField]
    private Stats stamina;

    //Desto lägre staminaRecharge desto snabbare laddas stamina.
    [SerializeField]
    private float staminaRecharge;

    //Variabel för timer.
    private float timeCheck = 0;

	private void Awake ()
    {
        health.Initialize();
        stamina.Initialize();
	}
	
	void Update () {

        //En timer som ökar stamina med 1 varje halv sekund så länge stamina ej är MaxValue.
        if (stamina.CurrentValue < stamina.MaxValue)
        {
            timeCheck += Time.deltaTime;
        }

        if (timeCheck > staminaRecharge)
        {
            timeCheck = 0;
            stamina.CurrentValue += 1;
        }
        
        //Test
        if (Input.GetKeyDown(KeyCode.A))
        {
            health.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            health.CurrentValue += 10;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            stamina.CurrentValue -= 10;
        }
    }

}
