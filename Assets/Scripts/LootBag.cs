using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour {

    //Inventory inventory;
    [SerializeField]
    private string itemInBag;

    private bool pickedUp;

    //used to set item in bag. Set by the enemy that drops it.
    public string ItemInBag
    {
        set
        {
            itemInBag = value;
        }
    }

    private void Start()
    {
        WaitRemoveRigidbody();
    }

    //allowes player to pick up item if its within range and using the interact key.
    private void OnTriggerStay(Collider other)
    {
        //if (collision.transform.tag == "Player" && Input.GetAxis("Interact") > 0.1 && !pickedUp && Inventory.instance.inventoryItems.Count < 12)
        if (other.transform.tag == "Player" && Input.GetAxis("Interact") > 0.1 && !pickedUp)
        {
            Inventory.instance.AddItem(itemInBag);
            pickedUp = true;
            Destroy(gameObject, 0.2f);
            Debug.Log("picked up" + itemInBag);
        }        
    }

    //keep the rigidbody from moving when pushed.
    IEnumerator WaitRemoveRigidbody()
    {
        yield return new  WaitForSeconds(1f);
        GetComponent<Rigidbody>().detectCollisions = false;
    }
}
//stina hedman
