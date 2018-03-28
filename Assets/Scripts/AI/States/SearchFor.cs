using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchFor : IState {

    private LayerMask searchLayer;
    private LayerMask obastacleLayer;
    private GameObject ownerGo;
    private Transform target;
    private float viewRange;
    private float viewDeg; 
    private float roamRadius;
    private float attackRange;
    private string tagToLookFor = "Player";
    private NavMeshAgent navMeshAgent;
    private float timeToWait = 5f;
    private float time;
    private bool roaming = false;
    private bool found;
    private Vector3 directionFromAngle;
    Vector3 distance;
    public bool searchCompleted;
    private System.Action<SearchResults> searchResultCallback;



    //int triedToFind = 0;

    public SearchFor(LayerMask searchLayer, LayerMask obstacleLayer, GameObject ownerGo, float viewRange, float viewDeg, float attackRange, float roamRadius, float timeBetweenMove, NavMeshAgent navMeshAgent, Action<SearchResults> searchResultsCallback)
    {

        this.searchLayer = searchLayer;
        this.obastacleLayer = obstacleLayer;
        this.ownerGo = ownerGo;
        this.viewRange = viewRange;
        this.viewDeg = viewDeg; 
        this.roamRadius = roamRadius;
        this.navMeshAgent = navMeshAgent;
        this.timeToWait = timeBetweenMove;
        this.searchResultCallback = searchResultsCallback;
        this.attackRange = attackRange;
    }

    private void SearchForPlayer()
    {
        Collider[] foundTargets = Physics.OverlapSphere(ownerGo.transform.position, this.viewRange, this.searchLayer);

        for (int i = 0; i < foundTargets.Length; i++)
        {
            if (foundTargets[i].CompareTag(this.tagToLookFor))
            {
                
                target = foundTargets[i].transform;

                var targetDistance = Vector3.Distance(ownerGo.transform.position, target.position);

                var targetDirection = (target.transform.position - ownerGo.transform.position).normalized;

                if (Vector3.Angle(ownerGo.transform.forward, targetDirection) < viewDeg / 2)
                {
                    if (!Physics.Raycast(ownerGo.transform.position, targetDirection, targetDistance, obastacleLayer))
                    {
                        var searchResults = new SearchResults(target, targetDistance);

                        this.searchResultCallback(searchResults);

                        Debug.Log("found target");

                        this.searchCompleted = true;
                    }
                }

                if (targetDistance < attackRange)
                {
                    this.navMeshAgent.isStopped = true;

                    var searchResults = new SearchResults(target, targetDistance);

                    this.searchResultCallback(searchResults);

                    Debug.Log("found target");

                    this.searchCompleted = true;
                }
            }

            else
            {
                found = false;
            }
        }
    }

    public void Enter()
    {
        Debug.Log("enterstatesearchfor");
    }

    public void Execute()
    {
        if (!searchCompleted)
        {
            SearchForPlayer();

            if (found == false)
            {
                if (roaming == false)
                {
                this.navMeshAgent.SetDestination(UnityEngine.Random.insideUnitSphere * roamRadius);
                Debug.Log("roaming");
                roaming = true;
                time = Time.time + timeToWait;
                }
                else if (Time.time >= time)
                {
                roaming = false;

                }
            }
        }

    }

    public void Exit()
    {

    }
}
public class SearchResults
{
    public bool attack;

    public Transform playerfound;

    public float distance;

    public SearchResults(Transform player, float distanceToPlayer)
    {
        this.playerfound = player;
        this.distance = distanceToPlayer;
    }
}
