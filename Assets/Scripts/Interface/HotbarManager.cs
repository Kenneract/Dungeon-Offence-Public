/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Sets up hotbar slots to mirror the ones in the inventory, manages which hotbar slot is selected, invokes callbacks when specific items are selected.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HotbarManager : MonoBehaviour {

	#region Variables
	//bool canSelect = true;
	//bool canUse = true;
	int numSlots = 4; //How many hotbar slots there are
	int curSlot = 1; //Which slot is currently selected
	public GameObject hotbarSlotsParent; //Contains slots on self
	public Text itemText; //Ref to the text that displays the current item
	public Text itemTextShadow; //Ref to the text that displays the current item
	[Space]
	public Controls controls; //USING NEW UNITY INPUT SYSTEM
	private InvSlot[] invSlots; //Stpres a ref to the inventory hotbar slots. autofilled
	HBSlot[] hotbarSlots; //Slots on self. autofilled
	private bool loadedSlots = false;
	#endregion

	#region Callbacks
	//OnChangeSlot callback(?) (potentially universal way of things disengaging?)
    public delegate void OnTowerSelect();
    public OnTowerSelect onTowerSelectCallback;
    //public delegate void OnTowerDeselect();
    //public OnTowerDeselect onTowerDeselectCallback;
    public delegate void OnMeleeSelect();
    public OnMeleeSelect onMeleeSelectCallback;
    //public delegate void OnMeleeDeselect();
    //public OnMeleeDeselect onMeleeDeselectCallback;
    public delegate void OnRangedSelect();
    public OnRangedSelect onRangedSelectCallback;
    //public delegate void OnRangedDeselect();
    //public OnRangedDeselect onRangedDeselectCallback;
    public delegate void OnDeselect();
    public OnDeselect onDeselectCallback;
	#endregion

	#region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static HotbarManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("A HotbarManager already exists; deleting self");
			Destroy(this.gameObject);
        } else {
			instance = this;
		}
		
		//DontDestroyOnLoad(this); // Make instance persistent.
    }//SetSingleton()
    #endregion

	#region Unity Methods
	void Awake()
	{
		SetSingleton();
		controls = new Controls();
	}//Awake()
	
	void OnEnable()
	{
		controls.Enable();

	}//OnEnable()

	void Start()
	{// Use this for initialization
		
		//Load controls
        controls.Hotbar.Scroll.performed += context => OnScroll(context); //Hook into buttonpress event
		controls.Hotbar.Slot1.started += context => OnNumPress(1); //Hook into slot select event
		controls.Hotbar.Slot2.started += context => OnNumPress(2); //Hook into slot select event
		controls.Hotbar.Slot3.started += context => OnNumPress(3); //Hook into slot select event
		controls.Hotbar.Slot4.started += context => OnNumPress(4); //Hook into slot select event

	}//Start()
	#endregion

	public void OnInvReady()
	{//Manually called when the inventory is readh
		//Loads slots
		LoadSlots();
		//Link hotbar slots with their parent inventory slots
		InitSlots();
		//Subscribe to inventory callback
		InventoryManager.instance.onItemChangedCallback += CheckCallbacks; //If user just placed a new item into the currently selected slot, need to update
		//Select first slot
		SelectSlot(1);
	}//OnInvReady()

	void LoadSlots()
	{//Loads the parent inventory slots and the actual hotbar slots.
		if (!loadedSlots)
		{//Only try to load if we havent already loaded them
			//Grab ref to the hotbar slots within the inventory
			invSlots = new InvSlot[numSlots];
			for (int i=0; i<numSlots; i++)
			{
				invSlots[i] = InventoryManager.instance.slots[i];
				if (invSlots[i] == null) {
					Debug.Log("HotbarManager: Unable to access hotbar slots within inventory (Tries twice).");
					return;
				}//if
			}//for

			//Find all the hotbar slots
			hotbarSlots = new HBSlot[numSlots];
			int k = 0;
			foreach (HBSlot slot in hotbarSlotsParent.GetComponentsInChildren<HBSlot>())
			{
				hotbarSlots[k] = slot;
				k++;
			}//foreach
			if (k != numSlots) Debug.LogWarning(string.Format("Only {0} of expected {1} hotbar slots were found.", k, numSlots));
			loadedSlots = true;
		}//if
	}//LoadSlots()


	void OnDisable()
	{
		if (controls != null) controls.Disable();
	}


	void OnScroll(InputAction.CallbackContext ctx)
	{
		if (!GameManager.instance.AreMenusOpen())
		{

			Vector2 value = (Vector2)ctx.ReadValueAsObject(); //X axis is horizontal scrolling, if available
			if (value.y > 0 || value.x < 0)
			{//Scrolling up or left; moving to left
				int newSlot = curSlot-1;
				if (newSlot < 1) newSlot = numSlots; //Rollover to right
				SelectSlot(newSlot);
			}
			else if (value.y < 0 || value.x > 0)
			{//Scrolling down or right; moving to right
				int newSlot = curSlot+1;
				if (newSlot > numSlots) newSlot = 1; //Rollover to left
				SelectSlot(newSlot);
			}//if

		}//if (no menus open)
	}//OnScroll()

	void OnNumPress(int num)
	{//Uses some event context to figure out which button was pressed and set that slot. To implement some day.
		if (!GameManager.instance.AreMenusOpen())
		{
			SelectSlot(num);
		}//if (no menus open)
	}//OnNumPress()

	public void SelectSlot(int slotNum)
	{//Highlights the given slot in the HUD, invokes the appropriate callback(s) for that item type, and changes [action] button purpose. Optionally forces the change, even if a menu is open
		if (!loadedSlots) {return;}
		if (slotNum > numSlots || slotNum < 1) {return;}
		for (int i=0; i<numSlots; i++)
		{
			if (i+1 == slotNum) {
				hotbarSlots[i].Highlight();
			} else {
				hotbarSlots[i].UnHighlight();
			}//if
		}//for
		curSlot = slotNum;
		//Check callbacks
		CheckCallbacks();
	}//SelectSlot(int)

	void CheckCallbacks()
	{//Assumes an item was just selected; check it and run the appropriate callback
		//Always have deselected *something*
		if (onDeselectCallback != null) onDeselectCallback.Invoke();

		//Update current item text (just in case)
		var cur = GetCurInvSlot().itemInfo;
		itemText.text = cur.itemName;
		itemTextShadow.text = cur.itemName;

		//if (onTowerSelectCallback == null) print("Nothing is subscribed to TowerSelect callback");
		//if (onMeleeSelectCallback == null) print("Nothing is subscribed to MeleeSelect callback");
		//if (onRangedSelectCallback == null) print("Nothing is subscribed to RangedSelect callback");

		if (cur.IsTower() && onTowerSelectCallback != null) 
		{
			//Debug.Log("Tower has been selected.");
			onTowerSelectCallback.Invoke();
		}//if
		if (cur.IsMelee() && onMeleeSelectCallback != null)
		{
			//Debug.Log("Melee Weapon has been selected.");
			onMeleeSelectCallback.Invoke();
		}//if
		if (cur.IsRanged() && onRangedSelectCallback != null)
		{
			//Debug.Log("Ranged Weapon has been selected.");
			onRangedSelectCallback.Invoke();
		}//if

	}//CheckCallbacks()


	void InitSlots()
	{//Tells the hotbar slots which inventory slots they should be mirroring
		for (int i=0; i<numSlots; i++)
		{
			hotbarSlots[i].SetParentSlot(invSlots[i]);
		}//for
	}//UpdateSlots()

	public InvSlot GetCurInvSlot()
	{//Returns the currently selected INVENTORY SLOT
		return hotbarSlots[curSlot-1].GetParentSlot();
	}//GetCurInvSlot()

}//HotbarManager