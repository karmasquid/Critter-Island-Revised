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

    //array of itemslots (buttons)    
    private List<Button> inventoryButtons = new List<Button>();

    private Button[] equippedButtons = new Button[5];


    //prefabs
    private Image inventoryBackground;
    private Canvas inventoryCanvas;  
    private Button inventorySlot;
    private Button equippedSlot;

    private Image background;
    private Canvas canvas;
    private Canvas eqCanvas;
    private Button slot;
    private Button equipped;

    //sounds open/close inventory
    //private AudioClip invOpenSound, invCloseSound;

    //reference to database
    private ItemDatabase database;

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
        equippedSlot = Resources.Load<Button>("Prefabs/UI/EquippedSlot");
        inventoryBackground = Resources.Load<Image>("Prefabs/UI/InventoryBackground");
        //invOpenSound = Resources.Load<AudioClip>("Audio/Inventory/openInv");
        //invCloseSound = Resources.Load<AudioClip>("Audio/Inventory/closeInv");

        CreateInventory();
    }

    private void CreateInventory()
    {
        eqCanvas = Instantiate(inventoryCanvas) as Canvas;
        eqCanvas.transform.name = "EquippedCanvas";
        eqCanvas.transform.SetParent(GameObject.Find("Canvas").transform, false);

        background = Instantiate(inventoryBackground) as Image;
        background.transform.SetParent(GameObject.Find("EquippedCanvas").transform, false);

        canvas = Instantiate(inventoryCanvas) as Canvas;
        canvas.transform.name = "InventoryCanvas";
        canvas.transform.SetParent(GameObject.Find("EquippedCanvas").transform, false);

        int eqdegrees = 360 / 5;
        int offset = -(eqdegrees / 2);
        int eqslotRotation;

        for (int i = 0; i < 5; i++)
        {
            eqslotRotation = i * eqdegrees + offset;
            Button button = Instantiate(equippedSlot, equippedSlot.transform.position, Quaternion.Euler(new Vector3(0, 0, eqslotRotation))) as Button;
            button.name = "equippedSlot" + i.ToString();
            equippedButtons[i] = button;
            button.transform.SetParent(eqCanvas.transform, false);
            button.GetComponent<EquippedSlot>().UpdateSlot(-eqslotRotation);
            button.GetComponent<EquippedSlot>().IndexInList = i;
        }

        //close inventory
        eqCanvas.enabled = false;
        showInventory = false;
    }

    public void UpdateInventory()
    {

        if (inventoryButtons.Count > 0) // destroy all previous buttons
        {
            foreach (Transform child in canvas.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        if (inventoryItems.Count > 0) //place out new buttons
        {
            int slotRotation;
            int degrees = 360 / inventoryItems.Count;

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                slotRotation = i * degrees;
                
                Button button = Instantiate(inventorySlot, inventorySlot.transform.position, Quaternion.Euler(new Vector3(0, 0, slotRotation))) as Button;
                button.name = "slot" + i.ToString();
                inventoryButtons.Add(button);
                button.transform.SetParent(canvas.transform, false);
                button.GetComponent<InventorySlot>().ItemOnSlot = inventoryItems[i];
                button.GetComponent<InventorySlot>().UpdateSlot(-slotRotation);
                button.GetComponent<InventorySlot>().IndexInList = i;
                
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
                //SoundManager.instance.PlaySingle(invOpenSound);
                showInventory = true;
                eqCanvas.enabled = true;
                UpdateInventory();
            }

            else
            {
                //SoundManager.instance.PlaySingle(invCloseSound);
                showInventory = false;
                eqCanvas.enabled = false;
            }
        }
    }

    public void EquipItem(int index, int slot)
    {
        if (equippedItems[slot] == null)
            {
            equippedItems[slot] = inventoryItems[index];
            equippedButtons[slot].GetComponent<EquippedSlot>().ItemOnSlot = inventoryItems[index];
            equippedButtons[slot].GetComponent<EquippedSlot>().ChangeSprite();
            inventoryItems.RemoveAt(index);
            UpdateInventory();
            }
        else
            {
            Debug.Log("occupado!");
            Item storedItem = equippedButtons[slot].GetComponent<EquippedSlot>().ItemOnSlot;
            equippedItems[slot] = inventoryItems[index];
            equippedButtons[slot].GetComponent<EquippedSlot>().ItemOnSlot = inventoryItems[index];
            equippedButtons[slot].GetComponent<EquippedSlot>().ChangeSprite();
            inventoryItems[index] = storedItem;
            UpdateInventory();
        }

    }
    
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
    public void AddItem(string nameOfItem) // kommer behöva ändras för att lägga till på rätt position.
    {
        int databaseIndex = -1;
        databaseIndex = database.allItems.FindIndex(i => i.Name == nameOfItem);

        if (databaseIndex != -1 && inventoryItems.Count <= 12)
        {
            inventoryItems.Add(database.allItems[databaseIndex]);
        }

    }
}

//Stina Hedman


