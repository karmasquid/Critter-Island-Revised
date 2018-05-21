using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindScript : MonoBehaviour
{
    [SerializeField]
    float offsetTimer;

    bool emitting = true;

    [SerializeField]
    ParticleSystem WindPartical;

    GameObject player;
    void Start()
    {
        InvokeRepeating("OnOffEmission", 0f, offsetTimer);
        player = GameObject.Find("Player");
    }
    //TODO When time for actual particalls is set -> Add Coroutine
    void OnOffEmission()
    {
        if (!emitting)
        {

            WindPartical.Play();
            emitting = true;
        }
        else
        {
            WindPartical.Stop();
            emitting = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        //SOLUTION A
        if (other.gameObject.name == "Player" && emitting)
        {
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = Vector3.MoveTowards(player.transform.position, WindPartical.transform.forward, 10 * Time.fixedDeltaTime);
            print("You are now falling now...");
            //player.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (!emitting)
        {
                print("Not falling down...");
        }
        /* SOLUTION B
        if (other.gameObject.name == "Player")
        {
            if (!player.GetComponent<Character>().dodging)
            {
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.transform.position = Vector3.MoveTowards(player.transform.position, WindPartical.transform.forward, 10 * Time.deltaTime);
            }
            else
            {
                print("You got passed.");
            }
        }
        */
    }
}