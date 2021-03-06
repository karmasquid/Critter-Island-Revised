﻿using System.Collections;
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


        DontDestroyOnLoad(gameObject);

        health.Bar = GameObject.Find("HealthBar").GetComponent<BarScript>();
        stamina.Bar = GameObject.Find("StaminaBar").GetComponent<BarScript>();
        player = GameObject.Find("Player");

        health.Initialize();
        stamina.Initialize();
    }
    #endregion

    //updates the current value of the stamina.
    private void Update()
    { 
        if (stamina.CurrentValue < stamina.MaxValue)
        {
            timeCheck += Time.deltaTime;
        }
        
        if (timeCheck > waitTime)
        {
            timeCheck = 0;
            stamina.CurrentValue += staminaRecharge;
        }
    }

    //used to change stats on the player when an item is equipped.
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

        GameObject.Find("AmmoCounter").GetComponent<AmmoCounterHUD>().UpdateAmmoCounter(ammoCount);

        //characterScript.SpeedMultiplier += itemAdd.MovementDiff;

        //characterattackthingystuffscript.attackrate += itemAdd.AttackSpeed;
    }

    //used to remove stats when an item is unequipped
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

        GameObject.Find("AmmoCounter").GetComponent<AmmoCounterHUD>().UpdateAmmoCounter(ammoCount);
    }

    //used to set ammo on the player.
    public void SetAmmo(int ammo)
    {
        ammoCount = ammo;
        AmmoCounter(0);
    }

    //used to drain stamina on rangeattack.
    public void RangeAttack(GameObject enemy)
    {
        stamina.CurrentValue -= Inventory.instance.equippedItems[1].StaminaCost;
    }

    //used when the player uses a consumable item from its backpack.
    public void UseConsumable(Item consumable)
    {
        health.CurrentValue += consumable.Health;

        stamina.CurrentValue += consumable.StaminaRecovery;
    }

    //used to set the stamina after a special attack.
    public void SpecAttack(GameObject[] enemys)
    {
        stamina.CurrentValue -= Inventory.instance.equippedItems[0].StaminaCostSpec;
    }


    //used to take damage from enemies.
    public void TakeDamage(float Damage)
    {
        health.CurrentValue -= Damage;

        if (health.CurrentValue <= 0)
        {
            GameObject.Find("Player").GetComponent<Attacker>().Dead = true;
            Character.instance.IsDead = true;
        }
    }

    //Updates the ammocount and the hud ammocountdisplay.
    public void AmmoCounter(int AmmoDrain)
    {
        ammoCount -= AmmoDrain;

        // Uppdatera ammocounter i hud
        Debug.Log("Current ammo count: " + ammoCount);

        GameObject.Find("AmmoCounter").GetComponent<AmmoCounterHUD>().UpdateAmmoCounter(ammoCount);
    }

    //used to loose stamina.
    public void LooseStamina(float sta)
    {

        if (stamina.CurrentValue >= sta)
        {
            stamina.CurrentValue -= sta;
        }

        else
            stamina.CurrentValue = 0;       
    }

} // Stina Hedman & Mattias Eriksson & Daniel Laggar.
