using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script for elderbrute, enemy.
[RequireComponent(typeof(NavMeshAgent))]
public class ElderBrute : MonoBehaviour {

    private StateMachine stateMachine = new StateMachine();

    private GameObject player;

    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private float attackRangeMin;
    [SerializeField]
    private float attackRangeMax;

    [Header("Fov range and degrees (0-360)")]
    [SerializeField]
    private float viewRange;
    [SerializeField]
    private float viewDeg;
    [SerializeField]
    private float roamRange;
    [SerializeField]
    private float roamTimeDelay;


    private NavMeshAgent navMeshAgent;

    public float AttackRangeMin {
    get
        {
            return attackRangeMin;
        }
    }
    public float AttackRangeMax
    {
        get
        {
            return attackRangeMax;
        }
    }

    public float ViewRange
    {
        get
        {
            return viewRange;
        }
    }
    public float ViewDeg
    {        get
        {
            return viewDeg;
        }
    }

    //for unityeditor to make the spaces visable
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
        player = GameObject.FindGameObjectWithTag("Player");
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.stateMachine.ChangeState(new SearchFor(this.playerLayer, this.obstacleLayer, this.gameObject, this.ViewRange, this.viewDeg,this.attackRangeMax, this.roamRange, this.roamTimeDelay, this.navMeshAgent, this.SearchDone));
    }

    private void Update()
    {
        this.stateMachine.ExecuteStateUpdate();
       
    }

    public void SearchDone(SearchResults searchResults)
    {
        var playertransform = searchResults.playerfound;
        var distanceFromAI = searchResults.distance;
        this.stateMachine.ChangeState(new AttackState(this.navMeshAgent,this.gameObject,this.player,this.attackRangeMin, this.attackRangeMax,this.AttackDone));
    }

    public void AttackDone(AttackResult attackResult)
    {
        var goToIdle = attackResult;
    }

    ////trigger the Attack
    //public PlayerInRange()
    //{
    //    //trigger the Attack
    //}




} // Stina Hedman