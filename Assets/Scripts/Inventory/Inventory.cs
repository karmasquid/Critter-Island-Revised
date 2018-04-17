﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public List<Item> inventoryItems = new List<Item>();
    public Item[] equippedItems = new Item[5];
    private Image[] equippedButton = new Image[5];
    private Button[] inventoryButton = new Button[12];


    //prefabs
    private Image inventoryBackground;
    private Canvas inventoryCanvas;  
    private Button inventorySlot;
    private Image equippedSlot;
    

    private Image background;
    private Canvas canvas;
    private Canvas eqCanvas;
    private Button slot;
    private Image equipped;

    //sounds open/close inventory
    //private AudioClip invOpenSound, invCloseSound;

    //references to scripts
    private PlayerManager playerManager;
    private ItemDatabase database;
    private InventorySlot inventorySlotScript;
    private EquippedSlot equippedSlotScript;
    private Attacker attacker;

    //slot for weaponitem.
    private Transform playerHand;
    private GameObject meleeWeapon;
    private GameObject rangedWeapon;
    private GameObject rangeWepHUD;

    bool showInventory;
    
    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        database = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        playerHand = GameObject.FindGameObjectWithTag("Player").transform.Find("asset_char_mc_fixed_final/root_JNT/spine_1_JNT/spine_2_JNT/chest_JNT/torso_JNT/R_clavicle_JNT/R_shoulder_JNT/R_elbow_JNT/R_forearm_JNT/R_hand_JNT");
        rangeWepHUD = GameObject.Find("CurrentItem");
        attacker = GameObject.FindGameObjectWithTag("Player").GetComponent<Attacker>();

    }

    void Start()
    {
        inventoryCanvas = Resources.Load<Canvas>("Prefabs/UI/InventoryCanvas");
        inventorySlot = Resources.Load<Button>("Prefabs/UI/InventorySlot");
        equippedSlot = Resources.Load<Image>("Prefabs/UI/EquippedSlot");
        inventoryBackground = Resources.Load<Image>("Prefabs/UI/InventoryBackground");

        //invOpenSound = Resources.Load<AudioClip>("Audio/Inventory/openInv");
        //invCloseSound = Resources.Load<AudioClip>("Audio/Inventory/closeInv");

        CreateInventory();
    }

    void FixedUpdate()
    {

        //open and close inventory.
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!showInventory)
            {
                //SoundManager.instance.PlaySingle(invOpenSound);
                UpdateInventory();
                showInventory = true;
                eqCanvas.gameObject.SetActive(true);

            }

            else
            {
                //SoundManager.instance.PlaySingle(invCloseSound);
                eqCanvas.gameObject.SetActive(false);
                showInventory = false;
            }
        }
    }

    //creates inventory at start.
    private void CreateInventory()
    {
        eqCanvas = Instantiate(inventoryCanvas) as Canvas;
        eqCanvas.transform.name = "InventoryCanvas";

        background = Instantiate(inventoryBackground) as Image;
        background.transform.SetParent(GameObject.Find("InventoryCanvas").transform, false);

        canvas = Instantiate(inventoryCanvas) as Canvas;
        canvas.transform.name = "InventoryCanvas2";
        canvas.transform.SetParent(GameObject.Find("InventoryCanvas").transform, false);


        int eqdegrees = 360 / 5;
        int offset = -(eqdegrees / 2);
        int eqslotRotation;

        //create equipped slots.
        for (int i = 0; i < 5; i++)
        {
            eqslotRotation = i * eqdegrees + offset;
            Image image = Instantiate(equippedSlot, equippedSlot.transform.position, Quaternion.Euler(new Vector3(0, 0, eqslotRotation))) as Image;
            image.name = "equippedSlot" + i.ToString();
            equippedButton[i] = image;
            image.transform.SetParent(eqCanvas.transform, false);
            image.GetComponent<EquippedSlot>().UpdateSlot(-eqslotRotation);
            image.GetComponent<EquippedSlot>().IndexInList = i;
        }

        //create inventory slots.
        for (int i = 0; i < 12; i++)
        {
            Button button = Instantiate(inventorySlot);
            button.name = "slot" + i.ToString();
            inventoryButton[i] = button;
            button.transform.SetParent(canvas.transform, false);
        }

        //closes inventory
        eqCanvas.gameObject.SetActive(false);
        showInventory = false;
    }

    //Updates inventryUI, to be used after changes.
    public void UpdateInventory()
    {
        int itemsInInventory = inventoryItems.Count;

        for (int i = 0; i < 12; i++)
        {
            inventoryButton[i].gameObject.SetActive(false);
        }
        if (itemsInInventory > 0) //place out new buttons
        {
            int slotRotation;
            int degrees = -360 / itemsInInventory;

            for (int i = 0; i < itemsInInventory; i++)
            {
                slotRotation = i * degrees;

                inventoryButton[i].gameObject.SetActive(true);
                inventorySlotScript = inventoryButton[i].gameObject.GetComponent<InventorySlot>();
                inventoryButton[i].gameObject.transform.SetPositionAndRotation(inventoryButton[i].gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, -slotRotation)));
                inventorySlotScript.ItemOnSlot = inventoryItems[i];
                inventorySlotScript.UpdateImage();
                inventorySlotScript.RotateSlot();
                inventorySlotScript.IndexInList = i;
            }

            EventSystem.current.SetSelectedGameObject(inventoryButton[0].gameObject, null);
        }
    }

    //method for moving item to equippeditem.
    public void EquipItem(int index, int slot)
    {
        playerManager.AddPlayerstats(inventoryItems[index]);

        if (slot == 1 || slot == 0)
        {
            if (slot == 1)
            {
                attacker.currentEquipped = inventoryItems[index].Go;
                rangeWepHUD.GetComponent<Image>().sprite = inventoryItems[index].Icon;
            }

            else
            {
                HoldWeapon(inventoryItems[index]);              
            }
        }

        if (equippedItems[slot] == null)
            {
            equippedItems[slot] = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ItemOnSlot = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ChangeSprite();
            inventoryItems.RemoveAt(index);
            UpdateInventory();
            }
        else
            {
            Item storedItem = equippedButton[slot].GetComponent<EquippedSlot>().ItemOnSlot;
            playerManager.Removestats(storedItem);
            equippedItems[slot] = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ItemOnSlot = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ChangeSprite();
            inventoryItems[index] = storedItem;
            UpdateInventory();
        }
    }
    
    //method called when unequipping item
    public void UnEquipItem(int index)
    {
        if (inventoryItems.Count <= 12 && equippedItems[index] != null)
        {
            inventoryItems.Add(equippedItems[index]);
            equippedItems[index] = null;
            UpdateInventory();
        }
    }

    // used to add item to inventory by name. ----------------------------------------------------Trydisshit----------------------------------
    public void AddItem(string nameOfItem)
    {
        int databaseIndex = -1;
        databaseIndex = database.allItems.FindIndex(i => i.Name == nameOfItem);

        if (databaseIndex != -1 && inventoryItems.Count <= 12)
        {
            inventoryItems.Add(database.allItems[databaseIndex]);
        }
        UpdateInventory();
    }
    
    public void HoldWeapon(Item weapon)
    {
        if (meleeWeapon != null)
        {
            Destroy(meleeWeapon.gameObject);
        }
        meleeWeapon = Instantiate(weapon.Go, playerHand, false);
    }
}

//Stina Hedman


