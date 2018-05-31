using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour {


    //private Button button;

    private Item itemOnSlot;

    private Item.TypeOfItem itemType;

    private int indexInList;

    public int IndexInList
    {
        get { return this.indexInList; }
        set { indexInList = value; }
    }

    public Item ItemOnSlot
    {
        get { return this.itemOnSlot; }
        set { itemOnSlot = value; }
    }

    private Image Imageslot;

    void Start()
    {
        // om vi vill kunna unequippa items från equippedslots.
        //button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(TaskOnClick);
    }
    public void UpdateSlot(int rotation)
    {
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, rotation);
        }
    }

    public void ChangeSprite()
    {
        this.transform.GetChild(0).GetComponent<Image>().overrideSprite = itemOnSlot.Icon;
    }
    void TaskOnClick()
    {
       if (Inventory.instance.inventoryItems.Count < 12)
        {
            this.transform.GetChild(0).GetComponent<Image>().overrideSprite = null;
            Inventory.instance.UnEquipItem(indexInList);
        }
    }
}
//Stina Hedman