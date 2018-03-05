using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {
    /* AI states
     * 
     * IDLE
     * PATROLING
     * TARGETING
     * APROACH (kan vara snabbare hastighet än patrol)
     * ATTACK
     * LOSE TARGET > IDLE > PATROLING
     * DEAD  */
    bool playerInSight;
    int currentHP = 100;
    int maxHP = 100;
    public int attackDMG = 0;
   
    [SerializeField]
    float patrolSpeed = 0f;
    [SerializeField]
    float targetingRange = 0f;
    [SerializeField]
    float aggroRange = 0f;
    [SerializeField]
    float approachSpeed = 0f;
    float acceleration = 0.1f; // patrolSpeed += acceleration;
    [SerializeField]
    float attackRange = 0f;
    
	void Start ()
        {
        InvokeRepeating("Searching", 0f, 0.5f );
        playerInSight = false;
        
	    }

	void Update ()
        {
		
	    }

    void Searching()
        {
       GameObject playerMesh = GameObject.FindWithTag("Player");
        float closestRange = Mathf.Infinity;
        GameObject pointBlankPlayer = null;

        /* while (playerInSight = false) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        } */
    }

    void playerTargeted() {
        print ("Hey you!");
       // transform.position = new Vector3();

    }

    void OnDrawGizmosSelected () //Måste kunna se hur långt den kan göra olika saker.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

}
