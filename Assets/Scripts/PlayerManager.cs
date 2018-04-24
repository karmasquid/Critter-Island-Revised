using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public bool dead;

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
    public int AmmoCount { get { return this.ammoCount; } set { this.ammoCount = value; } }

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
        //SKAPA CORUTINER av detta ----------------------------------------------------------------------------
    }

    public void AddPlayerstats(Item itemAdd)
    {
        armor += itemAdd.Armor;

        staminaRecharge += itemAdd.StaminaRecovery;

        ammoCount += itemAdd.AmmoCount;

        meleeDamage += itemAdd.DamageMelee;

        meleeSpecDamage += itemAdd.DamageSpec;

        rangeDamage += itemAdd.DamageRange;

        meleeStaminaCost += itemAdd.StaminaCost;

        meleeSpecStaminaCost += itemAdd.StaminaCostSpec;

        rangeStaminaCost += itemAdd.StaminaCost;

        ammoCounterScript.UpdateAmmoCounter(ammoCount);

        //knockBackForce += itemAdd.knockBackForce;

        //characterScript.SpeedMultiplier += itemAdd.MovementDiff;

        //characterattackthingystuffscript.attackrate += itemAdd.AttackSpeed;
    }

    public void Removestats(Item itemrem)
    {

        armor -= itemrem.Armor;

        staminaRecharge -= itemrem.StaminaRecovery;

        ammoCount -= itemrem.AmmoCount;

        meleeDamage -= itemrem.DamageMelee;

        meleeSpecDamage -= itemrem.DamageSpec;

        rangeDamage -= itemrem.DamageRange;

        meleeStaminaCost -= itemrem.StaminaCostSpec;

        meleeSpecStaminaCost -= itemrem.StaminaCostSpec;

        rangeStaminaCost -= itemrem.StaminaCost;

        //knockBackForce -= itemAdd.knockBackForce;

        //characterScript.SpeedMultiplier -= itemrem.MovementDiff;

        //movementscriptstuff += itemrem.AttackSpeed;

        ammoCounterScript.UpdateAmmoCounter(ammoCount);
    }
    public void RangeAttack(GameObject enemy)
    {
        stamina.CurrentValue -= inventoryScript.equippedItems[1].StaminaCost;
    }

    public void UseConsumable(Item consumable)
    {
        health.CurrentValue += consumable.Health;

        stamina.CurrentValue += consumable.StaminaRecovery;
    }

    public void SpecAttack(GameObject[] enemys)
    {
        stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCostSpec;
    }

    public void TakeDamage(float Damage)
    {
        health.CurrentValue -= Damage;

        if (health.CurrentValue <= 0)
        {
            GameObject.Find("Player").GetComponent<Attacker>().Dead = true;
            dead = true;
        }
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

    public void ConnectToBarsInHUD()
    {
        health.Bar = GameObject.Find("HealthBar").GetComponent<BarScript>();
        stamina.Bar = GameObject.Find("StaminaBar").GetComponent<BarScript>();
        player = GameObject.Find("Player");
    }
} // Stina Hedman
