using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    public static ItemDatabase instance;

    //list of all items in game
    public List<Item> allItems = new List<Item>();

    private Inventory inventory;

    private int index;

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
    }

    private void Start()
    {

        //public Item(int Id, string Name, string Desc, TypeOfItem KindOfGear, float MeleeDamage, float RangeDamage, float SpecDamage, float Health, float Armor, float StaminaCost, float StaminaCostSpec, float StaminaRecovery, float Range, int AmmoCount, float MovementDiff, float Attackspeed, int Ability, int Cost)

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        //Weapons
        allItems.Add(new Item(1, "Travellers_Sword", "Traveller's Sword!", Item.TypeOfItem.Weapon, 15f, 0f, 25f, 0f, 0f, 10f,40f,0f,0f,0,0f,1f,0,0));
        allItems.Add(new Item(2, "Judicators_Hammer", "Judicator’s Hammer!", Item.TypeOfItem.Weapon, 20f, 0f, 5f, 25f,0f,15f,50f,0f,0f,0,0,0f,0,0));

        //Ranged
        allItems.Add(new Item(3, "Pebble", "Pebble!", Item.TypeOfItem.Ranged, 0f, 6f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(4, "Knife", "Knife!", Item.TypeOfItem.Ranged, 0f, 8f, 0f, 0f, 0f,0f,0f,0f,1f,10000,0,0,0,0));
        allItems.Add(new Item(4, "Black Powder Bombs", "Black Powder Bombs!", Item.TypeOfItem.Ranged, 0f, 22f, 0f, 30f, 0f,15f,0f,0f,0.75f,5,0,0,0,0));

        //Helmets
        allItems.Add(new Item(5, "Crown_of_the_Foolhardy", "Crown of the Foolhardy!", Item.TypeOfItem.Headgear, 0f, 0f, 0f, 0f, 5f,0,0,0.9f,0,0,0,0,0,0));
        allItems.Add(new Item(6, "Hood_of_Dusk", "Hood of Dusk!", Item.TypeOfItem.Headgear, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0f, 0f, 0, 0));
        allItems.Add(new Item(7, "Mask_of_the_Ancestors", "Mask of the Ancestors!", Item.TypeOfItem.Headgear, 3f, 3f, 0f, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

        //Armors
        allItems.Add(new Item(7, "Vestment_of_the_Knave", "Vestment of the Knave!", Item.TypeOfItem.Armor, 0f, 0f, 0f, 0f, 0f, 5f, 0f, 0f, 0f,0,-2f, 0f, 0, 8));
        allItems.Add(new Item(8, "Cuirass_of_the_Bold", "Cuirass of the Bold!", Item.TypeOfItem.Armor, 3f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0f, 0f, 0, 25));

        //Boots
        allItems.Add(new Item(9, "Steadfast_Greaves", "Steadfast Greaves!", Item.TypeOfItem.Boots, 0f,0f,0f,0f,5f,0,0,0,0,0,2,0,0,8));
        allItems.Add(new Item(10, "Feather_Stride_Boots", "Feather Stride Boots!", Item.TypeOfItem.Boots, 0f, 0f, 0f, 0f, 0f,0f,0f,1.1f, 0f, 0, 0f, 0f, 0, 14));

        //Consumables
        allItems.Add(new Item(1, "Dusk_Leaf", "Dusk Leaf!", Item.TypeOfItem.Consumables, 0f, 0f, 0f, 25f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2));
        allItems.Add(new Item(1, "Ember_Root", "Ember Root!", Item.TypeOfItem.Consumables, 0f, 0f, 0f, 10f, 10, 0, 0, 1.2f, 0, 0, 0, 0, 0, 6));

        //Miscellaneous
        allItems.Add(new Item(11, "Key", "unlocks secret door!", Item.TypeOfItem.Miscellaneous, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0f, 0f, 0, 3));
        allItems.Add(new Item(12, "Sphere_of_Renewal", "Sphere of Renewal!", Item.TypeOfItem.Miscellaneous, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));

        if (inventory != null)
        {
        for (int i = 0; i < allItems.Count-3; i++) // <-----------------------------------------------TA BORT SEN-------------------------------------------------------------------------------------------
        {
                inventory.inventoryItems.Add(allItems[i]);
        }
        }// <---------------------------------------------------------------------------TA BORT SEN-------------------------------------------------------------------------------------------

    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {

        }
    }

}

