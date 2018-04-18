using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, ISelectHandler
{

    private Button button;

    private Item itemOnSlot;

    private Item.TypeOfItem itemType;

    private int indexInList;

    private Transform descText;

    private PlayerManager playerManager;

    public int IndexInList {
        get { return this.indexInList; }
        set { indexInList = value; } }

    public Item ItemOnSlot {
        get{ return this.itemOnSlot; }
        set { itemOnSlot = value; } }

    public PlayerManager PlayerManagerScript
    { set { playerManager = value; } }

    private Inventory inventory;
    private Image Imageslot;

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        descText = this.transform.GetChild(2);
        descText.gameObject.SetActive(false);
    }

    public void RotateSlot()
    {

        foreach (Transform child in transform)
        {
            child.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -gameObject.transform.rotation.z));
        }
    }

    public void UpdateImage()
    {
        this.transform.GetChild(0).GetComponent<Image>().overrideSprite = itemOnSlot.Icon;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected : " + gameObject.name);
    }

    private void DropItem()
    {
        inventory.inventoryItems.RemoveAt(IndexInList);
        inventory.UpdateInventory();

    }

    void TaskOnClick()
    {  
        switch (itemOnSlot.ItemType)
        {
            case Item.TypeOfItem.Headgear:
                inventory.EquipItem(indexInList, 3);
                break;

            case Item.TypeOfItem.Armor:
                inventory.EquipItem(indexInList, 4);
                break;

            case Item.TypeOfItem.Boots:
                inventory.EquipItem(indexInList, 2);
                break;

            case Item.TypeOfItem.Weapon:
                inventory.EquipItem(indexInList, 0);
                break;

            case Item.TypeOfItem.Ranged:
                inventory.EquipItem(indexInList, 1);
                break;

            case Item.TypeOfItem.Consumables:
                playerManager.UseConsumable(itemOnSlot);
                inventory.inventoryItems.RemoveAt(indexInList);
                inventory.UpdateInventory();
                
                break;
        }
    }
}

//Stina Hedm