/*
* Author: Kennan
* Date: Apr.9.2021
* Summary: Manages the display of a vertical upgrade bar.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOption : MonoBehaviour {

	#region Variables
	public GameObject barHolder;
	public int numBars = 5;
	public Color barFillColour = Color.red;
	public Color barEmptyColour = Color.white;
	private Image[] barImgs;
	[Space]
	public Text costText;
	public Text typeText;
	public Text valText;
	[Space]
	public Button upgradeButton;
	private enum UpgradeTypes {Health, Range, Damage, Speed}
    private UpgradeTypes upgradeType;
	private GameObject curTower;
	[Header("Sound")]
	public AudioClip upgradeSound;

	private string resourceName; //The name of the required resource.
	private int resourceQnt; //How many resources are required for an upgrade
	#endregion

	#region Unity Methods
	void Start()
	{// Use this for initialization
		loadBars();

		//Subscribe to item change callbacks
		InventoryManager.instance.onItemChangedCallback += UpdateDisplay;

	}//Start()
	#endregion

	void loadBars()
	{
		if (barImgs == null)
		{
			barImgs = new Image[numBars];
			int i = -1;
			foreach (Image img in barHolder.GetComponentsInChildren<Image>())
			{
				if (i >= 0) barImgs[i] = img; //Skip the first image cuz its the background
				i++;
			}//foreach
			if (i != numBars) Debug.LogWarning(string.Format("Only {0} of expected {1} upgrade bar images were found.", i, numBars));
		}//if
	}//loadBars

	public void SetLevel(int level)
	{//Sets the level of the bar; somewhere between 0 and NUM_BARS.
		loadBars(); //Make sure the bars are loaded
		int numEmpty = numBars - level;
		for (int i = 0; i < numEmpty; i++)
		{
			barImgs[i].color = barEmptyColour;
		}//for
		for (int i = numEmpty; i < numBars; i++)
		{
			barImgs[i].color = barFillColour;
		}//for
	}//SetLevel(int)

	public void SetCost(int qnt, string name)
	{
		resourceName = name;
		resourceQnt = qnt;
		costText.text = string.Format("Cost:\n{0} {1}", qnt, name);
	}//SetCost(int, string)

	int GetMaxLvl()
	{
		return curTower.GetComponent<Tower>().upgradeSteps;
	}

	bool AtMaxLvl()
	{
		return GetCurrentLvl() >= GetMaxLvl();
	}//AtMaxLvl

	void UpdateDisplay()
	{//Manually called when the number of items in the inventory changes

		//Check if at max level; disable buttin if is
		//var towerObj = curTower.GetComponent<Tower>();
		//if (curTower.)

		//Try to get tower reference (divert if failed)
		Tower twr;
		try {
			twr = curTower.GetComponent<Tower>();
		} catch {
			return;
		}
		//Set button
		//Check if have enough resources & not at max level
		if (HaveEnough() && !AtMaxLvl())
		{
			upgradeButton.interactable = true;
		} else {
			upgradeButton.interactable = false;
		}//if

		//Set bar
		SetLevel(GetCurrentLvl());

		//Update type data
		if (upgradeType == UpgradeTypes.Health) {
			valText.text = string.Format("{0} / {1}", twr.damageableBehaviour.health, twr.damageableBehaviour.maxHealth);
		}//if
		if (upgradeType == UpgradeTypes.Range) {
			valText.text = string.Format("{0}", twr.curRange.ToString("#.0"));
		}//if
		if (upgradeType == UpgradeTypes.Damage) {
			valText.text = string.Format("{0}", twr.rangedAttackBehaviour.attackDamage);
		}//if
		if (upgradeType == UpgradeTypes.Speed) {
			valText.text = string.Format("{0} shot/sec", (1.00f/twr.rangedAttackBehaviour.attackCooldown).ToString("0.0"));
		}//if

		//Update main window
		TowerMenuManager.instance.UpdateDisplay();

	}//UpdateDisplay()


	public void OnButtonPress()
	{//Manually called when the button of this upgrade option is pressed.
		if (upgradeType == UpgradeTypes.Health) TryUpgradeHealth();
		if (upgradeType == UpgradeTypes.Range) TryUpgradeRange();
		if (upgradeType == UpgradeTypes.Damage) TryUpgradeDamage();
		if (upgradeType == UpgradeTypes.Speed) TryUpgradeSpeed();
		var pitch = Scale(0f, 1f, 0.7f, 1.3f, (float)GetCurrentLvl()/GetMaxLvl()); //0.7 to 1.3 pitch, based on level
		SoundManager.instance.PlayGlobal(upgradeSound, pitch, pitch);
		UpdateDisplay();
	}//OnButtonPress()


	float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
	{//https://forum.unity.com/threads/mapping-or-scaling-values-to-a-new-range.180090/
		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
		return(NewValue);
	}

	//TODO: THERES A LOT OF UGLY REPEATED CODE EVERYWHERE; COULD BE MADE A LOT SMALLER BUT IM ON A TIME CRUNCH SO ITS STAYIN

	bool HaveEnough()
	{//Returns true if the inventory has enough items in it to make this
		return (InventoryManager.instance.HasItem(resourceName)) >= resourceQnt;
	}//HaveEnough()

	public void TryUpgradeHealth()
	{
		//BUTTON IS ONLY ENABLED WHEN ITS POSSIBLE, SO DONT NEED TO CHECK ANYTHING HERE
		Tower twr = curTower.GetComponent<Tower>();
		InventoryManager.instance.RemItem(resourceName, resourceQnt);
		twr.SetHealthLvl(GetCurrentLvl()+1);

	}//TryUpgradeHealth()
	public void TryUpgradeRange()
	{
		//BUTTON IS ONLY ENABLED WHEN ITS POSSIBLE, SO DONT NEED TO CHECK ANYTHING HERE
		Tower twr = curTower.GetComponent<Tower>();
		InventoryManager.instance.RemItem(resourceName, resourceQnt);
		twr.SetRangeLvl(GetCurrentLvl()+1);
	}//TryUpgradeRange()
	public void TryUpgradeSpeed()
	{
		//BUTTON IS ONLY ENABLED WHEN ITS POSSIBLE, SO DONT NEED TO CHECK ANYTHING HERE
		Tower twr = curTower.GetComponent<Tower>();
		InventoryManager.instance.RemItem(resourceName, resourceQnt);
		twr.SetSpeedLvl(GetCurrentLvl()+1);
	}//TryUpgradeSpeed()
	public void TryUpgradeDamage()
	{
		//BUTTON IS ONLY ENABLED WHEN ITS POSSIBLE, SO DONT NEED TO CHECK ANYTHING HERE
		Tower twr = curTower.GetComponent<Tower>();
		InventoryManager.instance.RemItem(resourceName, resourceQnt);
		twr.SetDamageLvl(GetCurrentLvl()+1);
	}//TryUpgradeDamage()



	//rangedAttackBehaviour.attackCooldown;
	//rangedAttackBehaviour.attackDamage;
	//damageableBehaviour.health;
	//curRange;



	private int GetCurrentLvl()
	{
		Tower twr = curTower.GetComponent<Tower>();
		if (upgradeType == UpgradeTypes.Health) return twr.hpLvl;
		if (upgradeType == UpgradeTypes.Range) return twr.rangeLvl;
		if (upgradeType == UpgradeTypes.Damage) return twr.damageLvl;
		if (upgradeType == UpgradeTypes.Speed) return twr.cooldownLvl;
		return -1;
	}//GetCurrentLvl

	public void SetTower(GameObject towerIn)
	{
		curTower = towerIn;
	}//SetTower(GameObject)

	public void SetForHealth()
	{
		upgradeType = UpgradeTypes.Health;
		var comp = curTower.GetComponent<Tower>();
		var qnt = comp.hpResourceReq;
		var resName = comp.upgradeResource.GetComponent<ItemInfo>().itemName;
		SetCost(qnt, resName);
		typeText.text = "Health";
		UpdateDisplay();
	}//SetForHealth
	public void SetForRange()
	{
		upgradeType = UpgradeTypes.Range;
		var comp = curTower.GetComponent<Tower>();
		var qnt = comp.rangeResourceReq;
		var resName = comp.upgradeResource.GetComponent<ItemInfo>().itemName;
		SetCost(qnt, resName);
		typeText.text = "Range";
		UpdateDisplay();
	}
	public void SetForDamage()
	{
		upgradeType = UpgradeTypes.Damage;
		var comp = curTower.GetComponent<Tower>();
		var qnt = comp.damageResourceReq;
		var resName = comp.upgradeResource.GetComponent<ItemInfo>().itemName;
		SetCost(qnt, resName);
		typeText.text = "Damage";
		UpdateDisplay();
	}
	public void SetForSpeed()
	{
		upgradeType = UpgradeTypes.Speed;
		var comp = curTower.GetComponent<Tower>();
		var qnt = comp.speedResourceReq;
		var resName = comp.upgradeResource.GetComponent<ItemInfo>().itemName;
		SetCost(qnt, resName);
		typeText.text = "Speed";
		UpdateDisplay();
	}
}//UpgradeBar