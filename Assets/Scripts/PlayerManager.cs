using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

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

    //relevanta script
    private Inventory inventoryScript;
    private Character characterScript;
    private BasicAI basicAI;

    private float armor;

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

    void RechargeStamina()
    {

    }

    public void AddPlayerstats(Item itemAdd)
    {
        health.MaxValue += itemAdd.Health;
        armor += itemAdd.Armor;

        staminaRecharge += itemAdd.StaminaRecovery;
        ammoCount += itemAdd.AmmoCount;

        characterScript.SpeedMultiplier += itemAdd.MovementDiff;

        //characterattackthingystuffscript.attackrate += itemAdd.AttackSpeed;
    }

    public void Removestats(Item itemrem)
    {
        health.MaxValue-= itemrem.Health;
        armor -= itemrem.Armor;

        staminaRecharge -= itemrem.StaminaRecovery;
        
        characterScript.SpeedMultiplier -= itemrem.MovementDiff;

        //movementscriptstuff += itemrem.AttackSpeed;
    }

    public void MeleeAttack(GameObject enemy)
    {
        stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCost;
        
        //if (staminaRecharging == true)
        //{
        //    //kör corutine!
        //}

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

    public void SpecAttack(GameObject enemy)
    {
        stamina.CurrentValue -= inventoryScript.equippedItems[0].StaminaCostSpec;

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

    private void LooseStamina(int sta)
    {
        if (stamina.CurrentValue >= sta)
        {
            stamina.CurrentValue -= sta;
        }
        else
            stamina.CurrentValue = 0;
    }


}
