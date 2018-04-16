using System;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackState : IState {

    private EnemyStats enemyStats;

    private PlayerManager playerManager;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform ownerGO;
    private Transform playerGO;
    private GameObject projectile;
    private Vector3 startPos;
    private float attackRangeMin;
    private float attackRangeMax;
    private float viewRange;

    private float timeBetweenAttacks;
    private float waitAttack;

    private float timeBetweenMoves = 4f;
    private float waitMove;

    private bool rotating;
    private bool attacking;

    private Vector3 direction;

    private Quaternion startRotation;

    public bool attackComplete;

    private RangeEnemy rangeEnemyScript;

    private System.Action<RangeAttackResult> rangeAttackResultCallback;

    public RangeAttackState(RangeEnemy ai)
    {
        this.rangeEnemyScript = ai;
        this.projectile = ai.Projectile;
        this.navMeshAgent = ai.NavMeshAgent;
        this.ownerGO = ai.Aitransform;
        this.playerGO = ai.Player;
        this.attackRangeMin = ai.AttackRangeMin;
        this.attackRangeMax = ai.AttackRangeMax;
        this.viewRange = ai.ViewRange;
        this.rangeAttackResultCallback = ai.RangeAttackDone;
        this.anim = ai.Anim;
        this.timeBetweenAttacks = ai.TimeBetweenAttacks;
        this.enemyStats = ai.EnemyStats;
        this.startRotation = ai.StartRot;
    }

    void IState.Enter()
    {
        this.startPos = ownerGO.position;
        waitAttack = Time.time;
        this.navMeshAgent.enabled = false;
    }

    void IState.Execute()
    {
        if (!attackComplete)
        {
            var distanceBetween = Vector3.Distance(this.playerGO.position, this.ownerGO.position);
            direction = (this.ownerGO.position - this.playerGO.position);

            RotateTowards();

            if (distanceBetween <= attackRangeMax && Time.time > waitAttack)
            {
                if (!attacking )
                {              
                    attacking = true;
                }
                anim.SetTrigger("attack");
                Debug.Log("attacking!");

                rangeEnemyScript.ShootProjectile();

                this.waitAttack = Time.time + timeBetweenAttacks;

                //Instantiate(projectile, ownerGO, false);
                
            }
            else if (distanceBetween > attackRangeMax && distanceBetween <= viewRange)
            {
                if (attacking)
                {
                this.attacking = false;
                }
                this.waitMove = Time.time + timeBetweenMoves;
            }
            else if (distanceBetween > viewRange && Time.time > waitMove)
            {
                var rangeAttackResult = new RangeAttackResult(true);
                this.rangeAttackResultCallback(rangeAttackResult);
                this.attackComplete = true;
            }
        }
    
    }

    void IState.Exit()
    {
        this.navMeshAgent.enabled = true;
    }   

    private void Attack()
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

public class RangeAttackResult
{
    public bool trueForSearchFalseForIdle;

    public RangeAttackResult(bool trueForSearchFalseForIdle)
    {
        this.trueForSearchFalseForIdle = trueForSearchFalseForIdle;
    }
}
// Stina Hedman
