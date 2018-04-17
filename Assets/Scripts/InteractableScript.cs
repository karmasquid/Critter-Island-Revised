using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour {

    [Header("One key needed to open the door")]
    public bool bronzeKey;
    public bool silverKey;
    public bool goldKey;

    private string keyName;

    Inventory inventory;

    // Use this for initialization
    void Start () {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        if (bronzeKey)
        {
            keyName = "Bronze Key";
        }
        else if (silverKey)
        {
            keyName = "Silver Key";
        }
        else if (goldKey)
        {
            keyName = "key";
        }
    }
    private void OnCollisionEnter(Collision col)
    {
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
