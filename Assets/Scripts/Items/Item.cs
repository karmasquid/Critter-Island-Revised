using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    //info all item contains.
    protected int id;
    protected string name;
    protected string desc;
    protected Sprite icon;
    protected GameObject go;

    public enum TypeOfItem { Headgear, Armor, Boots, Weapon, Ranged, Consumables, Miscellaneous }
    private TypeOfItem itemType;

    //Wearables related info
    protected int damageMelee;
    protected int damageSpec;
    protected int damageRange;
    protected float health;
    protected float armor;

    protected float staminaCost;
    protected float staminaCostSpec;
    protected float staminaRecovery;

    protected float range;
    //protected float stackLimit;
    protected int ammoCount;
    protected float movementDiff;
    protected float attackSpeed;

    protected int ability;
    protected int cost;


    public Item(int Id, string Name, string Desc, TypeOfItem KindOfGear, int MeleeDamage, int RangeDamage, int SpecDamage, float Health, float Armor, float StaminaCost, float StaminaCostSpec, float StaminaRecovery, float Range, int AmmoCount, float MovementDiff, float Attackspeed, int Ability, int Cost)
    {
        id = Id;
        name = Name;
        desc = Desc;
        itemType = KindOfGear;

        damageMelee = MeleeDamage;
        damageRange = RangeDamage;
        damageSpec = SpecDamage;
        health = Health;
        armor = Armor;

        staminaCost = StaminaCost;
        staminaCostSpec = StaminaCostSpec;
        staminaRecovery = StaminaRecovery;

        range = Range;
        ammoCount = AmmoCount;
        movementDiff = MovementDiff;
        attackSpeed = Attackspeed;

        ability = Ability;
        cost = Cost;
        //Sprite and Prefab for item;
        icon = Resources.Load<Sprite>("Sprites/UI/Item Icons/" + name);
        go = Resources.Load<GameObject>("Prefabs/Items/" + name);
    }

    public Item() //Gör ett tomt item
    {

    }

    public int ID { get { return this.id; } }
    public string Name { get { return this.name; } }
    public string Desc { get { return this.desc; } }
    public TypeOfItem ItemType { get { return this.itemType; } }

    public int DamageMelee { get { return this.damageMelee; } }
    public int DamageRange { get { return this.damageRange; } }
    public int DamageSpec { get { return this.damageSpec; } }
    public float Health { get { return this.health; } }
    public float Armor { get { return this.armor; } }

    public float StaminaCost { get { return this.staminaCost; } }
    public float StaminaCostSpec { get { return this.staminaCostSpec; } }
    public float StaminaRecovery { get { return this.staminaRecovery; } }

    public float Range { get { return this.range; } }
    public int AmmoCount { get { return this.ammoCount; } }
    public float MovementDiff { get { return movementDiff; } }

    public float AttackSpeed { get { return this.attackSpeed; } }
    public int Ability { get { return this.ability; } }
    public int Cost { get { return this.cost; } }

    public Sprite Icon { get { return this.icon; } }
    public GameObject Go { get { return this.go; } }
}
//Stina Hedman


