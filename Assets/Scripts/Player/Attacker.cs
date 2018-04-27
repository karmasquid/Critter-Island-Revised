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

    public Item currentRange;
    public Item currentMelee;

    bool dead;
    bool dying;

    float waitUntillRestart;

    bool chargingAttack;
    bool throwing = false;
    
    float chargeTimer;
    bool canAtk = true;

    Throwable throwableScript;
    Inventory inventory;
    Character characterScript;
    IEnumerator Reload;

    Animator anim;

    Rigidbody rb;

    GameObject[] enemies;
    List<GameObject> listOfEffect = new List<GameObject>();


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

        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        characterScript = GetComponent<Character>();
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

            if (PlayerManager.instance.AmmoCount > 0 && InputManager.ThrowAttack())
            {
                if (!throwing)
                {
                    throwing = true;

                    Debug.Log(PlayerManager.instance.RangeDamage);
                    StartCoroutine(ThrowCD());

                }
            }
            if (InputManager.Dodge() && chargingAttack == true)
            {
                chargingAttack = false;
                chargeTimer = 0;
                anim.SetBool("isCharging", false);
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
            else if (Time.time >= waitUntillRestart && dead)
            {

                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().Loadlevel(0);
                dead = false;
                Destroy(GameObject.Find("Inventory"));
                Destroy(GameObject.Find("ItemDatabase"));
                Destroy(GameObject.Find("Inventory"));
                Destroy(GameObject.Find("PlayerManager"));
                Destroy(GameObject.Find("Player"));
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
            if (chargingAttack && PlayerManager.instance.Stamina.CurrentValue >= PlayerManager.instance.MeleeSpecStaminaCost)
            {
                StartCoroutine(SpecAttackCD());
            }
            else
            {
                StartCoroutine(AttackCD());
            }
            listOfEffect.Clear();
            if (listOfEffect.Count.Equals(0))
            {
                enemies = null;
            }
            chargeTimer = 0;

            Reload = AttackCD();
            StartCoroutine(Reload);
        
    }

    IEnumerator ThrowCD () //Coroutine for throw:
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

        rb.velocity = Vector3.zero;

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.2f);

        LaunchAttack(attackHitBoxes[0]);
        PlayerManager.instance.LooseStamina(currentMelee.StaminaCost); //Stamina drain ---------------------------------------------- FIXA DETTA  ---------------------------------

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
            foreach (GameObject enemy in enemies)
            {
                Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                enemy.GetComponent<EnemyStats>().TakeDamange(currentMelee.DamageSpec); //Takes special melee damage
            }

        }

        characterScript.Attacking = false;
    }
}
