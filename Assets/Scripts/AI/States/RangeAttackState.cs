using System;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackState : IState {


    private PlayerManager playerManager;
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

    private int damage;
    private bool rotating;
    private bool moving;
    private bool attacking;

    public bool attackComplete;

    private System.Action<RangeAttackResult> rangeAttackResultCallback;

    public RangeAttackState(NavMeshAgent navMeshAgent, Transform ownerGO, Transform playerGO, float attackRangeMin, float attackRangeMax, float viewRange, Action<RangeAttackResult> rangeAttackResultCallback, Animator anim, float timeBetweenAttacks, PlayerManager playerManager, int damage)
    {
        this.navMeshAgent = navMeshAgent;
        this.ownerGO = ownerGO;
        this.playerGO = playerGO;
        this.attackRangeMin = attackRangeMin;
        this.attackRangeMax = attackRangeMax;
        this.viewRange = viewRange;
        this.rangeAttackResultCallback = rangeAttackResultCallback;
        this.anim = anim;
        this.timeBetweenAttacks = timeBetweenAttacks;
        this.playerManager = playerManager;
        this.damage = damage;
    }

    void IState.Enter()
    {
        Debug.Log("Entered attackstate");
        this.startPos = ownerGO.position;
        moving = false;
    }

    void IState.Execute()
    {
        if (!attackComplete)
        {
            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);
            var direction = (this.ownerGO.position - this.playerGO.position);

            //move towards the enemy if its too far away
            if (distanceBetween < attackRangeMin)
            {
                //ANIMATION FOR IDLE
                if (!rotating)
                {
                    RotateTowards();
                    rotating = true;
                }
                //ownerGO.transform.Translate((-this.ownerGO.forward) * (this.navMeshAgent.speed+4.0f) * Time.deltaTime);
            }
            else if (distanceBetween > attackRangeMin && distanceBetween <= attackRangeMax && Time.time > waitAttack && !rotating)
            {
                if (moving)
                {
                    anim.SetBool("isWalking", false);
                    moving = false;
                }

                anim.SetTrigger("attack");

                this.rotating = false;
                this.navMeshAgent.enabled = false;
                if (!rotating)
                {
                    RotateTowards();
                }

                waitAttack = Time.time + timeBetweenAttacks;
                
                playerManager.TakeDamage(damage);
            }
            else if (distanceBetween > attackRangeMax && distanceBetween <= viewRange)
            {
                this.navMeshAgent.enabled = true;
                RotateTowards();
                if (!moving)
                {
                    attacking = false;
                    moving = true;
                    anim.SetBool("isWalking", true);
                }
                waitMove = Time.time + timeBetweenMoves;
                this.navMeshAgent.SetDestination(this.playerGO.position);
            }
            else if (Time.time > waitMove)
            {
                var rangeAttackResult = new RangeAttackResult(true);
                this.rangeAttackResultCallback(rangeAttackResult);
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
        rotating = false;
    }

}
public class RangeAttackResult
{
    public bool trueForSearchFalseForIdle;

    public RangeAttackResult(bool trueForSearchFalseForIdle)
    {
        this.trueForSearchFalseForIdle = trueForSearchFalseForIdle;
    }
}
// Stina Hedman
