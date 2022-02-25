/*
* Author: Kennan
* Date: Apr.6.2021
* Summary: Stores the information of the item stored in this slot, and handles dragged items being dropped on it
*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class InvSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon; //Reference to image on this slot
    public Text countText; //Reference to the quantity text on this slot
    public ItemInfo itemInfo; //Reference to ItemInfo object on this slot; stores name, desc, prefab, etc.
    public bool acceptDrop = true; //If this is unchecked checked, items cannot be dragged into this
    public bool equipmentSlot = false; //If this is checked, only items which are "armour" will be accepted on-drag, and the callback is different

    public bool IsEmpty()
    {//Returns true if this slot is empty
        return (itemInfo.quantity == 0);
    }//IsEmpty()

    public void SetItem(ItemInfo info)
    {//Sets the item this slot contains
        //Mirror ItemInfo properties
        itemInfo.MirrorInfo(info);
        //Load data into display for slot
        countText.text = string.Format("x{0}", itemInfo.quantity);
        icon.sprite = itemInfo.icon;
        countText.enabled = true;
        icon.enabled = true;
        //Clear this slot if an empty item was just loaded
        if (IsEmpty()) ClearSlot();
    }//SetItem(string, Info)

    public string GetName()
    {//Returns the name of the item in this slot
        return itemInfo.itemName;
    }//GetName()

    public GameObject GetPrefab()
    {
        return itemInfo.MyPrefab();
    }//GetPrefab()

    public int GetQnt()
    {
        return itemInfo.quantity;
    }

    public void AddQuantity(int q)
    {
        itemInfo.quantity += q;
        countText.text = string.Format("x{0}", itemInfo.quantity);
    }//AddQuantity(int q)

    public void RemQuantity(int q)
    {
        itemInfo.quantity -= q;
        countText.text = string.Format("x{0}", itemInfo.quantity);
    }

    public void ClearSlot()
    {//Clears the item out of this slot and marks it empty.
        itemInfo.ClearInfo();
        icon.sprite = null;
        icon.enabled = false;
        countText.enabled = false;
    }//ClearSlot()

    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter( PointerEventData eventData ) {
        
        if (!IsEmpty())
        {
            //Debug.Log(string.Format("Enter {0}x {1} ({2})", itemInfo.quantity, itemInfo.itemName, itemInfo.description));
            InvTooltip tooltip = InvTooltip.instance.GetComponent<InvTooltip>();
            tooltip.SetInfo(itemInfo);
            tooltip.Show(gameObject.transform.position);
        } else {
            //Debug.Log("Mouse enter empty slot");
        }
    }//OnPointerEnter(PointerEventData)
   
    // Called when the pointer exits our GUI component.
    // Stop tracking the mouse
    public void OnPointerExit( PointerEventData eventData ) {
        
        if (!IsEmpty())
        {
            //Debug.Log(string.Format("Mouse left {0}", itemInfo.itemName));
            InvTooltip tooltip = InvTooltip.instance.GetComponent<InvTooltip>();
            tooltip.Hide();
        } else {
            //Debug.Log("Mouse left empty slot");
        }
        
    }//OnPointerExit(PointerEventData)

	public void OnDrop(PointerEventData eventData)
	{//IDropHandler implementation
        if (acceptDrop)
        {
            var dragInfo = DragHandler.itemBeingDragged.GetComponent<ItemInfo>(); //item being dropped
            var otherSlot = DragHandler.startParent.GetComponent<InvSlot>(); //slot item originated from

            //If we're set to only take equipment and this item isnt that, divert.
            if (equipmentSlot && !dragInfo.IsEquipment() && !otherSlot.IsEmpty()) {
                //Debug.Log("Drag operation cancelled; the dragged item isnt armour");
                return;
            }

            //If the other slot is set to only take equipment and the item we're giving it isnt that, divert.
            if (otherSlot.equipmentSlot && !itemInfo.IsEquipment() && !IsEmpty()) {
                //Debug.Log("Drag operation cancelled; the target drop slot has something that isnt armour");
                return;
            }

            if (this.gameObject != DragHandler.startParent)
            {//This isnt the item

                //Grab a COPY of the dropped item's info
                var newInfo = this.gameObject.AddComponent<ItemInfo>();
                newInfo.MirrorInfo(dragInfo);

                //Set the original slot to store the item in this slot
                otherSlot.SetItem(itemInfo);

                //Set this slot to the dropped item
                SetItem(newInfo);

                //Delete the temporary copy of info
                Destroy(newInfo);

                //Update tooltip on this spot
                InvTooltip tooltip = InvTooltip.instance.GetComponent<InvTooltip>();
                tooltip.EndDrag();
                tooltip.SetInfo(itemInfo);
                tooltip.Show(gameObject.transform.position);

                //Run the callback(s)
                if (InventoryManager.instance.onItemChangedCallback != null) InventoryManager.instance.onItemChangedCallback.Invoke();
                if (equipmentSlot && InventoryManager.instance.onEquipChangedCallback != null) InventoryManager.instance.onEquipChangedCallback.Invoke();
            }//if
        }//if (accept drop)

	}//OnDrop(PointerEventData)

}//InvSlot
