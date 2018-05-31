using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Door : MonoBehaviour {
    
    [SerializeField]
    bool GoldKey, SilverKey, BronzeKey;

    string keyNeeded;

    Vector3 startPos;

    Vector3 endPos;

    bool opened;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        endPos = startPos + (Vector3.down*6);

        this.GetComponent<SphereCollider>().isTrigger = true;

        if (GoldKey)
        {
            keyNeeded = "Gold Key";
        }
        else if (SilverKey)
        {
            keyNeeded = "Silver Key";
        }
        else if (BronzeKey)
        {
            keyNeeded = "Bronze Key";
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetAxis("Interact")>0.1f && !opened)
        {
            int inventoryIndex = -1;

            inventoryIndex = Inventory.instance.inventoryItems.FindIndex(i => i.Name == keyNeeded);

            if (inventoryIndex != -1)
            {

                StartCoroutine(OpenDoor());
                Inventory.instance.inventoryItems.RemoveAt(inventoryIndex);
                Inventory.instance.UpdateInventory();
            }

            else
            {
                Debug.Log("Missing Key");
            }
        }


    }

    //door opening.
    IEnumerator OpenDoor()  
    {
        opened = true;


        GetComponent<SphereCollider>().enabled = false;

       while (this.transform.position.y > endPos.y)
        {


            this.transform.Translate(Vector3.down/25);


            yield return new WaitForSeconds(0.05f);
           
        }

        yield break;
    }
}
