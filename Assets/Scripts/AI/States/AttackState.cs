using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IState {

    private NavMeshAgent navMeshAgent;
    private Transform ownerGO;
    private Transform playerGO;
    private Vector3 startPos;
    private float attackRangeMin;
    private float attackRangeMax;
    private float viewRange;
    private bool moving;
    public bool attackComplete;
    private System.Action<AttackResult> attackResultCallback;

    public AttackState(NavMeshAgent navMeshAgent, Transform ownerGO, Transform playerGO, float attackRangeMin, float attackRangeMax, float viewRange, Action<AttackResult> attackResultCallback)
    {
        this.navMeshAgent = navMeshAgent;
        this.ownerGO = ownerGO;
        this.playerGO = playerGO;
        this.attackRangeMin = attackRangeMin;
        this.attackRangeMax = attackRangeMax;
        this.viewRange = viewRange;
        this.attackResultCallback = attackResultCallback;
    }

    void IState.Enter()
    {
        Debug.Log("Entered attackstate");
        this.startPos = ownerGO.position;
    }

    void IState.Execute()
    {
        Debug.Log("Attackstate");

        if (!attackComplete)
        {
            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);
            var direction = (this.ownerGO.position - this.playerGO.position);

            //move towards the enemy if its too far away
            if (distanceBetween <= attackRangeMin)
            {
                if (!moving)
                {
                    RotateTowards();
                    moving = true;
                }
                ownerGO.transform.Translate((-this.ownerGO.forward) * (this.navMeshAgent.speed+4.0f) * Time.deltaTime);
            }
            else if (distanceBetween >= attackRangeMin && distanceBetween <= attackRangeMax)
            {
                this.moving = false;
                this.navMeshAgent.enabled = false;
                RotateTowards();
                //----------------------------------------------------------------------------SWITCH THIS SHIT OUT LAT0RZ-----------------------------------------------------------------------------
                
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            else if (distanceBetween >= attackRangeMax && distanceBetween <= viewRange)
            {
                this.navMeshAgent.enabled = true;
                RotateTowards();
                this.navMeshAgent.SetDestination(this.playerGO.position);
            }
            else
            {
                var attackResults = new AttackResult(true);
                this.attackResultCallback(attackResults);
                this.attackComplete = false;
            }
        }
    }

    void IState.Exit()
    {

    }

    private void RotateTowards()
    {
        float rotationspeed = 10;
        Vector3 direction = (this.playerGO.position - this.ownerGO.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.ownerGO.transform.rotation = Quaternion.Slerp(this.ownerGO.rotation, lookRotation, Time.deltaTime * rotationspeed);
    }

}
public class AttackResult
{
    public bool trueForSearchFalseForIdle;

    public AttackResult(bool trueForSearchFalseForIdle)
    {
        this.trueForSearchFalseForIdle = trueForSearchFalseForIdle;
    }
}// Stina Hedman
