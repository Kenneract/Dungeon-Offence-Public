/*
* Author: Kennan
* Date: Mar.17.2021
* Summary: Responsible for governing a single displayed crafting recipie. Auto-fills out the requirement and product fields. Greys self / makes self available if required resources are missing / acquired. If available, adds item to inventory upon clicking craft.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOption : MonoBehaviour {

	#region Variables
	private bool canCraft;
	private GameObject prefab;
	public InvSlot ingredientSlot;
	public InvSlot productSlot;
	public Button button;
	public Image craftArrow;
	private ItemInfo itemInfo;
	private CraftingIngredient ingredInfo;
	[Header("Sounds")]
    public AudioClip craftSound;
	#endregion

	#region Unity Methods
	void Awake()
	{// Use this for initialization
		//Hook into item change event
		InventoryManager.instance.onItemChangedCallback += UpdateDisplay;
	}//Awake()
	#endregion

	public void SetPrefab(GameObject pref)
	{
		//Load prefab into self
		prefab = pref;
		//Load recipie from prefab (ONLY SUPPORTS ONE ITEM)
		itemInfo = prefab.GetComponent<ItemInfo>(); //Load prefab item into into self
		//Set result button to item
		productSlot.SetItem(itemInfo);
		//Set recipie
		ingredInfo = itemInfo.craftingIngredients[0];
		var inf = ingredInfo.resourcePrefab.GetComponent<ItemInfo>();
		ingredientSlot.SetItem(inf);
		ingredientSlot.AddQuantity(ingredInfo.quantity-1); //The quantity would only be at 1, so add the remaining

		//Update display
		UpdateDisplay();
	}//SetPrefab()

	bool HaveMaterial()
	{//Returns true if the inventory contains all the required items
		return (InventoryManager.instance.HasItem(ingredInfo.resourcePrefab.GetComponent<ItemInfo>().itemName) >= ingredInfo.quantity); //If inventory has >= the number of required items
	}//HaveMaterial()

	void UpdateDisplay()
	{//Updates the display of this item; automatically disables self if material not available
		if (HaveMaterial())
		{
			//enable button
			button.interactable = true;
			//set arrow to green
			craftArrow.color = Color.green;
		} else {
			//disable button
			button.interactable = false;
			//set arrow to red
			craftArrow.color = Color.red;
		}
	}//UpdateDisplay()

	void TryCraftItem()
	{
		if (HaveMaterial())
		{
			//Remove materials
			InventoryManager.instance.RemItem(ingredInfo.resourcePrefab.GetComponent<ItemInfo>().itemName, ingredInfo.quantity);

			//Add item
			InventoryManager.instance.AddItem(prefab.GetComponent<ItemInfo>());

			//Play sound
			SoundManager.instance.PlayGlobal(craftSound);
		}//if
	}//TryCraftItem()


	public void OnCraftButton()
	{//Manually called when the crafting button is pressed
		TryCraftItem();
	}//OnCraftButton()

}//CraftingOption