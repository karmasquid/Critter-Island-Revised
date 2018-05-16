using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour {

    PlayerManager playermanager;

    private System.Action<Results> actionCallback;

    [SerializeField]
    private bool willDrop;

    [SerializeField]
    private string itemToDrop;

    Animator anim;

    [SerializeField]
    private float health;
    private float maxHealth;
    private float damage;

    private bool playerInRange;

    private bool dead;

    private Rigidbody rB;
    private NavMeshAgent navMeshAgent;

    private Transform playerPos;

    public bool Dead
    {
        get { return dead; }
    }

    public float Health
    {
        set
        {
            health = value;
            maxHealth = value;
        }

        get { return this.health; }
    }

    public float MaxHealth
    {
        get { return this.maxHealth; }
    }

    public float Damage
    {
        set
        {
            damage = value;
        }

        get { return this.damage; }
    }

    public bool PlayerInRange
    {
        set
        {
            playerInRange = value;
        }

        get { return this.playerInRange; }
    }

    public Action<Results> ActionCallback
    {
        set
        {
            actionCallback = value;
        }
    }

    private void Start()
    {
        dead = false;

        rB = this.GetComponent<Rigidbody>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        playerPos = GameObject.Find("Player").transform;

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
        StartCoroutine(BeingPushed());

        health -= damageDealt;

        if (health <= 0 && !dead)
        {
            dead = true;
            anim = GetComponent<Animator>();
            anim.SetTrigger("isDead");

            if (willDrop)
            {
                GameObject lootBag = Instantiate(Resources.Load("Prefabs/Loot Bag"), new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z), Quaternion.identity) as GameObject;
                lootBag.GetComponent<LootBag>().ItemInBag = itemToDrop;
            }

            rB.velocity = Vector3.zero;
            rB.angularVelocity = Vector3.zero;

            Destroy(this.gameObject, 5f);
        }


        //if (health > (MaxHealth / 10) * 3)
        //{
        //    var enemystatscallback = new Results(4);
        //    actionCallback(enemystatscallback);
        //}

    }
    private IEnumerator BeingPushed()
    {

        if (health > playermanager.MeleeDamage)
        {

            rB.AddForce(playerPos.forward *2f, ForceMode.Impulse);

            yield return new WaitForSeconds(2f);

        }


        yield break;
    }
    public void DealDamage()
    {
        if (playerInRange)
        {
            playermanager.TakeDamage(damage);
        }
    }
}//Stina Hedman

