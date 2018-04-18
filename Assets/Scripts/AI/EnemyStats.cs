using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    PlayerManager playermanager;

    Animator anim;

    private float health;

    private float damage;

    private bool dead;
    private bool dying;

    public bool Dead
    {
        get { return dead; }
    }

    public float Health
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

        get
        {
            return this.damage;
        }
    }

    private void Start()
    {
        dead = false;

        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamange(5);
        }
    }

    public void TakeDamange(int damageDealt)
    {
        health -= damageDealt;

        if (health <= 0 && !dead)
        {
            dead = true;
            anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("isDead");
            Destroy(this.gameObject, 3f);
        }

        health -= damageDealt;
    }

    public void DealDamage()
    {
        playermanager.TakeDamage(damage);
    }
}//Stina Hedman

