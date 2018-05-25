using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour {

    EnemyStats enemyStats;

    // Use this for initialization
    void Awake () {
        enemyStats = transform.parent.GetComponent<EnemyStats>();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyStats.PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyStats.PlayerInRange = false;
        }
    }


}
