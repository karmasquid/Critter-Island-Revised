using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    private Item itemInBag;

    public string thisItem;

    Inventory inventory;
    ItemDatabase itemDatabase;

	// Use this for initialization
	void Start () {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();


	}

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
                Debug.Log("iteminbox = " + itemDatabase.allItems[11].Name);

        if (col.gameObject.tag == "Player")
        {
            inventory.inventoryItems.Add(itemDatabase.allItems[11]);
            inventory.UpdateInventory();

            Destroy(gameObject);
        }
        
    }
   
}
