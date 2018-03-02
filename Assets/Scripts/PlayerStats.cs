using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    private Stats health;

    [SerializeField]
    private Stats stamina;

	// Use this for initialization
	private void Awake ()
    {
        health.Initialize();
        stamina.Initialize();
	}
	
	// Update is called once per frame
	void Update () {

        if (stamina.CurrentValue < stamina.MaxValue)
        {
            stamina.CurrentValue += 1;
        }
        

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
            stamina.CurrentValue -= 30;
        }
    }

}
