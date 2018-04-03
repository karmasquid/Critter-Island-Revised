using UnityEngine;
using UnityEngine.AI;

//Script for elderbrute, enemy.
[RequireComponent(typeof(NavMeshAgent))]
public class ElderBrute : MonoBehaviour { 
    /// make this an AI class.
    private Transform player;

    [SerializeField]
    private StateMachine stateMachine = new StateMachine();

    PlayerManager playerManager;

    //AI variables
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;
    [Header("Health and Damage")]
    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [Header("Range of the attack")]
    [SerializeField]
    private float attackRangeMin;
    [SerializeField]
    private float attackRangeMax;

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

    private bool dead;

    private NavMeshAgent navMeshAgent;

    //for unityeditor to make the spaces visable
    public float AttackRangeMin {   get {   return attackRangeMin;  }   }
    public float AttackRangeMax {   get {   return attackRangeMax;  }   }
    public float ViewRange      {   get {   return viewRange;       }   }
    public float ViewDeg        {   get {   return viewDeg;         }   }

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
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg,this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone));
    }

    private void Update()
    {   if (!dead)
        {
            this.stateMachine.ExecuteStateUpdate();
        }
    
    }


    public void DealDamage()
    {
        playerManager.TakeDamage(this.damage);
    }
    public void TakeDamange(int damageDealt)
    {
        health -= damageDealt;

        if (health <= 0)
        {
            dead = true;
            


            //this.gameObject.transform.rotate(new Vector3())
        }
    }

    //-------------------------------------------------------------- could be made into only one method with enum? -------------------------------------------------------------------
    //Next state after Searchstate
    public void SearchDone(SearchResult searchResult)
    {
        if (searchResult.trueForAttackFalseForIdle)
        {
            this.stateMachine.ChangeState(new AttackState(this.navMeshAgent,this.gameObject.transform,this.player,this.attackRangeMin, this.attackRangeMax,this.viewRange,this.AttackDone));
        }

        else
        {
            //go to idle
            this.stateMachine.ChangeState(new IdleState(this.gameObject.transform,this.player,this.obstacleLayer,this.playerLayer,this.viewRange,this.ViewDeg,this.attackRangeMax,this.idleTimeBetweenMoves, this.IdleDone));
            //currently going to roaming.
        }

    }
    //Next state after AttackState
    public void AttackDone(AttackResult attackResults)
    {   //attack     
        if (attackResults.trueForSearchFalseForIdle)
        {
            this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg, this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone));
        }
        //idle
        else
        {
            this.stateMachine.ChangeState(new IdleState(this.gameObject.transform, this.player, this.obstacleLayer, this.playerLayer, this.viewRange, this.ViewDeg, this.attackRangeMax, this.idleTimeBetweenMoves, this.IdleDone));
        }
    }
    public void IdleDone(IdleResult idleResult)
    {   //attack     
        if (idleResult.trueForAttackFalseForSearch)
        {
            this.stateMachine.ChangeState(new AttackState(this.navMeshAgent, this.gameObject.transform, this.player, this.attackRangeMin, this.attackRangeMax, this.viewRange, this.AttackDone));
        }
        //idle
        else
        {
            this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject.transform, this.startPos, this.viewRange, this.viewDeg, this.attackRangeMax, this.roamRange, this.navMeshAgent, this.SearchDone));
        }
    }//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

} // Stina Hedman