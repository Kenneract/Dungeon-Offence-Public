/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Singleton; core game object. Manages incoming and outgoing item prefab objects, as well as displaying the inventory and crafting screen.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//TODO: Move anything crafting related into it's own "Crafting.cs" file, and rename the InventoryManager object to just "Inventory", so we have "inventory.inventorymanager" and "inventory.crafting"


[Serializable]
public struct StartItem {
    public GameObject prefab;
    public int quantity;
}//EnemySpawn

public class InventoryManager : MonoBehaviour
{
    public int maxSlots = 16;
    public int maxEquipSlots = 2;
    [Space]
    //public List<GameObject> craftablePrefabs = new List<GameObject>(); //DEPRECATED IN FAVOUR OF THE ItemHolder
    private int usedSlots = 0; //TODO: MAKE PRIVATE
    public GameObject inventoryWindow;
    private Controls controls; //USING NEW UNITY INPUT SYSTEM
    private Dictionary<string, int> items = new Dictionary<string, int>(); //Lookup reference; [item name : invslot index] pairs. Purely for efficiency/ease of use
    [HideInInspector] public InvSlot[] slots;
    [HideInInspector] public InvSlot[] equipSlots;
    public GameObject inventorySlotsParent; //The parent object to all the inventory slots
    public GameObject hotbarSlotsParent; //The parent object to all the inventory hotbar slots
    public GameObject equipmentSlotsParent; //The parent object to all the equipment slots (not part of normal inventory)
    public GameObject craftingContent; //The content object for placing crafting recipies
    public GameObject craftOptionPrefab; //A ref to the prefab for crafting options
    [Space]
    [Header("Level Transision")]
    [Tooltip("When switching levels, what percent of the current scrap & other resources should be kept")]
    public float resourceKeep = 0.25f;
    [Tooltip("When switching levels, what percent of the current tower ammo should be kept")]
    public float ammoKeep = 0.25f;
    [Tooltip("The list of items that the user should get if this is the first level. Gets overridden with items from the last level, if there was one.")]
    public StartItem[] levelItems;
    [Tooltip("The list of items that the user should ALWAYS start with.")]
    public StartItem[] startItems;

