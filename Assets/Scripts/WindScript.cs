using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindScript : MonoBehaviour
{
    //Particle effect whom will cause the effect to the player.
    [SerializeField]
    ParticleSystem WindPartical;

    //private gameobject.
    GameObject player;

    void Start()
    {
        //Sets the gameobject to the player in the current scene.
        player = GameObject.Find("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player") //If the player is inside the particle effect area...
        {
            if (!player.GetComponent<Character>().dodging) //And not dodging...
            {
                //Disable the navmeshagent and transform the position of the player to the forward direction of the wind particle effect.
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.transform.position = Vector3.MoveTowards(player.transform.position, WindPartical.transform.forward, 5 * Time.deltaTime);
            }
        }
        
    }
}