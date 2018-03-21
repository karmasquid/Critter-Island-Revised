using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Attacker.hit = true;
            Destroy(gameObject);
        }
    }
}
