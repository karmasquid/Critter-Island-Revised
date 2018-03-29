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
            keyname = "Gold Key";
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("found player");

            foreach (Item i in inventory.inventoryItems)
            {
                if (i.Name == keyname)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
