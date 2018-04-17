using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]

public class RangeEnemy : MonoBehaviour {
    private Transform player;

    [SerializeField]
    private StateMachine stateMachine = new StateMachine();

    PlayerManager playerManager;

    Animator anim;

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

    public Vector3 DirectionsFromDegrees(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += this.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
