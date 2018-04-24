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

    private void Start()
    {
        WaitRemoveRigidbody();
    }

    private void OnTriggerStay(Collider other)
    {
        //if (collision.transform.tag == "Player" && Input.GetAxis("Interact") > 0.1 && !pickedUp && Inventory.instance.inventoryItems.Count < 12)
        if (other.transform.tag == "Player" && Input.GetAxis("Interact") > 0.1 && !pickedUp)
        {
            Inventory.instance.AddItem(itemInBag);
            pickedUp = true;
            Destroy(gameObject, 0.5f);
            Debug.Log("picked up" + itemInBag);
        }        
    }

    IEnumerator WaitRemoveRigidbody()
    {
        yield return new  WaitForSeconds(1f);
        GetComponent<Rigidbody>().detectCollisions = false;
    }
}
//stina hedman
