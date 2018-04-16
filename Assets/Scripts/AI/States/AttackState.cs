using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IState {

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

    private System.Action<AttackResult> attackResultCallback;

    public AttackState(NavMeshAgent navMeshAgent, Transform ownerGO, Transform playerGO, float attackRangeMin, float attackRangeMax, float viewRange, Action<AttackResult> attackResultCallback, Animator anim, float timeBetweenAttacks, PlayerManager playerManager, int damage)
    {
        this.navMeshAgent = navMeshAgent;
        this.ownerGO = ownerGO;
        this.playerGO = playerGO;
        this.attackRangeMin = attackRangeMin;
        this.attackRangeMax = attackRangeMax;
        this.viewRange = viewRange;
        this.attackResultCallback = attackResultCallback;
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
            RotateTowards();
            //move towards the enemy if its too far away
            //if (distanceBetween < attackRangeMin)
            //{
            //    //ANIMATION FOR IDLE
            //    if (!rotating)
            //    {
            //        RotateTowards();
            //        rotating = true;
            //    }
            //    ownerGO.transform.Translate((-this.ownerGO.forward) * (this.navMeshAgent.speed+4.0f) * Time.deltaTime);
            //}
            if (distanceBetween > attackRangeMin && distanceBetween <= attackRangeMax && Time.time > waitAttack && !rotating)
            {
                if (moving)
                {
                    anim.SetTrigger("attack");
                    moving = false;
                    this.navMeshAgent.enabled = false;
                }

                waitAttack = Time.time + timeBetweenAttacks;

                if (Time.time < waitAttack)
                {
                    playerManager.TakeDamage(damage);
                }

               
            }
            else if (distanceBetween > attackRangeMax && distanceBetween <= viewRange && !rotating)
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
        float rotationspeed = 2 * Time.deltaTime;
        Vector3 direction = (this.playerGO.position - this.ownerGO.position);
        direction.y = 0;
        Vector3 newDir = Vector3.RotateTowards(this.playerGO.transform.forward, direction, rotationspeed, 0f);
        Debug.DrawRay(playerGO.transform.position, direction, Color.blue);
        this.ownerGO.transform.rotation = Quaternion.LookRotation(direction);
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
