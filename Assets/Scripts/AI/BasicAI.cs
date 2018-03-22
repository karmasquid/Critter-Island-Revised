using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Scritpt skriven av Felype Arévalo Arancibia.
// Combat skriven av _____________.

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

   // bool playerInSight;
    public int currentHP = 100;
    public int maxHP = 100;
    public int maxArmor = 0;
    public int currentArmor = 0;
    public int attackDMG = 0;
    bool playerInSight;
    bool isAttacking; // <------------------------------------------- check
    Transform targetPlayer;
    string playerTag = "Player";

    [SerializeField]
    float noGoZone = 2f;
    [SerializeField]
    float targetingRange = 10f;
    [SerializeField]
    float aggroRange = 0f;
    [SerializeField]
    float attackRange = 0f;

    public float roamingRadius = 0f;
    public float roamingTimer = 0f;
    private Transform enemyAI;
    private float timer;
    private float timeCheck;
    private float timeBetweenAttacks = 2f;
    private float timeForAttack;

    
    NavMeshAgent agent;
    
	void Start ()
        {
        isAttacking = false;
        playerInSight = false;
        agent = GetComponent<NavMeshAgent>(); // Tillåter agent att flytta på modellen via en gilltig NavMesh på scenen.
        
        InvokeRepeating("Searching", 0f, 0.5f ); //Startar metoden Searching vid sekund 0 och upprepar den var 0.5 sekund.
        InvokeRepeating("Roaming", 0f, 2f);      //Startar metoden Roaming vid sekund 0 och upprepar den var 2 sekund.
        targetPlayer = PlayerManager.instance.player.transform;
        timer = roamingTimer;
        timeForAttack = Time.time + timeBetweenAttacks;
        
        }


    void Searching()
        {
       
        float distanceToPlayer = Vector3.Distance(targetPlayer.position, transform.position);

        if (distanceToPlayer <= targetingRange && distanceToPlayer <= aggroRange)
        {

            if (distanceToPlayer <= attackRange && Time.time >= timeForAttack)
            {
                isAttacking = true;
                Attack();
                //attack
                if (distanceToPlayer <= noGoZone)
                {
                                   }
            }
            
            else
            {

                PlayerTargeted();
                agent.SetDestination(targetPlayer.position);
                playerInSight = true;
            }

        }
        else if (distanceToPlayer > aggroRange)
        {
            //print("Must have been my imagination");
            playerInSight = false;
            return; // Gå tillbaka till att roama.
        }
    }

    void Attack()
    {
        PlayerManager.instance.TakeDamage(5);
        timeForAttack = Time.time + timeBetweenAttacks;
    }

    void PlayerTargeted()
    {
        //print ("Hey you!");
        // Anim alarmed/approaching here

        var lookPos = targetPlayer.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

    }

    void Roaming() //Denna uppdateras automatiskt via searching() som i sin tur uppdateras via invoke repeat vid start.
    {
        if (playerInSight == true)
        {
            return;
        }
        //print("Whistle*");
        // Anim patrol here
        timer += Time.deltaTime;

        if (timer >= roamingTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, roamingRadius, 1);
            agent.SetDestination(newPos);
            roamingTimer = 0;
            
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }


    public void TakeDMG(int Damage)
    {
        /// Precis som metoden heter så ska skadan in här, 
        /// tänk på att armor och HP ska vara aktiva variabler här.
        currentHP -= Damage;
        if(currentHP <= 0)
        {
            //Death anim 
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected () //Ritar ut wirespheres när modellen väls i scenen. De Visar hur lång de olika rangen är.
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, noGoZone);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, roamingRadius);
    }

}
