using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour {

    private EnemyStats enemyStats;
    private float damage;
    private PlayerManager playerManager;

    public float Damage
    {
        set
        {
            damage = value;
        }
    }

    public EnemyStats EnemyStats
    {
        set
        {
            enemyStats = value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            enemyStats.DealDamage();
            gameObject.SetActive(false);
        }

        else
        {
            WaitUntillDisable();
        }
    }

    private IEnumerator WaitUntillDisable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
