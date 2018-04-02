using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    private List<string> itemsInBag = new List<string>();

    private Item itemInBag;

    [Header("Possible Drops")]
    [SerializeField]
    private bool bronzeKey;
    [SerializeField]
    private bool silverKey;
    [SerializeField]
    private bool goldKey;

    Inventory inventory;

    ItemDatabase itemDatabase;

    // Use this for initialization
    void Start () {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();

        if (bronzeKey)
        {
            itemsInBag.Add("Bronze Key");
        }
        if (silverKey)
        {
            itemsInBag.Add("Silver Key");
        }
        if (goldKey)
        {
            itemsInBag.Add("Gold Key");
        }
        
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
