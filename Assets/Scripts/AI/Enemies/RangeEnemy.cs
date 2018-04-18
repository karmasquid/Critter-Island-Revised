using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]

public class RangeEnemy : MonoBehaviour
{
    List<GameObject> projectiles = new List<GameObject>();
    int index = 0;
    bool dying;

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;

    [Header("Health and Damage")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [Header("AttackSettings")]
    [SerializeField]
    private float projectileForce;
    [SerializeField]
    private float attackRangeMin;
    [SerializeField]
    private float attackRangeMax;
    [SerializeField]
    private float timeBetweenAttacks;

    [Header("FOV-Range and degrees (0-360)")]
    [SerializeField]
    private float viewRange;
    [SerializeField]
    private float viewDeg;
    [SerializeField]
    private float roamRange;
    [SerializeField]
    private float idleTimeBetweenMoves;

    private Transform player;
    private Transform aitransform;

    private bool dead;
    private NavMeshAgent navMeshAgent;
    private StateMachine stateMachine = new StateMachine();

    private PlayerManager playerManager;
    private EnemyStats enemyStats;
    private EnemyProjectile projectileScript;

    Animator anim;

    private Vector3 startPos;
    private Quaternion startRot;

    //used by editorscript to make the ranges visual & used by the different aistates
    public GameObject Projectile { get { return projectile; } }
    public float AttackRangeMin { get { return attackRangeMin; } }
    public float AttackRangeMax { get { return attackRangeMax; } }
    public float ViewRange { get { return viewRange; } }
    public float ViewDeg { get { return viewDeg; } }
    public LayerMask PlayerLayer { get { return playerLayer; } }
    public LayerMask ObstacleLayer { get { return obstacleLayer; } }
    public float Health { get { return health; } }
    public float Damage { get { return damage; } }
    public float TimeBetweenAttacks { get { return timeBetweenAttacks; } }
    public float RoamRange { get { return roamRange; } }
    public float IdleTimeBetweenMoves { get { return idleTimeBetweenMoves; } }
    public PlayerManager PlayerManager { get { return playerManager; } }
    public EnemyStats EnemyStats { get { return enemyStats; } }
    public Animator Anim { get { return anim; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public Transform Player { get { return player; } }
    public Transform Aitransform { get { return aitransform; } }
    public Vector3 StartPos { get { return startPos; } }
    public Quaternion StartRot { get { return startRot; } }

    public Vector3 DirectionsFromDegrees(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += this.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Start()
    {
        aitransform = this.gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        enemyStats = GetComponent<EnemyStats>();
        EnemyStats.Health = health;
        EnemyStats.Damage = damage;

        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.anim = this.GetComponent<Animator>();
        this.stateMachine.ChangeState(new IdleState(this));
        startPos = transform.position;
        startRot = transform.rotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (!EnemyStats.Dead)
        {
            this.stateMachine.ExecuteStateUpdate();
        }

        else
        {

        }


    }

    //Next state after Searchstate
    //public void SearchDone(SearchResult searchResult)
    //{
    //    if (searchResult.trueForAttackFalseForIdle)
    //    {
    //        this.stateMachine.ChangeState(new AttackState(this));
    //    }

    //    else
    //    {
    //        //go to idle
    //        this.stateMachine.ChangeState(new IdleState(this));
    //        //currently going to roaming.
    //    }
    //}
    public void ShootProjectile()
    {
        GameObject tempProj;

        if (projectiles.Count < 5)
        {
            tempProj = Instantiate(projectile,new Vector3(this.transform.position.x,this.transform.position.y+0.7f,this.transform.position.z)+transform.forward,this.transform.rotation,null) as GameObject;
            projectileScript = tempProj.GetComponent<EnemyProjectile>();
            projectileScript.Damage = damage;
            projectileScript.EnemyStats = enemyStats;
            tempProj.GetComponent<Rigidbody>().AddForce(tempProj.transform.forward * projectileForce, ForceMode.Impulse);
            projectiles.Add(tempProj);
        }

        else
        {
            tempProj = projectiles[index];
            tempProj.SetActive(true);
            tempProj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z) + transform.forward;
            tempProj.transform.rotation = this.transform.rotation;
            tempProj.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
            tempProj.GetComponent<Rigidbody>().AddForce(tempProj.transform.forward * projectileForce, ForceMode.Impulse);

            if (index == 4)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

    }


    public void RangeAttackDone(RangeAttackResult attackResults)
    {   //attack     
        this.stateMachine.ChangeState(new IdleState(this));
    }

    public void IdleDone(IdleResult idleResult)
    {   //attack     
        this.stateMachine.ChangeState(new RangeAttackState(this));
    }
}
