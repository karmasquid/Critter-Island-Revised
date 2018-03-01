using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {
    /* AI states
     * 
     * IDLE
     * PATROLING
     * TARGETING
     * APROACH (kan vara snabbare hastighet än patrol)
     * ATTACK
     * LOSE TARGET > IDLE > PATROLING
     * DEAD  */
    bool playerInSight;
    int currentHP = 100;
    int maxHP = 100;
    public int attackDMG = 0;
    [SerializeField]
    float patrolSpeed = 0;
    [SerializeField]
    float targetingRange = 0;
    [SerializeField]
    float approachSpeed = 0;
    float acceleration = 0.1f;
    [SerializeField]
    float attackRange = 0;
    
	void Start () {

        playerInSight = false;

	}

	void Update () {
		
	}

    void playerTargeted() {

    }

}
