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

    private System.Action<Results> idleResultsCallback;

    public IdleState(MeleeEnemy ai)
    {
        this.ownerGo = ai.Aitransform;
        this.playerGo = ai.Player;
        this.obstacleLayer = ai.ObstacleLayer;
        this.searchLayer = ai.PlayerLayer;
        this.viewRange = ai.ViewRange;
        this.viewDeg = ai.ViewDeg;
        this.attackRange = ai.AttackRangeMax;
        this.idleResultsCallback = ai.NextState;
        this.idleTime = ai.IdleTimeBetweenMoves;
        this.anim = ai.Anim;
    }
    public IdleState(RangeEnemy ai)
    {
        this.ownerGo = ai.Aitransform;
        this.playerGo = ai.Player;
        this.obstacleLayer = ai.ObstacleLayer;
        this.searchLayer = ai.PlayerLayer;
        this.viewRange = ai.ViewRange;
        this.viewDeg = ai.ViewDeg;
        this.attackRange = ai.AttackRangeMax;
        this.idleResultsCallback = ai.NextState;
        this.idleTime = ai.IdleTimeBetweenMoves;
        this.anim = ai.Anim;
    }

    public void Enter()
    {
        Debug.Log("Entered idlestate");

    }

    public void Execute()
    {

        if (!idleCompleted)
        {
            LookForEnemy();
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
                        var idleResult = new Results(1);
                        this.idleResultsCallback(idleResult);
                        this.idleCompleted = true;
                    }
                }

                if (targetDistance < attackRange)
                {
                    var idleResult = new Results(1);
                    this.idleResultsCallback(idleResult);
                    this.idleCompleted = true;
                }
            }
        }
    }

    //private void RotateTowards()
    //{
    //    float rotationspeed = 10;
    //    Vector3 direction = (this.startPos - this.ownerGo.position).normalized;
    //    direction.y = 0;
    //    Quaternion lookRotation = Quaternion.LookRotation(direction);
    //    this.ownerGo.transform.rotation = Quaternion.Slerp(this.ownerGo.rotation, lookRotation, Time.deltaTime * rotationspeed);
    //}
}
//public class IdleResult
//{
//    public bool trueForAttackFalseForSearch;

//    public IdleResult(bool trueForAttackFalseForSearch)
//    {
//        this.trueForAttackFalseForSearch = trueForAttackFalseForSearch;
//    }


//}
// Stina Hedman
