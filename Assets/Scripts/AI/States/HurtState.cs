using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HurtState : IState
{

    float waitUntillDamage;

    float viewRange;

    Animator anim;
    NavMeshAgent navMeshAgent;

    Transform ownerGO;
    Transform playerGO;

    Vector3 navHitPos;

    EnemyStats enemyStats;

    float fleeTime;
    float timeBetweenFleeing = 2;

    private bool lowHealth;

    private System.Action<Results> hurtResultsCallback;

    public HurtState(MeleeEnemy ai)
    {
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGO = ai.transform;
        this.playerGO = ai.Player;
        this.hurtResultsCallback = ai.NextState;
        this.anim = ai.Anim;
        this.enemyStats = ai.EnemyStats;
        this.viewRange = ai.ViewRange;
    }

    public HurtState(RangeEnemy ai)
    {
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGO = ai.transform;
        this.playerGO = ai.Player;
        this.hurtResultsCallback = ai.NextState;
        this.anim = ai.Anim;
        this.enemyStats = ai.EnemyStats;
        this.viewRange = ai.ViewRange;
    }

    public void Enter()
    {
        timeBetweenFleeing = Time.time;

    }

    public void Execute()
    { 

        //ownerGO.rotation = Quaternion.LookRotation(ownerGO.position - playerGO.position);

        if (Time.time > timeBetweenFleeing)
        {
            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);

            if (distanceBetween < viewRange)
            {
                Debug.Log("fleeing");

                FleeToPoint();
            }

            else
            {
                navMeshAgent.isStopped = true;
                var hurtResults = new Results(1);
                this.hurtResultsCallback(hurtResults);
            }

        }

    }

    public void Exit()
    {

    }

    private void FleeToPoint()
    {

        NavMeshHit navHit;

        Vector3 dirToPlayer = Vector3.Normalize(ownerGO.position - playerGO.position);

        Vector3 newPos = (ownerGO.position - playerGO.position);

        NavMesh.SamplePosition(newPos, out navHit, 4, 1 << NavMesh.GetAreaFromName("Default"));
        Debug.Log("pewpew");

        navMeshAgent.SetDestination(newPos);

        Debug.Log(ownerGO.position + " ... " + navHit.position);

        fleeTime = Time.time + timeBetweenFleeing;

        anim.SetBool("isWalking", true);
    }

}
