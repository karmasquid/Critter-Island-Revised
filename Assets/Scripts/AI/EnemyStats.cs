using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    PlayerManager playermanager;

    [SerializeField]
    private bool willDrop;

    [SerializeField]
    private string itemToDrop;

    Animator anim;private float health;
    private float damage;
    private bool dead;

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

            if (willDrop)
            {
                GameObject lootBag = Instantiate(Resources.Load("Prefabs/Loot Bag"), new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z), Quaternion.identity) as GameObject;
                lootBag.GetComponent<LootBag>().ItemInBag = itemToDrop;
            }

            Destroy(this.gameObject, 5f);
        }

        health -= damageDealt;
    }

    public void DealDamage()
    {
        playermanager.TakeDamage(damage);
    }
}//Stina Hedman

