using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : MonoBehaviour , IState {

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

    //private float waitDealingDamage;
    //private float timeBeforeDealingDamage = 0.5f;

    //private bool rotating;
    private bool moving;
    private bool attacking;

    public bool attackComplete;

    private System.Action<AttackResult> attackResultCallback;

    public AttackState(MeleeEnemy meleeEnemy)
    {
        this.navMeshAgent = meleeEnemy.NavMeshAgent;
        this.ownerGO = meleeEnemy.transform;
        this.playerGO = meleeEnemy.Player;
        this.attackRangeMin = meleeEnemy.AttackRangeMin;
        this.attackRangeMax = meleeEnemy.AttackRangeMax;
        this.viewRange = meleeEnemy.ViewRange;
        this.attackResultCallback = meleeEnemy.AttackDone;
        this.anim = meleeEnemy.Anim;
        this.timeBetweenAttacks = meleeEnemy.TimeBetweenAttacks;
        this.enemyStats = meleeEnemy.EnemyStats;
    }

    void IState.Enter()
    {
        Debug.Log("Entered attackstate");
        this.startPos = ownerGO.position;
        moving = true;
        waitAttack = Time.time + timeBetweenAttacks;
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
            if (distanceBetween > attackRangeMin && distanceBetween <= attackRangeMax)
            {
                Debug.Log("attacking");

                moving = false;

                if (!attacking)
                {
                    this.navMeshAgent.enabled = false;
                    anim.SetTrigger("attack");
                }

                if (Time.time > waitAttack)
                {
                    waitAttack = Time.time + timeBetweenAttacks;
                    enemyStats.DealDamage();
                }


            }
            else if (distanceBetween > attackRangeMax && distanceBetween <= viewRange)
            {
                Debug.Log("moving towards");
                this.navMeshAgent.enabled = true;

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
    

    //FIXIT
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
