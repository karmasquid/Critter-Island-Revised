using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour {

    private Button button;

    private Item itemOnSlot;

    private Item.TypeOfItem itemType;

    private int indexInList;

    public int IndexInList
    {
        get { return this.indexInList; }
        set { indexInList = value; }
    }

    public Item ItemOnSlot {
        get{ return this.itemOnSlot; }
        set { itemOnSlot = value; }
    }

    private Inventory inventory;

    private Image Imageslot;

    void Start()
    {

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    public void UpdateSlot(int rotation)
    {
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, rotation);
            
        }


        this.transform.GetChild(0).GetComponent<Image>().overrideSprite = itemOnSlot.Icon;
    }

    void TaskOnClick()
    {  
        switch (itemOnSlot.ItemType)
        {
            case Item.TypeOfItem.Helmet:
                inventory.EquipItem(indexInList,3);
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

            case Item.TypeOfItem.Equippable:
                inventory.EquipItem(indexInList, 1);
                break;
        }
    }
}