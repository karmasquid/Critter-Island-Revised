using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingState : IState {


    private LayerMask searchLayer;
    private LayerMask obstacleLayer;
    private EnemyStats enemyStats;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform ownerGo;
    private Transform playerGo;
    private List<Transform> waypoints;
    private float attackRangeMax;
    private float viewRange;
    private float distanceToPlayer;
    private float wait;
    private float rotateWait;
    private float rotationspeed = 2;
    private float viewDeg;
    private int index = 0;
    private Vector3 positionToCheck;

    private bool roaming;
    private bool rotating;
    private bool nextPosSet;

    private System.Action<Results> roamingResultsCallback;

    public RoamingState(MeleeEnemy ai)
    {
        this.searchLayer = ai.PlayerLayer;
        this.obstacleLayer = ai.ObstacleLayer;
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGo = ai.transform;
        this.playerGo = ai.Player;
        this.attackRangeMax = ai.AttackRangeMax;
        this.viewRange = ai.ViewRange;
        this.roamingResultsCallback = ai.NextState;
        this.anim = ai.Anim;
        this.waypoints = ai.Waypoints;
        this.viewDeg = ai.ViewDeg;
        this.positionToCheck = ai.WaypointsMidPos;
    }

    public RoamingState(RangeEnemy ai)
    {
        this.searchLayer = ai.PlayerLayer;
        this.obstacleLayer = ai.ObstacleLayer;
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGo = ai.transform;
        this.playerGo = ai.Player;
        this.attackRangeMax = ai.AttackRangeMax;
        this.viewRange = ai.ViewRange;
        this.roamingResultsCallback = ai.NextState;
        this.anim = ai.Anim;
        this.waypoints = ai.Waypoints;
        this.viewDeg = ai.ViewDeg;
        this.positionToCheck = ai.WaypointsMidPos;
    }

    public void Enter()
    {
        wait = Time.time;
    }

    public void Execute()
    {
        RoamToPos();
        SearchForPlayer();
    }

    public void Exit()
    {

    }

    private void RoamToPos()
    {

        if (!roaming && !rotating && Time.time > wait)
        {
            navMeshAgent.SetDestination(waypoints[index].position);
            anim.SetBool("isWalking", true);

            wait = Time.time + 1f;

            if (index + 1 <= waypoints.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }

            positionToCheck = waypoints[index].position;

            roaming = true;

            Debug.Log("roaming destination set.");

        }
        else if (roaming && Time.time > wait)
        {
            if (!navMeshAgent.pathPending)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {

                    wait = Time.time + 1f;
                    rotateWait = Time.time + 0.5f;
                    //waitBeforemoving = Time.time + 3f;
                    roaming = false;
                    rotating = true;
                    anim.SetBool("isWalking", false);
                }
            }
        }

        else if (!roaming && rotating && Time.time > rotateWait)
        {
            anim.SetBool("isWalking", true);
            Debug.Log("rotating");
            RotateTowards();

            if (Time.time > wait)
            {
                anim.SetBool("isWalking", false);
                rotating = false;
                roaming = false;
                wait = Time.time + 1.5f;
            }
        }
    }

    private void SearchForPlayer()
    {
        Collider[] foundTargets = Physics.OverlapSphere(ownerGo.transform.position, this.viewRange, this.searchLayer);

        for (int i = 0; i < foundTargets.Length; i++)
        {
            if (foundTargets[i].CompareTag("Player"))
            {
                var target = foundTargets[i].transform;
                var targetDistance = Vector3.Distance(ownerGo.transform.position, target.position);
                var targetDirection = (target.position - ownerGo.position).normalized;

                if (Vector3.Angle(ownerGo.forward, targetDirection) < viewDeg / 2)
                {
                    if (!Physics.Raycast(ownerGo.position, targetDirection, targetDistance, obstacleLayer))
                    {
                        var roamingResults = new Results(1);
                        this.roamingResultsCallback(roamingResults);
                    }
                }
                if (targetDistance < attackRangeMax)
                {
                    var roamingResults = new Results(1);
                    this.roamingResultsCallback(roamingResults);
                }
            }
        }
    }

    private void RotateTowards()
    {
        Debug.Log("rotating");
        //move to next position in list.
        Vector3 targetDir = positionToCheck - ownerGo.position;

        // The step size is equal to speed times frame time.
        float step = rotationspeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(ownerGo.forward, targetDir, step, 0.0f);

        // Move our position a step closer to the target.
        ownerGo.rotation = Quaternion.LookRotation(newDir);

    }

}
