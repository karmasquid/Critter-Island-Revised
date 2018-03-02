using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    //array of items in the inventory
    public Item[] inventorySlots = new Item[12];

    public Item[] equippedItems = new Item[5];

    //array of itemslots (buttons)    
    public Button[] inventoryUI = new Button[12];

    public Button[] equippedUI = new Button[5];

    public GameObject InventoryUIGo;

    private int moveFromIndex, moveToIndex;

    //sounds open/close inventory
    //private AudioClip invOpenSound, invCloseSound;

    //reference to database
    private ItemDatabase database;

    bool showInventory;

    //public int slotsX = 2, slotsY = 5;

    public static Inventory instance;

    int indexitem = 0;

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

        inventoryUI = GameObject.Find("InventorySlots").GetComponentsInChildren<Button>(true);

        equippedUI = GameObject.Find("EquippedSlots").GetComponentsInChildren<Button>(true);

        //invOpenSound = Resources.Load<AudioClip>("Audio/Inventory/openInv");
        //invCloseSound = Resources.Load<AudioClip>("Audio/Inventory/closeInv");

        InventoryUIGo.SetActive(false);
        showInventory = false;
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < 12; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventoryUI[i].image.sprite = inventorySlots[i].Icon;
               // Debug.Log("woooop");
            }
            else
            {
                // Debug.Log("tom slot inv");
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (equippedItems[i] != null)
            {
                equippedUI[i].image.sprite = equippedItems[i].Icon;
                //  Debug.Log("woooop");
            }
            else
            {
                //  Debug.Log("tom slot equipped");
            }
        }
    }

    void FixedUpdate()
    {

        //open and close inventory.
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!showInventory)
            {
                Debug.Log("inventory Open");
                //SoundManager.instance.PlaySingle(invOpenSound);
                showInventory = true;
                InventoryUIGo.SetActive(true);
                UpdateInventory();
            }

            else
            {
                Debug.Log("inventory Closed");
                //SoundManager.instance.PlaySingle(invCloseSound);
                showInventory = false;
                InventoryUIGo.SetActive(false);
            }
        }
    }


    //creates the visual inventory and loads in itemicons.
    public void OpenInventory()
    {
        
    }

    public void MarkedItem()
    {
        //save index of button thats been clicked and store item

        // if one is allready saved and another is clicked, save second stored and switch places of them.
        // if one is clicked and second is null, move it then remove first.

        //if item type is the same as the name of button being clicked, move and switch.
    }

        // used to add item to inventory by name.
        public void AddItem(string nameOfItem)
    {
        int databaseIndex = -1;

        databaseIndex = database.allItems.FindIndex(i => i.Name == nameOfItem);

        if (databaseIndex != -1)
        {
            inventorySlots[indexitem] = database.allItems[databaseIndex];
            indexitem++;
        }

    }
}

//Stina Hedman


