using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    //array of items in the inventory
    public List<Item> inventoryItems = new List<Item>();

    //public Item[] inventoryItems = new Item[12];

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

    //reference to database
    private ItemDatabase database;
    private InventorySlot inventorySlotScript;
    private EquippedSlot equippedSlotScript;

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
    }

    void Start()
    {

        database = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
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

        for (int i = 0; i < 12; i++)
        {
            Button button = Instantiate(inventorySlot);
            button.name = "slot" + i.ToString();
            inventoryButton[i] = button;
            button.transform.SetParent(canvas.transform, false);
        }

        //close inventory
        eqCanvas.gameObject.SetActive(false);
        showInventory = false;
    }

    //Updates inventryUI after changes.
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
}

//Stina Hedman


