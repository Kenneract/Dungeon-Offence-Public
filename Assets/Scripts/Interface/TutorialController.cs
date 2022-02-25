/*
* Author: Kennan
* Date: Apr.11.2021
* Summary: Controls the tutorial window; a simple paragraph box with several pages, each outlining a portion of the game.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	#region Variables
	public Text titleText;
	public Text bodyText;
	public Button nextButton;
	public Button prevButton;
	//private int stage = 0;
	//private bool haveHoldSword = false;
	//private bool gameStarted = false;
	#endregion

	private int numTexts = 10;
	private int curText = 0;
	private string[] texts;


	#region Unity Methods
	void Start()
	{// Use this for initialization
		texts = new string[numTexts];

		texts[0] = "Welcome! You can move around using the [W][A][S][D] keys. Aim your sword with your mouse and Left-Click to attack. (This only works when you have your sword selected)";
		texts[1] = "Move over to some crates and smash them with your sword to get some pieces of scrap. Once you have 20 you can learn about crafting.";
		texts[2] = "Open your inventory with [E]/[TAB]. You can hover your mouse over items to view info about them, and click-and-drag items between slots. Make sure you keep your sword in your hotbar!";
		texts[3] = "Above this tutorial window is the crafting menu; try crafting a Basic Tower by clicking the \"Craft\" button next to it. It should appear in your hotbar (the bottom slots in the inventory).";
		texts[4] = "Close your inventory and use the scroll wheel or the [1][2][3][4] keys to select the Basic Tower in your hotbar. Hover your mouse over a desired location and left-click to place the tower.";
		texts[5] = "This is an attack tower! Right-click it to open the tower panel. In this menu you can upgrade stats of the tower using scraps and reload its ammo (can be crafted in your inventory).";
		texts[6] = "The countdown at the top-right of the screen is the time until a wave of enemies arrives. You can use your sword to physically attack them and/or use towers to fight them from a distance.";
		texts[7] = "You must prevent enemies from getting to the other end of the level (hurting your friend), and also avoid being mauled by them yourself. You can view your health in the top-left of the screen.";
		texts[8] = "You have two extra lives (top-left), so you and your friend may collectively die three times before you reach a gameover. Once all waves are defeated, visit your friend at the end of the level.";
		texts[9] = "You can close this window with the [X] button above, and start the wave with the [Start Now] button on the top-right. Best of luck fighting off the waves! (Make sure to select your sword again!)";

		bodyText.text = texts[0];
		UpdateInterface();



		//INITIAL STAGE
		//tutText.text = "Initial text";
		//stage = 0;

		//HotbarManager.instance.onMeleeSelectCallback += OnMelee;
		//HotbarManager.instance.onTowerSelectCallback += OnTower;
		//InventoryManager.instance.onInventoryCloseCallback += OnInvClose;
		//InventoryManager.instance.onInventoryOpenCallback += OnInvOpen;
		//InventoryManager.instance.onItemChangedCallback += OnItemChange;
		//GameManager.instance.onGameStartCallback += OnGameStart;
	
	}//Start()
	#endregion


	void UpdateInterface()
	{//Updates the Tutorial title, enables/disables buttons
		titleText.text = string.Format("Tutorial ({0}/{1})", curText+1, numTexts);
		nextButton.interactable = (curText+1 < numTexts); //Enable the next button if we're not at the end yet
		prevButton.interactable = (curText > 0); //Enable the prev button if we're not at the start
	}//UpdateInterface()


	public void OnNext()
	{
		if (curText+1 < numTexts)
		{
			curText++;
			bodyText.text = texts[curText];
		}
		UpdateInterface();
	}//OnNext()

	public void OnPrevious()
	{
		if (curText > 0)
		{
			curText--;
			bodyText.text = texts[curText];
		}//if
		UpdateInterface();
	}//OnPrevious()

	public void OnClose()
	{//Manually called when button pressed
		Destroy(this.gameObject);
	}//OnClose()


	/*
	void OnInvOpen()
	{
		//FIRST STAGE
		if (stage == 0)
		{
			stage = 1;
			tutText.text = "Great! Click and drag your wooden sword from the top of your inventory to the hotbar at the bottom.";
		}
	}//OnInvOpen
	void OnInvClose()
	{
		//SECOND STAGE (skippable)
		if (stage == 1 && !haveHoldSword)
		{
			stage = 2;
			tutText.text = "Use the number keys (1-4) or the scroll wheel to select your sword.";
		}

	}//OnInvClose

	void OnMelee()
	{
		//THIRD STAGE
		if (stage == 2 || stage == 1)
		{
			haveHoldSword = true;
			stage = 3;
			tutText.text = "Walk over to a crate (with WASD), and left click on it to attack it. Collect the scrap that comes out.";
		}//if

	}//OnMelee()

	void OnTower()
	{
		//SEVENTH STAGE
		if (stage == 6)
		{
			stage = 7;
			tutText.text = "Place the tower by left-clicking somewhere on the map";
		}
	}//OnTower()

	void OnItemChange()
	{
		//FOURTH STAGE
		if (stage == 3)
		{
			if (InventoryManager.instance.HasItem("Scrap") > 0)
			{
				stage = 4;
				tutText.text = "You now have some crafting material! Keep breaking crates until you have enough scrap for a tower.";
			}//if
		}//if
		//FIFTH STAGE
		if (stage == 4)
		{
			if (InventoryManager.instance.HasItem("Scrap") >= 15)
			{
				stage = 5;
				tutText.text = "You have enough scrap for a tower; open your inventory and craft one.";
			}//if
		}//if
		//SIXTH STAGE
		if (stage == 5)
		{
			if (HaveTower())
			{
				stage = 6;
				tutText.text = "Nice! Drag it to your hotbar, close your inventory, and select the tower with (1-4) or the scrollwheel.";
			}//if

		}//if

		//EIGHTH STAGE
		if (stage == 7)
		{
			if (!HaveTower())
			{
				stage = 8;
				tutText.text = "Congratulations - you're ready to fight the oncoming wave! Make sure you select your sword again.";

				if (gameStarted)
				{
					Destroy(this.gameObject);
				}
			}//if (no have tower)
		}//if (stage 7)
	}//OnItemChange()

	bool HaveTower()
	{
		return (InventoryManager.instance.HasItem("Basic Tower") > 0 || InventoryManager.instance.HasItem("Bomb Tower") > 0 || InventoryManager.instance.HasItem("Fast Tower") > 0 || InventoryManager.instance.HasItem("Sniper Tower") > 0);
	}

	void OnGameStart()
	{
		gameStarted = true;
		//NINTH STAGE
		if (stage == 8)
		{
			Destroy(this.gameObject);
		}
	}//OnGameStart()
	*/

}//TutorialController