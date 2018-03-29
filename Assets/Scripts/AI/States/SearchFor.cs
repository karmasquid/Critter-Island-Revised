using System;
using UnityEngine;
using UnityEngine.AI;

public class SearchFor : IState {

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

    private System.Action<SearchResult> searchResultCallback;



    //int triedToFind = 0;

    public SearchFor(LayerMask searchLayer, LayerMask obstacleLayer, Transform ownerGo, float viewRange, float viewDeg, float attackRange, float roamRadius, NavMeshAgent navMeshAgent, Action<SearchResult> searchResultCallback)
    {

        this.searchLayer = searchLayer;
        this.obstacleLayer = obstacleLayer;
        this.ownerGo = ownerGo;
        this.viewRange = viewRange;
        this.viewDeg = viewDeg; 
        this.roamRadius = roamRadius;
        this.navMeshAgent = navMeshAgent;
        this.searchResultCallback = searchResultCallback;
        this.attackRange = attackRange;
    }

   

    public void Enter()
    {
        Debug.Log("enterstatesearchfor");
    }

    public void Execute()
    {
        Debug.Log("Search running");

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
        Debug.Log("searching");
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
                    {   var searchResult = new SearchResult(true);
                        this.searchResultCallback(searchResult);
                        this.searchCompleted = true;
                    }
                }
                if (targetDistance < attackRange)
                {   var searchResult = new SearchResult(true);
                    this.searchResultCallback(searchResult);
                    this.searchCompleted = true;
                }
            }
        }
    }

    private void Roam()
    {
        {
            Vector3 roamingPoint;

            if (RoamToPoint(ownerGo.position, roamRadius, out roamingPoint))
            {
                Debug.Log("found roampos");
                navMeshAgent.SetDestination(roamingPoint);
                var searchResult = new SearchResult(false);
                this.searchResultCallback(searchResult);
                this.searchCompleted = true;
            }
        }
    }

    bool RoamToPoint(Vector3 center, float range, out Vector3 result)
    {
        
        Vector3 randomRoamPos = center + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomRoamPos, out navHit, range, NavMesh.AllAreas))
        {   result = navHit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
public class SearchResult
{
    public bool trueForAttackFalseForIdle;

    public SearchResult(bool trueForAttackFalseForIdle)
    {
        this.trueForAttackFalseForIdle = trueForAttackFalseForIdle;
    }
} 
//Stina Hedman
