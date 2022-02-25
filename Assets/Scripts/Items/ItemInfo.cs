/*
* Author: Kennan
* Date: Apr.6.2021
* Summary: Holds generic infomration about an item such as its name, description, and potential crafting requirements.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CraftingIngredient {
    public GameObject resourcePrefab;
    public int quantity;
}//EnemySpawn



public class ItemInfo : MonoBehaviour
{
    public enum ItemTypes {Resource, MeleeWeapon, RangedWeapon, Tower, Equipment, TowerAmmo, Other}
    [Header("General Properties")]
    public ItemTypes itemType;
    public string itemName;
    [Tooltip("Icon to show up in inventory.")]
    public Sprite icon;
    [TextArea] public string description;
    [Space]
    [Header("(Avoid Touching)")]
    [Tooltip("MUST BE 1+. The quantity of this item.")]
    public int quantity = 1; //NOTE: THIS ISNT BOUND TO 1+ BECAUSE IT NEEDS TO BE 0 ON INVENTORY SLOTS. SHOULD ONLY USE A NON-ONE VALUE IF PERFORMANCE IS CONCERN
    [Space]
    [Header("Crafting")]
    public bool craftable = false;
    public CraftingIngredient[] craftingIngredients;


    public GameObject MyPrefab()
    {
        return ItemHolder.instance.GetPrefab(itemName);
        //if (ItemHolder.instance == null) print("Unable to find ItemHolder instance");
        //if (InventoryManager.instance == null) print("Unable to find InventoryManager instance");
        //if (HotbarManager.instance == null) print("Unable to find HotbarManager instance");
    }//MyPrefab()

    public void Start()
    {

        //Attempt to load sprite as icon if one isnt defined. Leave blank if failed.
        if (icon == null) {
            try {
                icon = this.GetComponent<SpriteRenderer>().sprite;
            } catch {
            }//try-catch
        }//if

    }//Start()

    public void ClearInfo()
    {//Clears all info about this item. ONLY INTENDED FOR USE ON INVENTORY/HOTBAR SLOTS.
        itemType = ItemTypes.Other;
        itemName = null;
        description = null;
        craftable = false;
        craftingIngredients = null;
        quantity = 0;
    }//ClearInfo()

    public void MirrorInfo(ItemInfo info)
    {//Mirrors all the values of this ItemInfo to equal the ones of the provided ItemInfo.
        itemType = info.itemType;
        itemName = info.itemName;
        description = info.description;
        craftable = info.craftable;
        craftingIngredients = info.craftingIngredients; //TODO: THERE IS A CONCERN HERE THAT THIS IS JUST A POINTER, AND MAY BECOME EMPTY IF THE SOURCE CONTAINING GAMEOBJECT IS DELETED
        icon = info.icon;
        quantity = info.quantity;
    }//MirrorInfo(ItemInfo)

    public bool IsTower()
    {
        return (itemType == ItemTypes.Tower);
    }
    public bool IsResource()
    {
        return (itemType == ItemTypes.Resource);
    }
    public bool IsMelee()
    {
        return (itemType == ItemTypes.MeleeWeapon);
    }
    public bool IsRanged()
    {
        return (itemType == ItemTypes.RangedWeapon);
    }
    public bool IsEquipment()
    {
        return (itemType == ItemTypes.Equipment);
    }
    public bool IsTowerAmmo()
    {
        return (itemType == ItemTypes.TowerAmmo);
    }
    public bool IsOther()
    {
        return (itemType == ItemTypes.Other);
    }

}//ItemInfo

//Have a WeaponInfo class that extends this? would have attributes like damage, cooldown, attack colour (and/or animiation)? Would be used by the player