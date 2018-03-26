using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbacker : MonoBehaviour {

     Rigidbody rb =null;
    public float Force;

	// Use this for initialization
	void Start () {

        //rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();



		
	}

    private void OnMouseDown()
    {
        rb.AddForce(transform.forward * Force * 100f);
        Debug.Log("Forced");
    }

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

        if (rb != null)
        {
            Debug.Log("Has Body " + rb.name);
            rb.AddForce(transform.forward * Force * 10f, ForceMode.Impulse);
            Debug.Log("KRaften är " + transform.forward * Force * 10f);
            rb = null;
        }

    }

}
