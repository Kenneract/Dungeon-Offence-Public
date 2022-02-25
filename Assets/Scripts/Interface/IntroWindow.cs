/*
* Author: Kennan
* Date: Mar.17.2021
* Summary: A temporary script for controller the introduction popup at the start of games.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWindow : MonoBehaviour {

	#region Variable
	#endregion

	#region Unity Methods
	void Start()
	{// Use this for initialization
		try {
			LevelManager.instance.onScreenReadyCallback += OnScreenReady;
		} catch {
			Debug.LogWarning("(IntroWindow) Could not find LevelManager in this scene");
			OnScreenReady(); //If it doesnt exist, screen is definitely ready
		}//try
	}//Start()

	void OnScreenReady()
	{//Manually called when the screen fade-in is complete (so the timescale thing doesnt freeze it)
		this.gameObject.SetActive(true);
		Time.timeScale = 0.0f;
		GameManager.instance.MenuOpen(); //Note a window is open
	}//OnScreenReady()

	public void StartButtonClick()
	{//Called when start button is clicked
		//Set timescale to 1.0 and disable self
		Time.timeScale = 1.0f;
		GameManager.instance.MenuClose(); //Note window is closing
		this.gameObject.SetActive(false);
	}//StartButtonClick()
	#endregion

}//IntroWindow