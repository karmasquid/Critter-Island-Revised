using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    PlayerManager playermanager;

    private Stats health;

    private float damage;

    private bool dead;

    public bool Dead
    {
        get { return dead; }
    }

    public Stats Health
    {
        set
        {
            health = value;
        }
    }

    public float Damage
    {
        set
        {
            damage = value;
        }
    }

    private void Start()
    {



        dead = false;

        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public void TakeDamange(int damageDealt)
    {
        //health.CurrentValue -= damageDealt;

        //if (health.CurrentValue <= 0)
        //{
        //    dead = true;

        //    //this.gameObject.transform.rotate(new Vector3())
        //}

        health.CurrentValue -= damageDealt;
    }

    public void DealDamage()
    {
        playermanager.TakeDamage(damage);
    }
}
