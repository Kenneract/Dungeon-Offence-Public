/*
* Author: Kennan
* Date: Apr.10.2021
* Summary: Manages the transition/level start window. Notes you only kept some of your items, and shows any new crafting recipies you may have unlocked. Interfaces with ItemHolder.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScreenController : MonoBehaviour {

	#region Variables
	public GameObject noUnlockWindow; //Just the container for the regular window
	public GameObject unlockWindow; //Just the container for the unlock window
	public GameObject unlockContainer; //The parent of any unlocked items
	public GameObject blur;
	public GameObject unlockedItemPrefab;
	#endregion

	#region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static LevelStartScreenController instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LevelStartScreenController found.");
            //Destroy(this.gameObject);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

	#region Unity Methods
	void Awake()
	{
		SetSingleton();
		blur.SetActive(false); //Hide the blur
	}//Awake()
	void Start()
	{// Use this for initialization
		
	}//Start()
	#endregion

	public void OnContinueButton()
	{
		Time.timeScale = 1.0f;
		GameManager.instance.MenuClose();
		Destroy(this.gameObject); //Just straight up destroy this menu
	}//OnContinueButton()

	void EnableWindow()
	{
		GameManager.instance.MenuOpen();
		blur.SetActive(true);
		Time.timeScale = 0.0f;
	}//EnableWindow()

	public void LevelStart()
	{//Manually called if the player just came from another level, but there are no new crafting recipies
		EnableWindow();
		noUnlockWindow.SetActive(true);
	}//LevelStart()

	public void LevelStartNewRecipie(List<GameObject> list)
	{//Manually called (ItemHolder)
		EnableWindow();
		unlockWindow.SetActive(true);
		foreach (var item in list)
		{
			ItemInfo info = item.GetComponent<ItemInfo>();
			print("Adding item " + info.itemName);
			GameObject obj = Instantiate(unlockedItemPrefab, new Vector3(0,0,0), Quaternion.identity, unlockContainer.transform);
			var unlockInfo = obj.GetComponent<UnlockedItem>();
			unlockInfo.itemIcon.sprite = info.icon;
			unlockInfo.itemName.text = info.itemName;
		}//foreach
	}//LevelStartNewRecipie()

	//Only show up if a past inventory was found (meaninng loaded from another level)
	//Only show crafting unlocks if ItemHolder has some new ones


}//LevelStartScreenController