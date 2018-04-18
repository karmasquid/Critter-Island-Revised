using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Throwable : MonoBehaviour {

    int damage;

    Rigidbody rb;

    public int Damage
    {
        set
        {
            damage = value;
        }
    }

    void Start()
    {
        rb = this.transform.GetComponent<Rigidbody>();

        rb.AddForce(this.transform.forward * 20, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyStats>().TakeDamange(damage);

        }
            Destroy(gameObject); // Destroy if collison.

    }
}
