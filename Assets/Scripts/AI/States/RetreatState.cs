using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatState : IState {

    private LayerMask obstacleLayer;
    private LayerMask searchLayer;

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform ownerGO;
    private Transform playerGO;
    private Vector3 startPos;
    private Vector3 direction;
    private float attackRangeMax;

    private bool retreating;
    private bool firstmove;

    public void Enter()
    {
        direction = (this.playerGO.position - this.ownerGO.position).normalized;
        Physics.OverlapSphere(ownerGO.transform.position - (direction * 4f), 1f, this.searchLayer);
    }

    public void Execute()
    {

        if (!retreating)
        {


        }

    }

    public void Exit()
    {
        
    }
}