    #region Callbacks
    public delegate void OnItemChanged(); //Part of callback for when items change
    public OnItemChanged onItemChangedCallback; //Part of callback for when items change
    public delegate void OnInventoryOpen(); //Part of callback for when 
    public OnItemChanged onInventoryOpenCallback; //Part of callback for when 
    public delegate void OnInventoryClose(); //Part of callback for when 
    public OnItemChanged onInventoryCloseCallback; //Part of callback for when
    public delegate void OnEquipChanged(); //Callback for when armour/equipment is equipped, unequiped, or changed
    public OnItemChanged onEquipChangedCallback; //Callback for when armour/equipment is equipped, unequiped, or changed
    #endregion


    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static InventoryManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of InventoryManager found. Recovering some items from the last one.");
            LoadLastItems(); //Load some of the items
            Destroy(instance.gameObject); //Destroy old inventory
        }//if
        instance = this;
        DontDestroyOnLoad(this); // Make instance persistent.

    }//SetSingleton()
    #endregion

    void LoadLastItems()
    {//Looks at the current InventoryManager instance and loads a small percent of the items into the levelItems list.
        //Lazily clear & recreate LevelItems with the max possible size
        levelItems = null;
        levelItems = new StartItem[maxSlots];

        int i = 0;
        foreach (InvSlot slot in InventoryManager.instance.slots)
        {
            ItemInfo inf = slot.itemInfo;
            //Find the appropriate percentage to keep
            var keepPerc = 0.0f;
            if (inf.IsTowerAmmo()) keepPerc = ammoKeep;
            if (inf.IsResource()) keepPerc = resourceKeep;
            //Only add the item if some of it should be kept
            if (keepPerc > 0.00f)
            {
                levelItems[i].prefab = slot.itemInfo.MyPrefab();
                levelItems[i].quantity = (int)(slot.itemInfo.quantity*keepPerc);
                i++;
            }//if
        }//foreach
    }//LoadLastItems()

    void Awake()
    {
        SetSingleton();
        
        controls = new Controls();

        //Get all the valid inventory slots
        slots = new InvSlot[maxSlots];
        int i = 0;
        foreach (InvSlot slot in hotbarSlotsParent.GetComponentsInChildren<InvSlot>())
        {
            slots[i] = slot;
            i++;
        }//foreach
        foreach (InvSlot slot in inventorySlotsParent.GetComponentsInChildren<InvSlot>())
        {
            slots[i] = slot;
            i++;
        }//foreach
        if (i != maxSlots) Debug.LogWarning(string.Format("Only {0} of expected {1} inventory slots were found.", i, maxSlots));

        //Get all the equipment slots
        i = 0;
        equipSlots = new InvSlot[maxEquipSlots];
        foreach (InvSlot slot in equipmentSlotsParent.GetComponentsInChildren<InvSlot>())
        {
            equipSlots[i] = slot;
            i++;
        }//foreach
        if (i != maxEquipSlots) Debug.LogWarning(string.Format("Only {0} of expected {1} equipmennt slots were found.", i, maxEquipSlots));

    }//Awake()

    void OnEnable()
    {
        if (controls != null) controls.Enable();
    }
    void OnDisable()
    {
        if (controls != null) controls.Disable();
    }

    void Start()
    {// Start is called before the first frame update
        if (ItemHolder.instance == null) Debug.LogWarning("ItemHolder singleton was not found in this scene - this is critical for the function of the inventory.");
        
        controls.Inventory.Toggle.started += context => OnInvToggle(); //Hook into buttonpress event
        controls.Menu.Close.started += context => CloseInv(); //Hook into buttonpress event

        //Load crafting recipies
        LoadCraftingRecipies();

        //Add starting item(s)
        foreach (StartItem itm in startItems)
        {
            //Skip this item if it isnt defined
            if (itm.prefab == null || itm.quantity < 1) continue;
            //Add the given number of this item to inventory (This was just cleaner than having to instantiate an ItemInfo to use the quantity field)
            var itemInfo = itm.prefab.GetComponent<ItemInfo>();
            for (int i=0; i <itm.quantity; i++)
            {
                AddItem(itemInfo);
            }//for
        }//foreach

        //Add level-specific item(s)
        foreach (StartItem itm in levelItems)
        {
            //Skip this item if it isnt defined
            if (itm.prefab == null || itm.quantity < 1) continue;
            //Add the given number of this item to inventory (This was just cleaner than having to instantiate an ItemInfo to use the quantity field)
            var itemInfo = itm.prefab.GetComponent<ItemInfo>();
            for (int i=0; i <itm.quantity; i++)
            {
                AddItem(itemInfo);
            }//for
        }//foreach

        //Note the inventory is ready & Give a final item change callback so things settle
        HotbarManager.instance.OnInvReady();
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();

        //Close the inventory
        CloseInv();

    }//Start()

    void LoadCraftingRecipies()
    {
        //Run through all defined known items
        foreach (GameObject prefab in ItemHolder.instance.itemPrefabs)
        {
            var info = prefab.GetComponent<ItemInfo>();
            if (info.craftable)
            {
                //Instantiate a new crafting option object as a child of the crafting scroll window
                var craft = Instantiate(craftOptionPrefab, new Vector2(0,0), Quaternion.identity, craftingContent.transform);
                //Set the prefab on the crafting option to be this current item
                craft.GetComponent<CraftingOption>().SetPrefab(prefab);
                //print(string.Format("Discovered craftable {0}", info.itemName));
            }//if
        }//for-each   
    }//LoadCraftingRecipies()

    public float GetEquipDefense()
    {//Gets the total percent of 
        //if (onEquipChangedCallback != null) onEquipChangedCallback.Invoke(); //POTENTIALLY NOT USED ANYMORE
        float defTotal = 0.00f;
        foreach (InvSlot slot in equipSlots) {
            try {
                defTotal += slot.GetPrefab().GetComponent<ArmourInfo>().defense;
            } catch { //ignore empty slots
            }
        }//foreach
        return defTotal;
    }//GetEquipDefense

    void CloseInv()
    {//Closes the inventory window
        if (inventoryWindow.activeSelf) {
            //Inventory was open; close it
            inventoryWindow.SetActive(false);
            InvTooltip.instance.GetComponent<InvTooltip>().Hide(); //Hide the tooltip if the window is closing
            // Invoke callback noting inventory opened/closed
            if (onInventoryCloseCallback != null) onInventoryCloseCallback.Invoke(); //POTENTIALLY NOT USED ANYMORE
            //Note menu was closed
            GameManager.instance.MenuClose();
        }//if
    }//CloseInv()

    void OpenInv()
    {//Attempts to open the inventory
        if (!inventoryWindow.activeSelf) {
            if (!GameManager.instance.AreMenusOpen()) {
                //No menus open; open inventory
                inventoryWindow.SetActive(true);
                // Invoke callback noting inventory opened/closed
                if (onInventoryOpenCallback != null) onInventoryOpenCallback.Invoke(); //POTENTIALLY NOT USED ANYMORE
                GameManager.instance.MenuOpen();
            } else {
                return; //Divert if a menu is open
            }//if (no menu open)
            if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
        }//if
    }//OpenInv()

    public void OnInvToggle()
    {//Event triggered when inventory button is pressed

        //ONLY ALLOW OPENING INVENTORY IF NO OTHER WINDOW IS OPEN
        if (inventoryWindow.activeSelf) {
            CloseInv();
        } else {
            OpenInv();
        }//if (inv open)
    }//OnInvToggle()

    InvSlot GetSlot(string itemName)
    {//Returns the InvSlot object that contains the given item. Returns null if no slot found.
        foreach (InvSlot slot in slots)
        {
            if (slot.GetName() == itemName) return slot;
        }//foreach
        return null;
    }//GetSlot(string)

    InvSlot GetEquipmentSlot(string itemName)
    {//Returns the InvSlot object that contains the given item in the equipmennt slots. Returns null if item is not in equipment.
        foreach (InvSlot slot in equipSlots)
        {
            if (slot.GetName() == itemName) return slot;
        }//foreach
        return null;
    }//GetEquipmentSlot(string)

    private int GetEmptyIndex()
    {//Returns the index of the first empty InvSlot. Returns -1 if none are available.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty()) {return i;}
        }//for
        return -1;
    }//GetEmptyIndex()

    public int HasItem(string itemName)
    {//Returns how many of the given item the user has.
        var slot = GetSlot(itemName);
        if (slot != null) {return slot.GetQnt();}
        return 0;
    }//HasItem(string)


    public bool AddItem(ItemInfo info)
    {//Adds a given item to this inventory, assuming there is space. Returns true if successful, false if not.

        if (usedSlots >= maxSlots) return false;

        //Throw a warning if this item isnt in the prefab list
        var pref = ItemHolder.instance.GetPrefab(info.itemName);
        if (pref == null) Debug.LogWarning("The inventory item \"" + info.itemName + "\" is not in the ItemHolder! This will likely cause errors down the line.");

        //If this is an item we have equipped, stack it there instead
        var eqSlot = GetEquipmentSlot(info.itemName);
        if (eqSlot != null)
        { //Attempt to add item into equipment slot
            eqSlot.AddQuantity(info.quantity); //(most of the time that will be 1)
        } else {
            //Attempt to add item to normal slots
            var slot = GetSlot(info.itemName);
            if (slot != null)
            {
                //Already have item
                slot.AddQuantity(info.quantity); //(most of the time that will be 1)
            } else {
                //Adding as new item
                int pos = GetEmptyIndex(); //KNOW HAVE AT LEAST ONE SPOT AVAILABLE, SO NO ERROR CHECKING NEEDED HERE
                items.Add(info.itemName, pos); //Create new Name:Index pair for this item
                slots[pos].SetItem(info);
                usedSlots++;
            }//if
        }//if (in equipment slot)

        // Invoke callback noting item list has changed
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
        return true;
    }//AddItem(ItemInfo)



    public void TestAddScrap()
    {//TESTING METHOD; ADDS 10 SCRAP TO INVENTORY
        for (int i = 0; i <10; i++)
        {
            AddItem("Scraps");
        }
    }

    public void TestAddOPSword()
    {
        AddItem("OP Sword");
    }


    bool AddItem(string itemName)
    {//Adds a given item to this inventory, assuming there is space. Returns true if successful, false if not.
        //Get the prefab of the item
        var pref = ItemHolder.instance.GetPrefab(itemName);
        //Divert if cannot find item prefab
        if (pref == null)
        {
            Debug.LogWarning("Unable to add item \"" + itemName + "\" to inventory by itemName; Prefab is not in ItemHolder.");
            return false;
        }//if
        //Add item
        return AddItem(pref.GetComponent<ItemInfo>());
    }//AddItem(string)

    public bool RemItem(string itemName, int quantity)
    {//Removes a given quantity of a given item from this inventory. Returns true if successful, false if unable to remove item or no item exists.
        //Get slot
        var slot = GetSlot(itemName);

        if (slot != null && slot.GetQnt() >= quantity)
        {//Have sufficient amount of item.
            slot.RemQuantity(quantity);
            //Clear slot if item was completely depleted
            if (slot.IsEmpty())
            {
                items.Remove(itemName);
                usedSlots--;
                slot.ClearSlot();
            }//if
            
            // Invoke callback noting item list has changed
            if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
            return true;
        } else {
            return false;
        }//if
    }//RemItem(GameObject, int)

    public bool RemItem(string itemName)
    {//Removes a given item from this inventory. Returns true if successful, false if unable to remove item or no item exists.
        return RemItem(itemName, 1);
    }//RemItem(GameObject)

}//InventoryManager