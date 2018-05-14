using System;
using UnityEngine;
using UnityEngine.AI;

public class SearchFor : IState {

    private Animator anim;

    private NavMeshAgent navMeshAgent;

    private LayerMask searchLayer;
    private LayerMask obstacleLayer;

    private Transform ownerGo;
    private Transform target;

    private float viewRange;
    private float viewDeg; 
    private float roamRadius;
    private float attackRange;
    private string tagToLookFor = "Player";

    public bool searchCompleted;

    //tempvariables.
    private bool roaming;
    private float wait = 5f;
    private float waitBetweenRoam = 2f;
    private float waittime;
    private float waitroam;

    private Vector3 roamPos;

    private System.Action<Results> searchResultCallback;

    //int triedToFind = 0;

    public SearchFor(MeleeEnemy meleeEnemy)
    {

        this.searchLayer = meleeEnemy.PlayerLayer;
        this.obstacleLayer = meleeEnemy.ObstacleLayer;
        this.ownerGo = meleeEnemy.Aitransform;
        this.viewRange = meleeEnemy.ViewRange;
        this.viewDeg = meleeEnemy.ViewDeg; 
        this.roamRadius = meleeEnemy.RoamRange;
        this.navMeshAgent = meleeEnemy.NavMeshAgent;
        this.searchResultCallback = meleeEnemy.NextState;
        this.attackRange = meleeEnemy.AttackRangeMax;
        this.anim = meleeEnemy.Anim;
    }
    //public SearchFor(RangeEnemy rangeEnemy)
    //{

    //    this.searchLayer = rangeEnemy.PlayerLayer;
    //    this.obstacleLayer = rangeEnemy.ObstacleLayer;
    //    this.ownerGo = rangeEnemy.Aitransform;
    //    this.viewRange = rangeEnemy.ViewRange;
    //    this.viewDeg = rangeEnemy.ViewDeg;
    //    this.roamRadius = rangeEnemy.RoamRange;
    //    this.navMeshAgent = rangeEnemy.NavMeshAgent;
    //    this.searchResultCallback = rangeEnemy.SearchDone;
    //    this.attackRange = rangeEnemy.AttackRangeMax;
    //    this.anim = rangeEnemy.Anim;
    //}

    public void Enter()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("isWalking"))
        {
            anim.SetBool("isWalking", false);
        }
        roaming = false;
    }

    public void Execute()
    {
        if (!searchCompleted)
        {
            SearchForPlayer();
            Roam();
        }
    }

    public void Exit()
    {

    }

    private void SearchForPlayer()
    {
        Collider[] foundTargets = Physics.OverlapSphere(ownerGo.transform.position, this.viewRange, this.searchLayer);

        for (int i = 0; i < foundTargets.Length; i++)
        {
            if (foundTargets[i].CompareTag(this.tagToLookFor))
            {   target = foundTargets[i].transform;
                var targetDistance = Vector3.Distance(ownerGo.transform.position, target.position);
                var targetDirection = (target.position - ownerGo.position).normalized;

                if (Vector3.Angle(ownerGo.forward, targetDirection) < viewDeg / 2)
                {
                    if (!Physics.Raycast(ownerGo.position, targetDirection, targetDistance, obstacleLayer))
                    {   var searchResult = new Results(1);
                        this.searchResultCallback(searchResult);
                        this.searchCompleted = true;
                    }
                }
                if (targetDistance < attackRange)
                {   var searchResult = new Results(1);
                    this.searchResultCallback(searchResult);
                    this.searchCompleted = true;
                }
            }
        }
    }

    private void Roam()
    {
        if (!roaming)
        {           
            Vector3 roamingPoint;

            if (RoamToPoint(ownerGo.position, roamRadius, out roamingPoint))
            {
                anim.SetBool("isWalking", true);
                navMeshAgent.SetDestination(roamingPoint);
                roaming = true;
                waittime = Time.time + wait;
                waitroam = waittime + waitBetweenRoam;
                
            }
            
        }
        if (roaming && navMeshAgent.remainingDistance < 0.1|| Time.time > waittime)
        {
            anim.SetBool("isWalking", false);
            if (Time.time > waitroam)
            roaming = false;

        }
    }

    bool RoamToPoint(Vector3 center, float range, out Vector3 result)
    {
        
        Vector3 randomRoamPos = center + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomRoamPos, out navHit, range, NavMesh.AllAreas))
        {   result = navHit.position;
            roamPos = navHit.position;
            anim.SetBool("isWalking", true);
            return true;
        }

        result = Vector3.zero;
        return false;
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
//public class SearchResult
//{
//    public bool trueForAttackFalseForIdle;

//    public SearchResult(bool trueForAttackFalseForIdle)
//    {
//        this.trueForAttackFalseForIdle = trueForAttackFalseForIdle;
//    }
//} 
//Stina Hedman
