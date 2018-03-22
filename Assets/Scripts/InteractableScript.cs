using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour {


    public string NeededKey;

    Inventory inventory;

    // Use this for initialization
    void Start () {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("found player");

            foreach (Item i in inventory.inventoryItems)
            {
                if (i.Name == NeededKey)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
