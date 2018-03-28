using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IState {

    private NavMeshAgent navMeshAgent;
    private GameObject ownerGO;
    private GameObject playerGO;
    private float attackRangeMin;
    private float attackRangeMax;
    private float viewRange;
    public bool attacking;
    private System.Action<AttackResult> attackResultCallback;

    public AttackState(NavMeshAgent navMeshAgent, GameObject ownerGO, GameObject playerGO, float attackRangeMin, float attackRangeMax, Action<AttackResult> attackResultCallback)
    {
        this.navMeshAgent = navMeshAgent;
        this.ownerGO = ownerGO;
        this.playerGO = playerGO;
        this.attackRangeMin = attackRangeMin;
        this.attackRangeMax = attackRangeMax;
        this.attackResultCallback = attackResultCallback;
    }

    void IState.Enter()
    {
        Debug.Log("attacking");
    }

    void IState.Execute()
    {
        if (!attacking)
        {
            var distanceBetween = Vector3.Distance(playerGO.transform.position, ownerGO.transform.position);
            //move towards the enemy if its too far away
            if (distanceBetween > attackRangeMax && distanceBetween < viewRange)
            {
                var lookPos = playerGO.transform.position - ownerGO.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                ownerGO.transform.rotation = Quaternion.Slerp(ownerGO.transform.rotation, rotation, Time.deltaTime * 2);
                navMeshAgent.SetDestination(playerGO.transform.position);
            }
            //else start attacking.
            else if (distanceBetween < attackRangeMax && distanceBetween > attackRangeMin)
            {
           
                Debug.Log("dödadödadödadödadöda");
                //attackera rövhatten.
                
            }
            else
            {
                var attackresult = new AttackResult(true);
                this.attackResultCallback(attackresult);
                attacking = false;
            }
        }

    }

    void IState.Exit()
    {
        Debug.Log("stopped attacking");
    }

}
public class AttackResult
{
    public bool goToIdle;

    public AttackResult(bool goToIdle)
    {
        this.goToIdle = goToIdle;
    }
}
