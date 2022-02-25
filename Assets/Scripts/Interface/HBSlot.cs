/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Manages a hotbar slot UI item. Mirrors an inventory slot, 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HBSlot : MonoBehaviour {

	#region Variables
	public Image icon; //Reference to image on this slot
    public Text countText; //Reference to the quantity text on this slot
	private InvSlot parentSlot; //Reference to the slot this HB slot is mirroring
	#endregion

	public void SetParentSlot(InvSlot prnt)
	{
		parentSlot = prnt;
		if (prnt == null) {print("HBSlot: PARENT NOT GIVEN TO SetParentSlot()");}
	}//SetParentSlot(InvSlot)

	public InvSlot GetParentSlot()
	{
		return parentSlot;
	}//GetParentSlot()

	void OnEnable()
	{
	}//OnEnable

	void Start()
	{// Use this for initialization
		//Subscribe to inventory callback
		InventoryManager.instance.onItemChangedCallback += UpdateDisplay;
		UpdateDisplay();
	}//Start()

	void UpdateDisplay()
	{//Using data from the parent inventory slot, update the display of this hotbar slot
		try {
			var parentInfo = parentSlot.itemInfo;
			icon.sprite = parentInfo.icon;
			countText.text = string.Format("x{0}", parentInfo.quantity);
			countText.enabled = true;
			icon.enabled = true;
			//Clear this slot if an empty item was just loaded
			if (parentSlot.IsEmpty()) ClearDisplay();
		} catch {
			//Debug.LogError("This HBSlot is missing parent! Update skipped; this is a staging issue. (THIS SHOULD BE FIXED)"); [THIS IS NOW EXPECTED BEHAVIOUR AT THE START]
		}//try-catch

	}//UpdateDisplay()

    void ClearDisplay()
    {//Clears the display of this slot
		//if (parentSlot.itemInfo.itemName != "") print("Clearing hotbar slot with " +  parentSlot.itemInfo.itemName);
        icon.sprite = null;
        icon.enabled = false;
        countText.enabled = false;
    }//ClearDisplay()

	public bool IsEmpty()
	{
		return parentSlot.IsEmpty();
	}//IsEmpty()

	public void Highlight()
	{//Highlights this slot as the one thats selected.
		GetComponent<Image>().color = Color.cyan;
	}//Highlight()

	public void UnHighlight()
	{//Un-Highlights this slot
		GetComponent<Image>().color = Color.white;
	}//UnHighlight()

}//HBSlot