using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour {

    //Inventory inventory;

    private string itemInBag;

    private bool pickedUp;

    public string ItemInBag
    {
        set
        {
            itemInBag = value;
        }
    }

    //private void Awake()
    //{
    //    inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player" && Input.GetAxis("Interact") > 0.1 && !pickedUp && Inventory.instance.inventoryItems.Count < 12)
        {
            Inventory.instance.AddItem(itemInBag);
            pickedUp = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
//stina hedman
