using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    public static Attacker instance;

    [SerializeField]
    float atkSpeed;
    [SerializeField]
    float reloadSpeed;
    [SerializeField]
    Collider[] attackHitBoxes;

    //Current Equipped item.
    public Item currentRange;
    public Item currentMelee;

    //Bools used to check if player is dead.
    bool dead;
    bool dying;
    
    //Time to wait until restart.
    float waitUntillRestart;

    //Attack variables
    bool chargingAttack;
    bool throwing = false;
    float chargeTimer;
    bool canAtk = true;

    //Gets the scripts, animator and rigidbody.
    Throwable throwableScript;
    Inventory inventory;
    Character characterScript;
    Animator anim;
    Rigidbody rb;

    IEnumerator Reload;

    //Array/lists of enemy gameobjects.
    GameObject[] enemies;
    List<GameObject> listOfEffect = new List<GameObject>();

    public bool Dead //Dead bool, used to get/set the player to a dead state.
    {
        get
        {
            return this.dead;
        }
        set
        {
            dead = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //Sets the animator, rigidbody and the character.
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        characterScript = GetComponent<Character>();
    }
    void AtkControl()
    {
            if (canAtk) //Checking if the player can attack.
            {
                if (InputManager.Attack())
                {
                    chargeTimer += Time.deltaTime;
                    if (chargeTimer > 1.0f) //Promps the charge attack.
                    {
                        chargingAttack = true;
                        anim.SetBool("isCharging", true);
                    }
                }
                if (InputManager.AttackUp())
                {
                    Attacking();
                    canAtk = false;
                }
            }
    }
    void Update()
    {
        if (!dead) //Checking if the player is not dead.
        { 
            //-------------------------------------Main Attacks & Controls------------------------------------------------
            AtkControl();

            if (PlayerManager.instance.AmmoCount > 0 && InputManager.ThrowAttack()) //Check if player can throw.
            {
                if (!throwing)
                {
                    throwing = true;
                    StartCoroutine(ThrowCD());

                }
            }
            if (InputManager.Dodge() && chargingAttack == true) //Checking if dodging and charging an attack.
            {
                chargingAttack = false;
                chargeTimer = 0; //Resets the charge attack.
                anim.SetBool("isCharging", false);
            }

        }
        else //If the player is dead.
        {
            if (!dying) //If player is dying.
            {
                anim.SetTrigger("isDead");
                waitUntillRestart = Time.time + 5f; //Waiting 5 seconds from the moment the players health runs out to the moment the next scene is loaded.
                this.gameObject.layer = 0;
                dying = true;
            }
            else if (Time.time >= waitUntillRestart && dead) //Loads the village scene, returns the player to the surface...
            {
                GameObject.Find("LoadingScreen").GetComponent<LevelLoader>().Loadlevel(2);
                //Spawn at correct location...
                
                Destroy(GameObject.Find("ItemDatabase"));
                Destroy(GameObject.Find("PlayerManager"));
                Destroy(GameObject.Find("Player"));
                
            }
        }
    }

    void LaunchAttack(Collider col) //Launching an attack, gets the colliders inside the player's attack collider.
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach (Collider c in cols)
        {
            if (c.transform.root == gameObject.transform) // Don't hit yourself...
            {
                continue; //Skip your own collider.
            }
            listOfEffect.Add(c.gameObject); //Adds the enemy colliders into a list.
        }
    }
    void Attacking() //Checking Basic or special attack
    {

            //Check if charging up an attack or not and that there is stamina.
            if (chargingAttack && PlayerManager.instance.Stamina.CurrentValue >= PlayerManager.instance.MeleeSpecStaminaCost)
            {
                StartCoroutine(SpecAttackCD()); //Starts the cooldown timer.
            }
            else
            {
                StartCoroutine(AttackCD());//Starts the cooldown timer.
            }
               
            listOfEffect.Clear(); //Clears the list of enemies.

            if (listOfEffect.Count.Equals(0)) //Checking if the list is empty.
            {
                enemies = null; //Then array is empty.
            }

            chargeTimer = 0; //Reset the charge up timer.
            
            //Promps the cooldown.
            Reload = AttackCD();
            StartCoroutine(Reload);
        
    }

    IEnumerator ThrowCD () //Coroutine for throw attacks.
    {
        rb.velocity = Vector3.zero;

        characterScript.Attacking = true;
        throwing = true;
        anim.SetTrigger("throw");
        
        yield return new WaitForSeconds(0.25f);

        GameObject preo = Instantiate(currentRange.Go,attackHitBoxes[2].transform.position,attackHitBoxes[2].transform.rotation);

        preo.GetComponent<Throwable>().Damage = currentRange.DamageRange;

        PlayerManager.instance.AmmoCounter(1);

        Debug.Log("pewpew");
        yield return new WaitForSeconds(0.75f);
        throwing = false;
        characterScript.Attacking = false;
    }
    IEnumerator AttackCD() //Coroutine for attack cooldown.
    {
        characterScript.Attacking = true;

        rb.velocity = Vector3.zero; //Stopping the player when attacking normally.

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.2f);

        LaunchAttack(attackHitBoxes[0]);
        PlayerManager.instance.LooseStamina(currentMelee.StaminaCost); //Stamina drain 

        if (!listOfEffect.Count.Equals(0)) //If list isn't empty...
        {
            enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
        }
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies) //For every enemy, take their rigidbody and apply a force to them on successful hit.
            {
                Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                enemy.GetComponent<EnemyStats>().TakeDamange(currentMelee.DamageMelee); //Takes basic melee damage
            }
        }

        yield return new WaitForSeconds(0.8f);

        chargingAttack = false;
        anim.SetBool("isCharging", false);

        canAtk = true;
        characterScript.Attacking = false;
    }

    IEnumerator SpecAttackCD()
    {
       

        characterScript.Attacking = true;
        anim.SetTrigger("chargeAttack");
        yield return new WaitForSeconds(0.1f);

        LaunchAttack(attackHitBoxes[1]);
        chargingAttack = false;
        anim.SetBool("isCharging", false);
        PlayerManager.instance.LooseStamina(currentMelee.StaminaCostSpec); //Stamina drain

        if (!listOfEffect.Count.Equals(0)) //If list isn't empty...
        {
            enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
        }
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies) //For every enemy, take their rigidbody and apply a force to them on successful hit.
            {
                Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                enemy.GetComponent<EnemyStats>().TakeDamange(currentMelee.DamageSpec); //Takes special melee damage
            }

        }

        characterScript.Attacking = false;
    }
} //Mattias Eriksson
