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

        //()

        //Weapons
        allItems.Add(new Item(1, "W1", "Weapon 1 is Supermegaawsome", Item.TypeOfItem.Weapon, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100 ,0f, 0f, 1,5));
        allItems.Add(new Item(2, "W2", "Supermegaawsome weapon2!", Item.TypeOfItem.Weapon, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));

        //Equippable
        allItems.Add(new Item(3, "e1", "Ba-Boom!", Item.TypeOfItem.Ranged, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(4, "e2", "Ba-Boom!", Item.TypeOfItem.Ranged, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));

        //Helmets
        allItems.Add(new Item(5, "h1", "Helmet1 is helmet1!", Item.TypeOfItem.Headgear, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(6, "h2", "Helmet1 is helmet1!", Item.TypeOfItem.Headgear, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        //Armors
        allItems.Add(new Item(7, "a1", "armor de la awsome!", Item.TypeOfItem.Armor, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(8, "a2", "another awsome armor.", Item.TypeOfItem.Armor, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));

        //Boots
        allItems.Add(new Item(9, "b1", "Fart away!", Item.TypeOfItem.Boots, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(10, "b2", "Super pretty boots!", Item.TypeOfItem.Boots, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));

        //Miscellaneous
        allItems.Add(new Item(11, "m1", "unlocks secret door", Item.TypeOfItem.Miscellaneous, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        allItems.Add(new Item(12, "key", "unlocks a supersecret door", Item.TypeOfItem.Miscellaneous, 10f, 0f, 5f, 0f, 0f, 5f, 15f, 0f, 0f, 100, 0f, 0f, 1, 5));
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

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

