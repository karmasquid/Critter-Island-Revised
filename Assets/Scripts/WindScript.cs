using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindScript : MonoBehaviour
{
    [SerializeField]
    ParticleSystem WindPartical;

    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (!player.GetComponent<Character>().dodging)
            {
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.transform.position = Vector3.MoveTowards(player.transform.position, WindPartical.transform.forward, 5 * Time.deltaTime);
            }
            else
            {
                print("You got passed.");
            }
        }
        
    }
}