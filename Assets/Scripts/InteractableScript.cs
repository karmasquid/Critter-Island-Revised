using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour {

    [Header("One key needed to open the door")]
    public bool bronzeKey;
    public bool silverKey;
    public bool goldKey;

    private string keyname;

    Inventory inventory;

    // Use this for initialization
    void Start () {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        if (bronzeKey)
        {
            keyname = "Bronze Key";
        }
        else if (silverKey)
        {
            keyname = "Silver Key";
        }
        else if (goldKey)
        {
            keyname = "key";
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("at door");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("found player");

            foreach (Item i in inventory.inventoryItems)
            {
                if (i.Name == "key")
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

}// Stina Hedman
