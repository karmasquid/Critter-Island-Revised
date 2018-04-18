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

    //Variabel för timer.
    private float timeCheck = 0;

    //waitTime variabel = nuvarande staminaRecharge
    private float waitTime = 0.1f;

    //Daniel Laggar del
    private float knockBackForce = 2f;

    //relevanta script
    private Inventory inventoryScript;
    private AmmoCounterHUD ammoCounterScript;
    private Character characterScript;


    private float armor;
    private float drainingStamina;

    private int ammoCount = 0;
    public int AmmoCount { get { return this.ammoCount; } }

    private int rangeDamage;
    public int RangeDamage { get { return this.rangeDamage; } }

    private int meleeDamage;
    public int MeleeDamage { get { return this.meleeDamage; } }

    private int meleeSpecDamage;
    public int MeleeSpecDamage { get { return this.meleeSpecDamage; } }

    private float staminaRecharge;
    public float StaminaRecharge { get { return this.staminaRecharge; } set { this.staminaRecharge = value; } }

    private float meleeStaminaCost;
    public float MeleeStaminaCost { get { return this.meleeStaminaCost; } }

    private float meleeSpecStaminaCost;
    public float MeleeSpecStaminaCost { get { return this.meleeSpecStaminaCost; } }

    private float rangeStaminaCost;
    public float RangeStaminaCost { get { return this.rangeStaminaCost; } }

    public Stats Stamina
    {
        get
        {
            return stamina;
        }
    }

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

        inventoryScript = GameObject.Find("Inventory").GetComponent<Inventory>();
        ammoCounterScript = GameObject.Find("AmmoCounter").GetComponent<AmmoCounterHUD>();
    }
    #endregion

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

    public void AddPlayerstats(Item itemAdd)
    {
        health.MaxValue += itemAdd.Health;
        armor += itemAdd.Armor;

        staminaRecharge += itemAdd.StaminaRecovery;

        ammoCount += itemAdd.AmmoCount;

        meleeDamage = itemAdd.DamageMelee;

        meleeSpecDamage = itemAdd.DamageSpec;

        rangeDamage = itemAdd.DamageRange;

        if (itemAdd.ItemType == Item.TypeOfItem.Weapon)
        {
            meleeStaminaCost += itemAdd.StaminaCost;
            meleeSpecStaminaCost += itemAdd.StaminaCostSpec;
        }

        if (itemAdd.ItemType == Item.TypeOfItem.Ranged)
        {
            rangeStaminaCost += itemAdd.StaminaCost;
        }

        ammoCounterScript.UpdateAmmoCounter(ammoCount);

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

        ammoCounterScript.UpdateAmmoCounter(ammoCount);
    }
    public void RangeAttack(GameObject enemy)
    {
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

    public void TakeDamage(float Damage)
    {
        health.CurrentValue -= Damage;
    }
    public void AmmoCounter(int AmmoDrain)
    {
        ammoCount -= AmmoDrain;

        // Uppdatera ammocounter i hud
        Debug.Log("Current ammo count: " + ammoCount);

        ammoCounterScript.UpdateAmmoCounter(ammoCount);
    }

    public void LooseStamina(float sta)
    {

        if (stamina.CurrentValue >= sta)
        {
            stamina.CurrentValue -= sta;
        }

        else
            stamina.CurrentValue = 0;
            
    }


} // Stina Hedman
