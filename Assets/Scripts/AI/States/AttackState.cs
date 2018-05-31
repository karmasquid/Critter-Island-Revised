using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IState {

    private EnemyStats enemyStats;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform ownerGO;
    private Transform playerGO;
    private Vector3 startPos;
    private float attackRangeMax;
    private float viewRange;

    private float timeBetweenAttacks;
    private float waitAttack;

    private float timeBetweenMoves = 4f;
    private float waitMove;
    private float timeUntillDealDamage = 0.3f;
    private float waitDealDamage;

    private bool moving;
    private bool attacking;
    private bool executingAttack;

    Vector3 direction;

    public bool attackComplete;

    private System.Action<Results> attackResultCallback;

    public AttackState(MeleeEnemy ai)
    {
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGO = ai.transform;
        this.playerGO = ai.Player;
        this.attackRangeMax = ai.AttackRangeMax;
        this.viewRange = ai.ViewRange;
        this.attackResultCallback = ai.NextState;
        this.anim = ai.Anim;
        this.timeBetweenAttacks = ai.TimeBetweenAttacks;
        this.enemyStats = ai.EnemyStats;
    }

    public AttackState(RangeEnemy ai)
    {
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGO = ai.transform;
        this.playerGO = ai.Player;
        this.attackRangeMax = ai.AttackRangeMax;
        this.viewRange = ai.ViewRange;
        this.attackResultCallback = ai.NextState;
        this.anim = ai.Anim;
        this.timeBetweenAttacks = ai.TimeBetweenAttacks;
        this.enemyStats = ai.EnemyStats;
    }

    void IState.Enter()
    {
        this.startPos = ownerGO.position;
        moving = true;
        waitAttack = Time.time;
        anim.SetBool("isWalking", false);
    }

    void IState.Execute()
    {
        if (!attackComplete)
        {
            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);
            direction = (this.ownerGO.position + this.playerGO.position);

            if (distanceBetween <= attackRangeMax)
            {
                if (!attacking && Time.time > waitAttack)
                {
                    RotateTowards();
                    moving = false;
                    this.navMeshAgent.isStopped = true;
                }

                //Check if its allowed to attack again
                if (Time.time > waitAttack && enemyStats.PlayerInRange == true)
                {
                    Debug.Log("attacking");
                    RotateTowards();
                    anim.SetTrigger("attack");
                    waitAttack = Time.time + timeBetweenAttacks;
                    waitDealDamage = Time.time + timeUntillDealDamage;
                    executingAttack = true;
                }

                //check for when to deal the damage after starting to attack.
                if (Time.time > timeUntillDealDamage && executingAttack &&  enemyStats.PlayerInRange == true)
                {
                    Debug.Log("hitting.");
                    enemyStats.DealDamage();
                    executingAttack = false;
                }
            }
            //if the player is outside attackrange. move towards it.
            else if (distanceBetween > attackRangeMax && distanceBetween <= viewRange)
            {
                if (!moving)
                {
                    this.navMeshAgent.enabled = true;
                    moving = true;
                    anim.SetBool("isWalking", true);
                    this.navMeshAgent.isStopped = false;
                }
                waitMove = Time.time + timeBetweenMoves;
                this.navMeshAgent.SetDestination(this.playerGO.position);
            }

            //if the player is outside the enemies viewrange, move back to RoamingState.
            else if  (distanceBetween >= viewRange && Time.time > waitMove)
            {
                anim.SetBool("isWalking", false);
                this.attackComplete = true;
                var attackResults = new Results(2);
                this.attackResultCallback(attackResults);

            }
        }
    }

    void IState.Exit()
    {
        anim.SetBool("isWalking", false);
    }


    private void RotateTowards()
    {
        float rotationspeed =3;
        Vector3 direction = (this.playerGO.position - this.ownerGO.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.ownerGO.transform.rotation = Quaternion.Slerp(this.ownerGO.rotation, lookRotation, Time.deltaTime * rotationspeed);
    }
}
//public class AttackResult
//{
//    public bool trueForSearchFalseForIdle;

//    public AttackResult(bool trueForSearchFalseForIdle)
//    {
//        this.trueForSearchFalseForIdle = trueForSearchFalseForIdle;
//    }
//}
// Stina Hedman
