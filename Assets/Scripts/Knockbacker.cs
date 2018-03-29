﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbacker : MonoBehaviour {

     Rigidbody rb =null;
    public float Force;

	// Use this for initialization
	void Start () {

        //rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();



		
	}

    //Känner efter om spelaren vidrör collidern och hämtar då spelarens RigidBody för att kunna påverka den. Fixad av Kai.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Trigger " + other.name);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Triggered");
            rb = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        //Om rb blivit tilldelad, putta bak spelaren i puttarens framåtriktning. Fixad av Kai.
        if (rb != null)
        {
            Debug.Log("Has Body " + rb.name);
            rb.AddForce(transform.forward * Force * 10f, ForceMode.Impulse);
            Debug.Log("Kraften är " + transform.forward * Force * 10f);
            rb = null;
        }

    }

}