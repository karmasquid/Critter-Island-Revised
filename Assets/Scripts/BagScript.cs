using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    private List<string> itemsInBag = new List<string>();

    private string itemInBag;
    
    Inventory inventory;

    ItemDatabase itemDatabase;

    public string ItemInBag
    {
        set
        {
            itemInBag = value;
        }
    }

    private void Awake()
    {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();

    }


    private void OnTriggerEnter(Collider col)
    {
                Debug.Log("iteminbox = " + itemDatabase.allItems[11].Name);

        if (col.gameObject.tag == "Player" && inventory.inventoryItems.Count < 12)
        {
            inventory.AddItem(itemInBag);
            inventory.UpdateInventory();

            Destroy(gameObject, 1f);
        }
        
    }
   
} // Stina Hedman
