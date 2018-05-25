using System.Collections;
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
    private Transform waypointParent;
    private Transform hand;
    private Vector3 waypointsMid;
    private List<Transform> waypoints = new List<Transform>();

    private bool dead;
    private NavMeshAgent navMeshAgent;
    private StateMachine stateMachine = new StateMachine();

    private PlayerManager playerManager;
    private EnemyStats enemyStats;
    private EnemyProjectile projectileScript;

    private float originalViewRange;

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
    public List<Transform> Waypoints { get { return waypoints; } }
    public Vector3 WaypointsMidPos { get { return waypointsMid; } }

    public Vector3 DirectionsFromDegrees(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += this.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Awake()
    {
        aitransform = this.gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        enemyStats = GetComponent<EnemyStats>();
        EnemyStats.Health = health;
        EnemyStats.Damage = damage;

        hand = gameObject.transform.Find("root_JNT/spine_1_JNT/spine_2_JNT/chest_JNT/torso_JNT/R_clavicle_JNT/R_shoulder_JNT/R_elbow_JNT/R_forearm_JNT/R_hand_JNT");

        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.anim = this.GetComponent<Animator>();

        this.waypointParent = this.aitransform.Find("WayPoints");
        startPos = transform.position;
        startRot = transform.rotation;
        originalViewRange = viewRange;
        

    }

    private void Start()
    {
        if (waypointParent != null)
        {
            //int indexuru = 0;
            int childCount = waypointParent.childCount;

            for (int i = 1; i <= childCount; i++)
            {
                waypoints.Add(waypointParent.GetChild(i - 1).GetComponent<Transform>());
                Debug.Log(waypoints[i - 1].transform.name + "  ADDED");
            }

            foreach (Transform child in waypoints)
            {
                child.parent = null;
                waypointsMid += child.position;
            }

            waypointsMid /= childCount;

            this.stateMachine.ChangeState(new IdleState(this));
        }

        else
        {
            this.stateMachine.ChangeState(new IdleState(this));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!EnemyStats.Dead && !playerManager.dead)
        {
            this.stateMachine.ExecuteStateUpdate();
        }

        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("isWalking"))
            {
                anim.SetBool("isWalking", false);
            }
        }
    }

    public void NextState(Results stateResult)
    {
        switch (stateResult.nextState)
        {
            //SearchFor
            case 1:
                viewRange = originalViewRange;
                this.stateMachine.ChangeState(new RangeAttackState(this));
                break;

            case 2:
                viewRange = originalViewRange;
                this.stateMachine.ChangeState(new RangeAttackState(this));
                break;

            case 3:
                this.stateMachine.ChangeState(new IdleState(this));
                break;

            case 4:
                this.stateMachine.ChangeState(new HurtState(this));
                break;

            case 5:
                viewRange = attackRangeMax;
                anim.SetBool("isWalking", false);
                this.stateMachine.ChangeState(new AttackState(this));

                break;

            default:
                Debug.Log(transform.name + " is trying to switch to a state that doesn't exist !");
                break;
        }

    }
 
    public void ShootProjectile()
    {
        StartCoroutine(Throw());

    }

    private IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject tempProj;

        if (projectiles.Count < 5)
        {
            tempProj = Instantiate(projectile, hand.position + transform.forward, this.transform.rotation, null) as GameObject;
            projectileScript = tempProj.GetComponent<EnemyProjectile>();
            projectileScript.Damage = damage;
            projectileScript.EnemyStats = enemyStats;
            tempProj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * projectileForce, ForceMode.Impulse);
            projectiles.Add(tempProj);
        }

        else
        {
            tempProj = projectiles[index];
            tempProj.SetActive(true);
            tempProj.transform.position = hand.position;
            tempProj.transform.rotation = this.transform.rotation;
            tempProj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            tempProj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * projectileForce, ForceMode.Impulse);

            if (index == 4)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

        yield break;
    }


    //public void RangeAttackDone(RangeAttackResult attackResults)
    //{   //attack     
    //    this.stateMachine.ChangeState(new IdleState(this));
    //}

    //public void IdleDone(IdleResult idleResult)
    //{   //attack     
    //    this.stateMachine.ChangeState(new RangeAttackState(this));
    //}
}// STina Hedman
