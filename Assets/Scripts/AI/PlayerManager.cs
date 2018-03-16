using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private Inventory inventoryScript;
    private Movement movementScript;
    private BarScript barScript;
    private BasicAI basicAI;

    private float health; // <------------ fråga daniel
    private float armor;

    private int ammoCount;

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
    }
    #endregion

    public void AddPlayerstats(Item itemAdd)
    {
        health += itemAdd.Health;
        armor += itemAdd.Armor;

        //Barscriptvariabel += itemAdd.StaminaRecovery;
        ammoCount += itemAdd.AmmoCount;

        //movmentscriptvariabel += itemAdd.MovementDiff;

        //movementspeedstuff += itemAdd.AttackSpeed;
    }

    public void Removestats(Item itemrem)
    {
        health -= itemrem.Health;
        armor -= itemrem.Armor;

        //Datbarscriptfloat += itemAdd.StaminaRecovery

        ammoCount = itemrem.AmmoCount;
        //movementscript -= itemrem.MovementDiff;
        //movementscriptstuff += itemrem.AttackSpeed;
    }

    public void MeleeAttack()
    {
        //barscript -= stamina för rätt item.

        


    }

    public void RangeAttack(GameObject enemy)
    {
        if (ammoCount > 0)
      {
            ammoCount -= 1;
            //barscript -stamina
            basicAI = enemy.GetComponent<BasicAI>();
            //aiScript.currentHP  <----------fixa en set.
       }
    }

    public void SpecAttack(GameObject enemy)
    {
        //Remove stamina
        //Deal damage.
    }

    public void TakeDamage(int Damage)
    {
        //float damagedealt = Damage
    }




}
