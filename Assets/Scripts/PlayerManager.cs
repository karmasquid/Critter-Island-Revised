using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static bool outOfstamina; //Fullösning bör inte användas vid optimering.

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

    private int ammoCount;
    //--------------------------------------------------------------------------------------------FIXIT---------------------------------------------------------------------------------------
    private int rangeDamage = 15;
    public int RangeDamage { get { return this.rangeDamage; } }

    private int meleeDamage = 30;
    public int MeleeDamage { get { return this.meleeDamage; } }
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

        //knockBackForce += itemAdd.knockBackForce;

        characterScript.SpeedMultiplier += itemAdd.MovementDiff;

        //characterattackthingystuffscript.attackrate += itemAdd.AttackSpeed;
    }

    public void Removestats(Item itemrem)
    {
        health.MaxValue-= itemrem.Health;
        armor -= itemrem.Armor;

        staminaRecharge -= itemrem.StaminaRecovery;

        //knockBackForce -= itemAdd.knockBackForce;

        characterScript.SpeedMultiplier -= itemrem.MovementDiff;

        //movementscriptstuff += itemrem.AttackSpeed;
    }

    public void MeleeAttack(GameObject[] enemy, int extraDMG)
    {
        //stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCost;

        //if (staminaRecharging == true)
        //{
        //    //kör corutine!
        //}
        foreach (GameObject NME in enemy)
        {
            Rigidbody enemyRB = NME.GetComponent<Rigidbody>();
            enemyRB.AddForce(-NME.transform.forward * knockBackForce * 4f, ForceMode.Impulse);
            NME.GetComponent<BasicAI>().TakeDMG(meleeDamage + extraDMG); //Takes basic melee damage + potetial special attack damage.
        }
        //deal damage to enemy.
    }

    public void RangeAttack(GameObject enemy)
    {
        if (ammoCount > 0)
      {
            ammoCount -= 1;
            stamina.CurrentValue -= inventoryScript.equippedItems[1].StaminaCost;

            //if (staminaRecharging == true)
            //{
            //    //kör corutine!
            //}

            //basicAI = enemy.GetComponent<BasicAI>();
            //aiScript.currentHP  <----------fixa en set.
        }
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

    public void LooseStamina(float sta)
    {
        //____________________NEW CODE________________
        drainingStamina = sta;

        if (stamina.CurrentValue < drainingStamina)
        {
            outOfstamina = true; //Fullösning med public static bool...
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
