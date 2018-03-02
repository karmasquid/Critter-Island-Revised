using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    //list of all items in game
    public List<Item> allItems = new List<Item>();

    private Inventory inventory;

    private void Start()
    {

        //(int Id, string Name, string Desc, TypeOfItem KindOfGear, int DamageBuff, int HealthBuff, float StaminaRechargeBuff, int Damage, bool InfiniteAmmo, int AmmoCount, float FireRate, float MeleeRange, float ThrowRange, int StaminaLoss)

        //Weapons
        allItems.Add(new Item(1, "W1", "Weapon 1 is Supermegaawsome", Item.TypeOfItem.Weapon, 5, 0, 0f, 10, true, 0, 0f, 0.5f, 0f, 7));
        allItems.Add(new Item(2, "W2", "Supermegaawsome weapon2!", Item.TypeOfItem.Weapon, 10, 0, 0f, 15, true, 0, 0f, 0.7f, 0f, 10));

        //Equippable
        allItems.Add(new Item(3, "e1", "Ba-Boom!", Item.TypeOfItem.Equippable, 0, 0, 0f, 20, false, 15, 2f, 0f, 5f, 3));
        allItems.Add(new Item(4, "e2", "Ba-Boom!", Item.TypeOfItem.Equippable, 0, 0, 0f, 25, false, 10, 2f, 0f, 5f, 3));

        //Helmets
        allItems.Add(new Item(5, "h1", "Helmet1 is helmet1!", Item.TypeOfItem.Helmet, 0, 5, 0.5f, 0, false, 0, 0, 0, 0, 0));
        allItems.Add(new Item(6, "h2", "Helmet1 is helmet1!", Item.TypeOfItem.Helmet, 0, 3, 0.5f, 0, false, 0, 0, 0, 0, 0));

        //Armors
        allItems.Add(new Item(7, "a1", "Armor DE LA AWSOME!", Item.TypeOfItem.Armor, 0, 5, 0.5f, 0, false, 0, 0, 0, 0, 0));
        allItems.Add(new Item(8, "a2", "ANOTHER AWSOME ARMOR.", Item.TypeOfItem.Armor, 0, 5, 1f, 0, false, 0, 0, 0, 0, 0));

        //Boots
        allItems.Add(new Item(9, "b1", "Fart away!", Item.TypeOfItem.Boots, 0, 5, 1f, 0, false, 0, 0, 0, 0, 0));
        allItems.Add(new Item(10, "b2", "Super pretty boots!", Item.TypeOfItem.Boots, 0, 5, 1.5f, 0, false, 0, 0, 0, 0, 0));

        //Miscellaneous
        allItems.Add(new Item(9, "m1", "Unlocks secret door", Item.TypeOfItem.Miscellaneous, 0, 0, 0f, 0, false, 0, 0f, 0f, 0f, 0));
        allItems.Add(new Item(10, "m2", "Unlocks a supersecret door", Item.TypeOfItem.Miscellaneous, 0, 0, 0f, 0, false, 0, 0f, 0f, 0f, 0));

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        if (inventory != null)
        {
        for (int i = 0; i < 12; i++) // <-----------------------------------------------TA BORT SEN-------------------------------------------------------------------------------------------
        {
            inventory.inventorySlots[i] = allItems[i];
        }
        }// <---------------------------------------------------------------------------TA BORT SEN-------------------------------------------------------------------------------------------

    }

}

