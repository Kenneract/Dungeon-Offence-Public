/*
* Author: Kennan
* Date: Apr.9.2021
* Summary: Controls the pause menu; reloads the current scene or exits to the main menu.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

	#region Variables
	public GameObject pauseWindow;
	private Controls controls;
	#endregion

	#region Unity Methods
	void Awake()
	{// Use this for initialization
		controls = new Controls();
		controls.Menu.Close.started += context => OnPauseToggle(); //Hook into ESC button press
	}//Start()

	void OnEnable()
	{
		controls.Enable();
	}//OnEnable()

	void OnDisable()
	{
		controls.Disable();
	}//OnDisable()

	void Start()
	{
		CloseMenu(); //Close the menu if its open
	}//Start()
	#endregion

	void OnPauseToggle()
	{//Subscribed to ESC button press; toggles the menu
		if (pauseWindow.activeSelf)
		{
			CloseMenu();
		} else {
			OpenMenu();
		}//if
	}//OnPauseButton()

	void OpenMenu()
	{//Opens the menu
		if (LevelManager.instance.screenReady)
		{//Only allow the menu to be opened if the fade-out is finished.
			pauseWindow.SetActive(true);
			Time.timeScale = 0.0f;
			GameManager.instance.MenuOpen();
		}//if
	}//OpenMenu()

	void CloseMenu()
	{//Closes the menu
		Time.timeScale = 1.0f;
		pauseWindow.SetActive(false);
		GameManager.instance.MenuClose();
	}//CloseMenu()

	public void OnContinueButton()
	{
		Time.timeScale = 1.0f;
		CloseMenu();
	}//OnContinueButton()

	public void OnRestartButton()
	{
		Time.timeScale = 1.0f;
		Destroy(InventoryManager.instance.gameObject); //Destroy invenntory so it doesnt reload the items
		Destroy(ItemHolder.instance.gameObject); //Destroy item holder so it doesnt think we loaded into a new level and show the popup
		LevelManager.instance.TransistionReload();
	}//OnRestartButton()

	public void OnMainMenuButton()
	{
		Time.timeScale = 1.0f;
		LevelManager.instance.TransitionMainMenu();
	}//OnMainMenuButton()
}//PauseMenuController