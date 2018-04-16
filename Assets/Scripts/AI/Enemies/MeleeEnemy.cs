using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]

public class MeleeEnemy : MonoBehaviour {
    /// make this an AI class.
    private Transform player;

    [SerializeField]
    private StateMachine stateMachine = new StateMachine();

    PlayerManager playerManager;
    EnemyStats enemyStats;

    Animator anim;

    //AI variables
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;

    [Header("Health and Damage")]
    [SerializeField]
    private Stats health;
    [SerializeField]
    private float damage;
    [Header("AttackSettings")]
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

    private Vector3 startPos;

    protected bool dead;

    private NavMeshAgent navMeshAgent;


    //for unityeditor to make the spaces visable
    public float AttackRangeMin { get { return attackRangeMin; } }
    public float AttackRangeMax { get { return attackRangeMax; } }
    public float ViewRange { get { return viewRange; } }
    public float ViewDeg { get { return viewDeg; } }

    public LayerMask PlayerLayer
    {
        get
        {
            return playerLayer;
        }

    }

    public LayerMask ObstacleLayer
    {
        get
        {
            return obstacleLayer;
        }

    }

    public Stats Health
    {
        get
        {
            return health;
        }

    }

    public float Damage
    {
        get
        {
            return damage;
        }

    }

    public float AttackRangeMin1
    {
        get
        {
            return attackRangeMin;
        }

    }

    public float AttackRangeMax1
    {
        get
        {
            return attackRangeMax;
        }

    }

    public float TimeBetweenAttacks
    {
        get
        {
            return timeBetweenAttacks;
        }

    }

    public float ViewRange1
    {
        get
        {
            return viewRange;
        }

    }

    public float ViewDeg1
    {
        get
        {
            return viewDeg;
        }

    }

    public float RoamRange
    {
        get
        {
            return roamRange;
        }
    }

    public float IdleTimeBetweenMoves
    {
        get
        {
            return idleTimeBetweenMoves;
        }
    }

    public PlayerManager PlayerManager
    {
        get
        {
            return playerManager;
        }

    }

    public EnemyStats EnemyStats
    {
        get
        {
            return enemyStats;
        }

    }

    public Animator Anim
    {
        get
        {
            return anim;
        }

    }

    public NavMeshAgent NavMeshAgent
    {
        get
        {
            return navMeshAgent;
        }

    }

    public Transform Player
    {
        get
        {
            return player;
        }

    }

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        enemyStats = GetComponent<EnemyStats>();
        if (EnemyStats == null)
        {
            Debug.Log("wuddaheck");
        }
        EnemyStats.Health = health;
        EnemyStats.Damage = damage;

        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.anim = this.GetComponent<Animator>();
        this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg, this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone, this.Anim));
    }

    private void Update()
    {
        if (!EnemyStats.Dead)
        {
            this.stateMachine.ExecuteStateUpdate();
        }

        else
        {
            Destroy(this.gameObject);
        }

    }


    //public void TakeDamange(int damageDealt)
    //{
    //    health -= damageDealt;

    //    if (health <= 0)
    //    {
    //        dead = true;

    //        //this.gameObject.transform.rotate(new Vector3())
    //    }
    //}

    //-------------------------------------------------------------- could be made into only one method with enum? -------------------------------------------------------------------
    //Next state after Searchstate
    public void SearchDone(SearchResult searchResult)
    {
        if (searchResult.trueForAttackFalseForIdle)
        {
            this.stateMachine.ChangeState(new AttackState(this));
        }

        else
        {
            //go to idle
            this.stateMachine.ChangeState(new IdleState(this.gameObject.transform, this.player, this.obstacleLayer, this.playerLayer, this.viewRange, this.ViewDeg, this.attackRangeMax, this.idleTimeBetweenMoves, this.IdleDone, this.Anim));
            //currently going to roaming.
        }

    }
    //Next state after AttackState
    public void AttackDone(AttackResult attackResults)
    {   //attack     
        if (attackResults.trueForSearchFalseForIdle)
        {
            this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg, this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone, this.Anim));
        }
        //idle
        else
        {
            this.stateMachine.ChangeState(new IdleState(this.gameObject.transform, this.player, this.obstacleLayer, this.playerLayer, this.viewRange, this.ViewDeg, this.attackRangeMax, this.idleTimeBetweenMoves, this.IdleDone, this.Anim));
        }
    }
    public void IdleDone(IdleResult idleResult)
    {   //attack     
        if (idleResult.trueForAttackFalseForSearch)
        {
            this.stateMachine.ChangeState(new AttackState(this));
        }
        //idle
        else
        {
            this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg, this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone, this.Anim));
        }
    }//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
} // Stina Hedman
