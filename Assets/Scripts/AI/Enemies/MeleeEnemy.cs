﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]

public class MeleeEnemy : MonoBehaviour {
    
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
    private Vector3 waypointsMid;
    private List<Transform> waypoints = new List<Transform>();
    
    private bool dead;
    private NavMeshAgent navMeshAgent;
    private StateMachine stateMachine = new StateMachine();

    PlayerManager playerManager;
    EnemyStats enemyStats;

    Animator anim;

    private Vector3 startPos;

    //used by editorscript to make the ranges visual & used by the different aistates
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
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.anim = this.GetComponent<Animator>();
        this.aitransform = this.gameObject.transform;
        this.waypointParent = this.aitransform.Find("Waypoints");
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyStats.ActionCallback = this.NextState;
        enemyStats.Health = health;
        enemyStats.Damage = damage;
    }

    private void Start()
    {
        if (waypointParent != null)
        {
            //count the childs of waypoints. add them to a list of waypoints then sets parent to null to keep their position in the scene.
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

            this.stateMachine.ChangeState(new RoamingState(this));
        }

        //if the enemy has no waypoints, go to idlestate.
        else
        {
            this.stateMachine.ChangeState(new IdleState(this));
        }
        
    }

    private void FixedUpdate()
    {
        //if player or enemy isn't dead, execute states update.
        if (!EnemyStats.Dead && !playerManager.dead)
        {
            this.stateMachine.ExecuteStateUpdate();
        }

        //if dead stop walking.
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
        //Handles the results returned from the different states and evaluates which new state to switch to.
        switch (stateResult.nextState)
        {
            case 1:
                this.stateMachine.ChangeState(new AttackState(this));
                break;
                
            case 2:
                this.stateMachine.ChangeState(new RoamingState(this));
                break;

            case 3:
                this.stateMachine.ChangeState(new IdleState(this));
                break;

            case 4:
                this.stateMachine.ChangeState(new HurtState(this));
                break;
        }

    }

} // Stina Hedman


public class Results
{
    public int nextState;

    public Results(int setNextState)
    {
        this.nextState = setNextState;
    }
}
