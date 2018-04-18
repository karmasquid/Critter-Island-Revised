using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    [SerializeField]
    float atkSpeed;
    [SerializeField]
    float reloadSpeed;
    [SerializeField]
    Collider[] attackHitBoxes;

    public GameObject currentEquipped;

    bool dead;
    bool dying;

    float waitUntillRestart;

    bool chargingAttack;
    bool throwing = false;
    
    float chargeTimer;
    bool canAtk = true;

    PlayerManager playermanager;
    Throwable throwableScript;
    Inventory inventory;
    IEnumerator Reload;

    Animator anim;

    GameObject[] enemies;
    List<GameObject> listOfEffect = new List<GameObject>();

    public static Attacker instance;

    public bool Dead
    {
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

        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        anim = GetComponent<Animator>();
    }

    void Start ()
    {
        
    }
    void AtkControl()
    {
        if (canAtk)
        {
            if (InputManager.Attack())
            {

                chargeTimer += Time.deltaTime;
                if (chargeTimer > 1.0f)
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
        if (!dead)
        { 
            //-------------------------------------Main Attacks & Controls------------------------------------------------
            AtkControl();

            if (playermanager.AmmoCount > 0 && InputManager.ThrowAttack())
            {
                if (!throwing)
                {
                    anim.SetTrigger("throw");
                    GameObject preo = Instantiate(currentEquipped, attackHitBoxes[2].transform, false) as GameObject;
                    preo.GetComponent<Throwable>().Damage = playermanager.RangeDamage;
                    Debug.Log(playermanager.RangeDamage);

                    attackHitBoxes[2].transform.DetachChildren(); //Release the children!
                    throwing = true;

                    playermanager.AmmoCounter(1); //-1 ammo
                    Reload = ThrowCD(reloadSpeed);
                    StartCoroutine(Reload);

                    if (preo != null)
                    {
                        attackHitBoxes[3] = preo.GetComponent<Collider>();

                    }
                    else
                    {
                        attackHitBoxes[3] = null;
                    }


                    Destroy(preo, Time.deltaTime + 2f);
                }
            }
            if (InputManager.Dodge() && chargingAttack == true)
            {
                chargingAttack = false;
                chargeTimer = 0;
                anim.SetBool("isCharging", false);
            }


            //----------------------------------------Throwing invoke------------------------------------------------------
            if (throwing)
            {
                InvokeRepeating("DetectedThrow", 0f, 4f);
            }
        }
        else
        {
            if (!dying)
            {
                anim.SetTrigger("isDead");
                waitUntillRestart = Time.time + 5f;
                this.gameObject.layer = 0;
                dying = true;
            }
            else if (Time.time >= waitUntillRestart)
            {
                
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().Loadlevel(0);

            }
        }
    }

    void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach (Collider c in cols)
        {
            if (c.transform.root == gameObject.transform) // Don't hit yourself...
            {
                continue;
            }
            listOfEffect.Add(c.gameObject);
        }
    }
    void Attacking() //Checking Basic or special attack:
    {

            //---------Check if charging up an attack or not------------
            if (chargingAttack && playermanager.Stamina.CurrentValue >= playermanager.MeleeSpecStaminaCost)
            {
                anim.SetTrigger("chargeAttack");
                LaunchAttack(attackHitBoxes[1]);
                chargingAttack = false;
                anim.SetBool("isCharging", false);
                playermanager.LooseStamina(playermanager.MeleeSpecStaminaCost); //Stamina drain

                if (!listOfEffect.Count.Equals(0)) //If list isn't empty...
            {
                    enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
                }
                if (enemies != null)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                        enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                        enemy.GetComponent<EnemyStats>().TakeDamange(playermanager.MeleeSpecDamage); //Takes special melee damage
                    }

                }
            }
            else
            {
                anim.SetTrigger("attack");
                LaunchAttack(attackHitBoxes[0]);
                playermanager.LooseStamina(playermanager.MeleeStaminaCost); //Stamina drain

                if (!listOfEffect.Count.Equals(0)) //If list isn't empty...
                {
                    enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
                }
                if (enemies != null)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                        enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                        enemy.GetComponent<EnemyStats>().TakeDamange(playermanager.MeleeDamage); //Takes basic melee damage
                    }
                }

                chargingAttack = false;
                anim.SetBool("isCharging", false);
            }
            listOfEffect.Clear();
            if (listOfEffect.Count.Equals(0))
            {
                enemies = null;
            }
            chargeTimer = 0;

            Reload = AttackCD(atkSpeed);
            StartCoroutine(Reload);
        
    }
    void DetectedThrow() //Failsafe throw:
    {
        if (attackHitBoxes[3] != null)
        {
            LaunchAttack(attackHitBoxes[3]);
        }
    }
    IEnumerator ThrowCD (float CDTime) //Coroutine for throw:
    {
        yield return new WaitForSeconds(CDTime);
        throwing = false;
    }
    IEnumerator AttackCD(float AtkCDTime) //Coroutine for attack cooldown.
    {
        yield return new WaitForSeconds(AtkCDTime);
        canAtk = true;
    }
}
