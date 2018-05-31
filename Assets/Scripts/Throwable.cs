using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //Require the component: rigidbody.

public class Throwable : MonoBehaviour {

    //Private variables.
    int damage;
    Rigidbody rb;

    public int Damage //Public variable that takes in the damage of the throwable object.
    {
        set
        {
            damage = value;
        }
    }

    void Start()
    {
        //Sets the rigidbody to the throwable objects rigidbody.
        rb = this.transform.GetComponent<Rigidbody>();
        //Adds a force to the throwable object towards the forward direction.
        rb.AddForce(this.transform.forward * 20, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Checking if the throwable object is hitting a gameobject other then the player, destroy it.
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject); // Destroy if collison.
        }
        //Checking if the throwable object hits an enemy, add the damage and destroy it.
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyStats>().TakeDamange(damage);
            Destroy(this.gameObject); // Destroy if collison.
        }


    }
}//Mattias Eriksson
