﻿using System.Collections;
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

    private bool isSelected;

    public int IndexInList {
        get { return this.indexInList; }
        set { indexInList = value; } }

    public Item ItemOnSlot {
        get{ return this.itemOnSlot; }
        set { itemOnSlot = value; } }

    private Image Imageslot;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        descText = this.transform.GetChild(2);
        //descText.gameObject.SetActive(false);
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

    public void UpdateCount()
    {
        descText.GetComponent<Text>().text = "wat";
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected : " + gameObject.name);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Deselected : " + gameObject.name);
    }

    private void Update()
    {
        if (isSelected && Input.GetAxis("Interact") > 0.1)
        {
            TaskOnClick();
        }
    }

    private void DropItem()
    {
        Inventory.instance.inventoryItems.RemoveAt(IndexInList);
        Inventory.instance.UpdateInventory();

    }

    void TaskOnClick()
    {  
        switch (itemOnSlot.ItemType)
        {
            case Item.TypeOfItem.Headgear:
                Inventory.instance.EquipItem(indexInList, 3);
                break;

            case Item.TypeOfItem.Armor:
                Inventory.instance.EquipItem(indexInList, 4);
                break;

            case Item.TypeOfItem.Boots:
                Inventory.instance.EquipItem(indexInList, 2);
                break;

            case Item.TypeOfItem.Weapon:
                Inventory.instance.EquipItem(indexInList, 0);
                break;

            case Item.TypeOfItem.Ranged:
                Inventory.instance.EquipItem(indexInList, 1);
                break;

            case Item.TypeOfItem.Consumables:
                PlayerManager.instance.UseConsumable(itemOnSlot);
                Inventory.instance.inventoryItems.RemoveAt(indexInList);
                Inventory.instance.UpdateInventory();
                
                break;
        }
    }
}

//Stina Hedm