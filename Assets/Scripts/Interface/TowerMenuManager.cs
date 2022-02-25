/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Manages the Tower Menu - Opens/Closes self upon ruquest, displays stats of towers, calls upgrade methods on towers, etc.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuManager : MonoBehaviour {

	#region Variables
	private GameObject curTower;
	private Controls controls;
	
	public GameObject towerWindow;
	public int numCategories = 4;
	public GameObject optionsHolder;

	private UpgradeOption[] upgradeOptions;
	private Tower twr;
	[Space]
	public Text towerNameText;
	public Image towerImg;
	public Image ammoImg;
	public Button reloadButton;
	[Space]
	public Image hpBarImg;
	public Image ammoBarImg;
	public Text ammoText;

	#endregion

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static TowerMenuManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("A TowerMenuManager already exists; replacing it.");
            Destroy(instance.gameObject);
        }//if
        instance = this;
        DontDestroyOnLoad(this); // Make instance persistent.

    }//SetSingleton()
    #endregion

	#region Unity Methods
	void Awake()
	{
		SetSingleton();

		//Subscribe to control events
		controls = new Controls();
		controls.Inventory.Toggle.started += context => TryClose(); //Hook into inventory button press
		controls.Menu.Close.started += context => TryClose(); //Hook into exit button press
		controls.Interact.Action.started += context => TryClose(); //Hook into interaction press

		//Load all the upgrade options
		upgradeOptions = new UpgradeOption[numCategories];
		int i = 0;
		foreach (UpgradeOption opt in optionsHolder.GetComponentsInChildren<UpgradeOption>())
        {
			upgradeOptions[i] = opt;
            i++;
        }//foreach
		if (i != numCategories) Debug.LogWarning(string.Format("Only {0} of expected {1} UpgradeOption's were found.", i, numCategories));
	}//Awake()

	void OnEnable()
	{
		controls.Enable();
	}
	void OnDisable()
	{
		controls.Disable();
	}

	void Start()
	{// Use this for initialization
		TryClose();
	}//Start()
	#endregion

	public void UpdateDisplay()
	{
		//Set the tower name / sprite
		towerNameText.text = curTower.GetComponent<ItemInfo>().itemName;
		towerImg.sprite = curTower.GetComponent<SpriteRenderer>().sprite;

		//Set the ammo image
		ammoImg.sprite = twr.reloadItem.GetComponent<ItemInfo>().icon;
		
		//Update health & ammo bars + text
		hpBarImg.fillAmount = (float)twr.damageableBehaviour.health/twr.damageableBehaviour.maxHealth;
		ammoBarImg.fillAmount = (float)twr.curAmmo/twr.ammoCapacity;
		ammoText.text = string.Format("{0} / {1}", twr.curAmmo, twr.ammoCapacity);

		//Update the reload button
		if (!AmmoFull() && AmmoInInv())
		{
			reloadButton.interactable = true;
		} else {
			reloadButton.interactable = false;
		}//if
		
	}//UpdateDisplay()
    
	private bool AmmoFull()
	{//Returns true if the tower's ammo is full
		return (twr.curAmmo >= twr.ammoCapacity);
	}//AmmoFull
	private bool AmmoInInv()
	{//Returns true if the player has ANY ammo for this tower in their inventory
		var nm = twr.reloadItem.GetComponent<ItemInfo>().itemName;
		return InventoryManager.instance.HasItem(nm)>0;
	}//AmmoInInv

	public void OnReloadButton()
	{//Manually called when the reload button is pressed
		while (AmmoInInv() && !AmmoFull())
		{//Runs until the tower is full, or the player runs out of ammo
			//Remove one ammo from inventory & add to the ammo counter
			twr.curAmmo++;
			InventoryManager.instance.RemItem(twr.reloadItem.GetComponent<ItemInfo>().itemName);
		}//while

	}//OnReloadButton()

            
	private void LoadTowerInfo(GameObject towerIn)
	{
		//Save a ref to the tower
		curTower = towerIn;
		//Save a ref to the Tower script
		twr = curTower.GetComponent<Tower>();

		//Subscribe to tower callbacks
		twr.onInfoChangeback += UpdateDisplay; //Update displays if info changes
		twr.onDeathCallback += TryClose; //Close if tower is destroyed
		
		//Run through the four bars & set them up
		upgradeOptions[0].SetTower(curTower);
		upgradeOptions[1].SetTower(curTower);
		upgradeOptions[2].SetTower(curTower);
		upgradeOptions[3].SetTower(curTower);

		upgradeOptions[0].SetForHealth();
		upgradeOptions[1].SetForRange();
		upgradeOptions[2].SetForDamage();
		upgradeOptions[3].SetForSpeed();

		//Update the display
		UpdateDisplay();

	}//LoadTowerInfo(GameObject)



	public void TryOpen(GameObject towerIn)
	{
		if (!towerWindow.activeSelf && !GameManager.instance.AreMenusOpen())
		{
			GameManager.instance.MenuOpen();
			towerWindow.SetActive(true);
			LoadTowerInfo(towerIn);
		}//if
	}//TryOpen(GameObject)

	private void TryClose()
	{
		if (towerWindow.activeSelf)
		{
			towerWindow.SetActive(false);
			GameManager.instance.MenuClose();
		}//if
	}//TryClose()
}//TowerMenuManager