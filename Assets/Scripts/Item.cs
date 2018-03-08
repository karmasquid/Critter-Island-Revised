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

    public enum TypeOfItem { Helmet, Armor, Boots, Weapon, Equippable, Miscellaneous }
    private TypeOfItem itemType;

    //Wearables related info
    protected int damageBuff;
    protected int healthBuff;
    protected float staminaRechargeBuff;

    protected int damage;
    protected bool infiniteAmmo;
    protected int ammo;
    protected float fireRate;

    protected float meleeRange;
    protected float throwRange;
    protected int staminaLoss;


    public Item(int Id, string Name, string Desc, TypeOfItem KindOfGear, int DamageBuff, int HealthBuff, float StaminaRechargeBuff, int Damage, bool InfiniteAmmo, int AmmoCount, float FireRate, float MeleeRange, float ThrowRange, int StaminaLoss)
    {
        id = Id;

        //Description
        name = Name;
        desc = Desc;
        itemType = KindOfGear;

        //Stats
        damageBuff = DamageBuff;
        healthBuff = HealthBuff;
        staminaRechargeBuff = StaminaRechargeBuff;

        damage = Damage;
        infiniteAmmo = InfiniteAmmo;
        ammo = AmmoCount;

        fireRate = FireRate;
        meleeRange = MeleeRange;
        throwRange = ThrowRange;

        staminaLoss = StaminaLoss;

        //Sprite and Prefab for item;
        icon = Resources.Load<Sprite>("Sprites/UI/Item Icons/" + name);
        go = Resources.Load<GameObject>("Sprites/UI/Item Prefabs/" + name);
    }

    public Item() //Gör ett tomt item
    {

    }

    public int ID { get { return this.id; } }
    public string Name { get { return this.name; } }
    public string Desc { get { return this.desc; } }
    public TypeOfItem ItemType { get { return this.itemType; } }

    public int DamageBuff { get { return this.damageBuff; } }
    public int HealthBuff { get { return this.healthBuff; } }
    public float StaminaRechargeBuff { get { return this.staminaRechargeBuff; } }

    public int Damage { get { return this.damage; } }
    public bool Infiniteammo { get { return this.infiniteAmmo; } }
    public int Ammo { get { return ammo; } }

    public float FireRate { get { return this.fireRate; } }
    public float MeleeRange { get { return this.meleeRange; } }
    public float ThrowRange { get { return this.throwRange; } }

    public int StaminaLoss { get { return this.staminaLoss; } }

    public Sprite Icon { get { return this.icon; } }
    public GameObject Go { get { return this.go; } }





}
//Stina Hedman


