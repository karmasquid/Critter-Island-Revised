using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public bool outOfstamina;
    public bool hasAmmo = true;

    //Skapar stats från stats-scriptet.
    [SerializeField]
    private Stats health;

    [SerializeField]
    private Stats stamina;

    //Desto lägre staminaRecharge desto snabbare laddas stamina.
    [SerializeField]
    private float staminaRecharge;

    //Variabel för timer.
    private float timeCheck = 0;

    //waitTime variabel = nuvarande staminaRecharge
    private float waitTime = 0.1f;

    //Daniel Laggar del
    private float knockBackForce = 2f;

    //relevanta script
    private Inventory inventoryScript;
    private Character characterScript;
    private BasicAI basicAI;

    private float armor;
    private float drainingStamina;

    private int ammoCount = 2;
    //--------------------------------------------------------------------------------------------FIXIT---------------------------------------------------------------------------------------
    private int rangeDamage;
    public int RangeDamage { get { return this.rangeDamage; } }

    private int meleeDamage;
    public int MeleeDamage { get { return this.meleeDamage; } }

    private int meleeSpecDamage;
    public int MeleeSpecDamage { get { return this.meleeSpecDamage; } }
    //-----------------------------------------------------------------------------------------ENDFIXIT---------------------------------------------------------------------------------------
    public GameObject player;

    #region Singleton

    public static PlayerManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        health.Initialize();
        stamina.Initialize();

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        inventoryScript = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void Update()
    { 
        //SKAPA CORUTINER av detta ----------------------------------------------------------------------------
        if (stamina.CurrentValue < stamina.MaxValue)
        {
            timeCheck += Time.deltaTime;
        }

        //timeCheck vs wait time
        if (timeCheck > waitTime)
        {
            timeCheck = 0;
            //+= staminaRecharge
            stamina.CurrentValue += staminaRecharge;
        }

        if (ammoCount > 0)
        {
            hasAmmo = true;
        }
        //SKAPA CORUTINER av detta ----------------------------------------------------------------------------
    }

    public void RechargeStamina(float recharge)
    {

    }

    public void AddPlayerstats(Item itemAdd)
    {
        health.MaxValue += itemAdd.Health;
        armor += itemAdd.Armor;

        staminaRecharge += itemAdd.StaminaRecovery;
        ammoCount += itemAdd.AmmoCount;

        meleeDamage = itemAdd.DamageMelee;

        meleeSpecDamage = itemAdd.DamageSpec;

        rangeDamage = itemAdd.DamageRange;

        //knockBackForce += itemAdd.knockBackForce;

        //characterScript.SpeedMultiplier += itemAdd.MovementDiff;

        //characterattackthingystuffscript.attackrate += itemAdd.AttackSpeed;
    }

    public void Removestats(Item itemrem)
    {
        health.MaxValue -= itemrem.Health;
        armor -= itemrem.Armor;
        staminaRecharge -= itemrem.StaminaRecovery;
        ammoCount -= itemrem.AmmoCount;
        meleeDamage -= itemrem.DamageMelee;
        meleeSpecDamage -= itemrem.DamageSpec;
        rangeDamage -= itemrem.DamageRange;


        //knockBackForce -= itemAdd.knockBackForce;

        //characterScript.SpeedMultiplier -= itemrem.MovementDiff;

        //movementscriptstuff += itemrem.AttackSpeed;
    }

    public void MeleeAttack(GameObject[] enemies)
    {
        //stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCost;

        //if (staminaRecharging == true)
        //{
        //    //kör corutine!
        //}
        foreach (GameObject enemy in enemies)
        {
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

            enemyRB.AddForce(-enemy.transform.forward * knockBackForce * 4f, ForceMode.Impulse);

            enemy.GetComponent<BasicAI>().TakeDMG(meleeDamage); //Takes basic melee damage + potetial special attack damage.

            //enemy.GetComponent<ElderBrute>().TakeDMG(meleeDamage);
        }
        //deal damage to enemy.
    }

    public void RangeAttack(GameObject enemy)
    {
        enemy.GetComponent<BasicAI>().TakeDMG(rangeDamage); //Tills ammo fungerar...

        stamina.CurrentValue -= inventoryScript.equippedItems[1].StaminaCost;

        /*
        if (ammoCount > 0)
        {

            ammoCount -= 1;
            stamina.CurrentValue -= inventoryScript.equippedItems[1].StaminaCost;

            enemy.GetComponent<BasicAI>().TakeDMG(throwDMG);

            //if (staminaRecharging == true)
            //{
            //    //kör corutine!
            //}

            //basicAI = enemy.GetComponent<BasicAI>();
            //aiScript.currentHP  <----------fixa en set.
        }
        */
    }

    public void SpecAttack(GameObject[] enemys)
    {
        stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCostSpec;

        foreach (GameObject enemy in enemys)
        {
            // ---------------------------------------------------------------------------------------- Make enemy move away from player instead of player forward.------------------------------------------------------------------------
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
            enemyRB.AddForce(-enemy.transform.forward * knockBackForce * 10f, ForceMode.Impulse);
            // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }
        //if (staminaRecharging == true)
        //{
        //    //kör corutine!
        //}

        //Deal damage.
    }

    public void TakeDamage(int Damage)
    {
        health.CurrentValue -= Damage;
    }
    public void AmmoCounter(int AmmoDrain)
    {
        ammoCount -= AmmoDrain;

        if (ammoCount <= 0)
        {
            hasAmmo = false;
        }
        else
        {
            hasAmmo = true;
        }

        Debug.Log("Current ammo count: " + ammoCount);
    }

    public void LooseStamina(float sta)
    {
        //____________________NEW CODE________________
        drainingStamina = sta;

        if (stamina.CurrentValue < drainingStamina)
        {
            outOfstamina = true; 
        }
        else
        {
            stamina.CurrentValue -= drainingStamina;
            outOfstamina = false;
        }


        //______________________OLD CODE______________

        /*
        if (stamina.CurrentValue >= sta)
        {
            stamina.CurrentValue -= sta;
        }
        else
            stamina.CurrentValue = 0;
            */
    }


} // Stina Hedman
