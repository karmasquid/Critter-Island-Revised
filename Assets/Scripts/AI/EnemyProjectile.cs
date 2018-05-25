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
            DamagePlayer();
            StartCoroutine(WaitUntillDisable());
        }

        //if (collision.transform.name == "HurtBox")
        //{

        //}

        else
        {
            StartCoroutine(WaitUntillDisable());
        }
    }

    private IEnumerator WaitUntillDisable()
    {
        damage = 0;
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        yield break;
    }

    private void DamagePlayer()
    {
        enemyStats.PlayerInRange = true;
        enemyStats.DealDamage();
        enemyStats.PlayerInRange = false;
        //gameObject.SetActive(false);

    }
}
