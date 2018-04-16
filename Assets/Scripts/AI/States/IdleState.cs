using System;
using UnityEngine;

public class IdleState : IState {

    private Animator anim;

    public bool idleCompleted;
    public bool idleWait;

    private Transform ownerGo;
    private Transform playerGo;

    private LayerMask obstacleLayer;
    private LayerMask searchLayer;

    private float viewRange;
    private float viewDeg;
    private float attackRange;
    private float idleTime;
    private float time;

    private string tagToLookFor = "Player";

    private System.Action<IdleResult> idleResultsCallback;

    public IdleState(Transform ownerGo, Transform playerGo, LayerMask obstacleLayer, LayerMask playerLayer, float viewRange, float viewDeg, float attackRange, float idleTime, Action<IdleResult> idleResultsCallback, Animator anim)
    {
        this.ownerGo = ownerGo;
        this.playerGo = playerGo;
        this.obstacleLayer = obstacleLayer;
        this.searchLayer = playerLayer;
        this.viewRange = viewRange;
        this.viewDeg = viewDeg;
        this.attackRange = attackRange;
        this.idleResultsCallback = idleResultsCallback;
        this.idleTime = idleTime;
        this.anim = anim;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        Debug.Log("idle running");

        if (!idleCompleted)
        {
            Debug.Log("idletotallyrunning");

            LookForEnemy();

            Wait();
        }
    }

    public void Exit()
    {
        idleWait = false;
    }

    private void LookForEnemy()
    {
        Collider[] foundTargets = Physics.OverlapSphere(ownerGo.transform.position, this.viewRange, this.searchLayer);

        for (int i = 0; i < foundTargets.Length; i++)
        {
            if (foundTargets[i].CompareTag(this.tagToLookFor))
            {
                playerGo = foundTargets[i].transform;

                var targetDirection = (playerGo.position - ownerGo.position).normalized;
                var targetDistance = Vector3.Distance(ownerGo.transform.position, playerGo.position);


                if (Vector3.Angle(ownerGo.forward, targetDirection) < viewDeg / 2)
                {
                    if (!Physics.Raycast(ownerGo.position, targetDirection, targetDistance, obstacleLayer))
                    {
                        var idleResult = new IdleResult(true);
                        this.idleResultsCallback(idleResult);
                        this.idleCompleted = true;
                    }
                }

                if (targetDistance < attackRange)
                {
                    var idleResult = new IdleResult(true);
                    this.idleResultsCallback(idleResult);
                    this.idleCompleted = true;
                }
            }
        }
    }

    public void Wait()
    {
        if (!idleWait)
        {
            time = Time.time + idleTime;
            idleWait = true;
        }

        if (Time.time > idleTime)
        {
            var idleResult = new IdleResult(false);
            this.idleResultsCallback(idleResult);
            this.idleCompleted = true;
        }
    }
}
public class IdleResult
{
    public bool trueForAttackFalseForSearch;

    public IdleResult(bool trueForAttackFalseForSearch)
    {
        this.trueForAttackFalseForSearch = trueForAttackFalseForSearch;
    }


}// Stina Hedman
