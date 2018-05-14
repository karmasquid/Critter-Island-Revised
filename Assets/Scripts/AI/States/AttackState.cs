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
    private float attackRangeMin;
    private float attackRangeMax;
    private float viewRange;

    private float timeBetweenAttacks;
    private float waitAttack;

    private float timeBetweenMoves = 4f;
    private float waitMove;
    private float timeUntillDealDamage = 0.3f;
    private float waitDealDamage;

    //private float waitDealingDamage;
    //private float timeBeforeDealingDamage = 0.5f;

    //private bool rotating;
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
        this.attackRangeMin = ai.AttackRangeMin;
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
        this.attackRangeMin = ai.AttackRangeMin;
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
        waitAttack = Time.time + timeBetweenAttacks;
        anim.SetBool("isWalking", false);
    }

    void IState.Execute()
    {
        if (!attackComplete)
        {

            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);
            direction = (this.ownerGO.position + this.playerGO.position);

            //move towards the enemy if its too far away
            //if (distanceBetween < attackRangeMin)
            //{

               
            //}
            if (distanceBetween <= attackRangeMax)
            {
                if (!attacking && Time.time > waitAttack)
                {
                    RotateTowards();
                    moving = false;
                    this.navMeshAgent.isStopped = true;
                }

                //Check ifall det är tillåtet att attackera igen
                if (Time.time > waitAttack && enemyStats.PlayerInRange == true)
                {
                    RotateTowards();
                    anim.SetTrigger("attack");
                    waitAttack = Time.time + timeBetweenAttacks;
                    waitDealDamage = Time.time + timeUntillDealDamage;
                    executingAttack = true;
                }

                //check inför att göra skadan så den inte görs direkt. Kolla så att spelaren är inom attackcollider. Denna ska användas sen.
                if (Time.time > timeUntillDealDamage && executingAttack &&  enemyStats.PlayerInRange == true)
                {
                    enemyStats.DealDamage();
                    executingAttack = false;
                }
            }
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

            else if  (distanceBetween >= viewRange && Time.time > waitMove)
            {
                anim.SetBool("isWalking", false);
                var attackResults = new Results(2);
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
        Debug.Log("rotating");
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
