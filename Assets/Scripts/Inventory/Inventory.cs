using System;
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

    private float waitOpenClose = 0.5f;
    private float wait;

    //sounds open/close inventory
    //AudioClip invOpenSound, invCloseSound;

    //references to scripts
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

        wait = Time.time + waitOpenClose;
        CreateInventory();
    }

    void FixedUpdate()
    {

        //open and close inventory.
        if (Input.GetAxis("Inventory") > 0.1f)
        {
            if (!showInventory && Time.time > wait)
            {
                wait = Time.time + waitOpenClose;
                //SoundManager.instance.PlaySingle(invOpenSound);
                UpdateInventory();
                showInventory = true;
                eqCanvas.gameObject.SetActive(true);

            }

            else if (Time.time > wait)
            {
                wait = Time.time + waitOpenClose;
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
            button.transform.SetParent(background.transform, false);
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
        PlayerManager.instance.AddPlayerstats(inventoryItems[index]);

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
            PlayerManager.instance.Removestats(storedItem);
            equippedItems[slot] = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ItemOnSlot = inventoryItems[index];
            equippedButton[slot].GetComponent<EquippedSlot>().ChangeSprite();
            inventoryItems[index] = storedItem;
            UpdateInventory();
        }
    }
    
    //method called when unequipping item.
    public void UnEquipItem(int index)
    {
        if (inventoryItems.Count <= 12 && equippedItems[index] != null)
        {
            inventoryItems.Add(equippedItems[index]);
            equippedItems[index] = null;
            UpdateInventory();
        }
    }

    // used to add item to inventory by name.
    public void AddItem(string nameOfItem)
    {
        int ammoToAdd = 0;

        int databaseIndex = -1;
        
        //find item in database.
        databaseIndex = database.allItems.FindIndex(i => i.Name == nameOfItem);

        if (databaseIndex != -1 && inventoryItems.Count <= 12)
        {
            int eqslotIndex = -1;

            int invSlotIndex = -1;

            //check if item is in inventory and try to add ammo to that item.
            if (database.allItems[databaseIndex].ItemType == Item.TypeOfItem.Ranged || database.allItems[databaseIndex].ItemType == Item.TypeOfItem.Miscellaneous)
            {
                int ammoFull = 99;

                ammoToAdd = database.allItems[databaseIndex].AmmoCount;

                //check if same item is equipped.
               // eqslotIndex = Array.FindIndex(equippedItems, i => i.Name == database.allItems[databaseIndex].Name);

                for (int i = 0; 1 > equippedItems.Length; i++)
                {
                    if (nameOfItem == equippedItems[i].Name)
                    {
                        eqslotIndex = i;
                    }
                }

                //check if same item is in inventory.
                invSlotIndex = inventoryItems.FindIndex(i => i.Name == database.allItems[databaseIndex].Name);

                if (eqslotIndex != -1)
                {
                    //if room for the ammo add all ammo.
                    if ((equippedItems[eqslotIndex].AmmoCount + ammoToAdd) < ammoFull)
                    {
                        equippedItems[eqslotIndex].AmmoCount += ammoToAdd;
                    }

                    //if not room for all of the ammo, save the add to full ammo and but save the remaining ammo.
                    else
                    {

                        ammoToAdd = (equippedItems[eqslotIndex].AmmoCount + ammoToAdd - ammoFull);

                        equippedItems[eqslotIndex].AmmoCount = 99;
                        
                    }
                }

                if (invSlotIndex != -1 && ammoToAdd > 0)
                {
                    //if room for the ammo add all ammo.
                    if ((inventoryItems[invSlotIndex].AmmoCount + ammoToAdd) < ammoFull)
                    {
                        inventoryItems[invSlotIndex].AmmoCount += ammoToAdd;
                    }

                    else
                    {

                        ammoToAdd = (inventoryItems[invSlotIndex].AmmoCount + ammoToAdd - ammoFull);

                        inventoryItems[invSlotIndex].AmmoCount = 99;

                    }
                }
                
                else if (ammoToAdd > 0)
                {
                    inventoryItems.Add(database.allItems[databaseIndex]);
                    int item = inventoryItems.Count - 1;
                    inventoryItems[item].AmmoCount = ammoToAdd;
                    
                }
            }

            else 
            {
                    inventoryItems.Add(database.allItems[databaseIndex]);
            }
            
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


