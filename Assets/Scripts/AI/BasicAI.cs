using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public Transform targetPlayer;
    public string playerTag = "Player";
   
    
    [SerializeField]
    float targetingRange = 10f;
    [SerializeField]
    float aggroRange = 0f;
    [SerializeField]
    float attackRange = 0f;

    NavMeshAgent agent;
    
	void Start ()
        {
        playerInSight = false;
        agent = GetComponent<NavMeshAgent>();
      //  InvokeRepeating("Searching", 0f, 0.5f );
        targetPlayer = PlayerManager.instance.player.transform;
        
	    }

    void Searching()
        {
        

        

         
       /*GameObject[] playerMesh = GameObject.FindGameObjectsWithTag(playerTag);
        float closestRange = Mathf.Infinity;
        GameObject nearestPlayer = null; //Närmsta spelarmodellen som finns relativt till fienden med detta script.

         foreach (GameObject player in playerMesh)
            {
            
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceToPlayer < closestRange) //Om räckvidden till spelaren är mindre än närmsta räckvidden betyder det att fienden hittat spelaren. 
            {
                
                closestRange = distanceToPlayer;
                nearestPlayer = player;
            }
            
        }
        if (nearestPlayer != null && closestRange <= targetingRange)
        {
            print("Hello!");
            targetPlayer = nearestPlayer.transform;

            PlayerTargeted();
        }
        else
        {
            targetPlayer = null;
        } */
    }

    void PlayerTargeted()
    {
        print ("Hey you!");
       // transform.position = new Vector3();
          
    }
    void Update()
    {

        float distanceToPlayer = Vector3.Distance(targetPlayer.position, transform.position);

        if (distanceToPlayer <= targetingRange)
        {
            agent.SetDestination(targetPlayer.position);
        }
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
