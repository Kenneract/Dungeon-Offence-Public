/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: A rewrite of PrefabList - A workaround for unity's idiotic prefab self-reference garbage. Intended to hold a list of all item prefabs.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {

	#region Variables
	[Header("Items on This Level")]
	public GameObject[] itemPrefabs; //The item prefabs to be used within this level
	[HideInInspector] public Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>(); //Lookup reference; [item name : prefab] pairs. Purely for efficiency/ease of use
	[HideInInspector] public List<GameObject> newPrefabs = new List<GameObject>(); //Item prefabs that are new for this level (compared to last level)
	bool notFirstLevel = false; //Automatically determined based onn if another ItemHolder exists from last level.
	#endregion

	#region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static ItemHolder instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("Last level's ItemHolder found; noting differences and removing it.");
			LoadChangedPrefabs(); //Load the new items for this level (if any)
            Destroy(instance.gameObject); //Destroy old list
			notFirstLevel = true;
        }//if
        instance = this;
        DontDestroyOnLoad(this); // Make instance persistent.
    }//SetSingleton()
    #endregion

	#region Unity Methods
	void Awake()
	{
		//Set singleton
		SetSingleton();

		//Run through all defined item prefabs and construct a quick lookup dictionary
		foreach (GameObject obj in itemPrefabs)
		{
			prefabLookup.Add(obj.GetComponent<ItemInfo>().itemName, obj);
		}//foreach
	}//Awake()
	#endregion

	void LoadChangedPrefabs()
	{//Records any new prefabs on this ItemHolder that werent on the last level
		GameObject[] lastLevelPrefabs = ItemHolder.instance.itemPrefabs;
		//Run through every prefab in this level
		foreach (var pref in itemPrefabs)
		{
			//Run through all the prefabs of the last level, and note if this prefab was there too
			bool inLast = false;
			foreach (var lastPref in lastLevelPrefabs)
			{
				if (lastPref == pref) //SET TO ONLY COUNT CRAFTABLE THINGS
				{
					inLast = true;
					break;
				}//if
			}//foreach (item in last level)
			//If this prefab wasnt in the old level, add it to the new items list
			if (!inLast && pref.GetComponent<ItemInfo>().craftable) {
				newPrefabs.Add(pref);
				Debug.Log("ItemHolder: Item \"" + pref.GetComponent<ItemInfo>().itemName + "\" is new for this level!");
			}//if
		}//foreach (item in this level)

	}//LoadLastPrefabs()

	public GameObject GetPrefab(string itemName)
	{//Attempts to rerieve the prefab for the given item. Returns null if unable to find.
		try {
			return prefabLookup[itemName];
		} catch {
			return null;
		}//try-catch
	}//GetPrefab(string)

	void Start()
	{
		LevelManager.instance.onScreenReadyCallback += OnScreenReady;
	}//Start()

	void OnScreenReady()
	{
		if (notFirstLevel)
		{	
			//Depending on if anything was unlocked or not, call the LevelStartScreenController
			if (newPrefabs.Count > 0)
			{
				LevelStartScreenController.instance.LevelStartNewRecipie(newPrefabs);
			} else {
				LevelStartScreenController.instance.LevelStart();
			}//if
		}//if
	}//OnScreenReady()

}//ItemHolder