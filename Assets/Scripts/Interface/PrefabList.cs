/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: A workaround for unity's idiotic prefab self-reference garbage. Intended to hold a list of all item prefabs.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabList : MonoBehaviour {

	#region Variables
	[HideInInspector] public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(); //Lookup reference; [item name : prefab] pairs. Purely for efficiency/ease of use
    public GameObject[] itemPrefabs;
	public List<GameObject> newPrefabs = new List<GameObject>(); //Stores the item prefabs that are new for this level (compared to last level)
	#endregion

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static PrefabList instance;
    void SetSingleton()
    {
		print("PrefabList setting self as singleton");
        if (instance != null)
        {
            Debug.Log("Last level's PrefabList found; recording recipies");
			LoadChangedPrefabs(); //Load the new items for this level (if any)
			Destroy(instance.gameObject); //Delete old PrefabList
        } else {
			print("This PrefabList believes it is the first");
		}
		instance = this;
		DontDestroyOnLoad(this); // Make instance persistent.
    }//SetSingleton()
    #endregion

	#region Unity Methods
	void Awake()
	{
		Debug.LogWarning("PrefabList is deprecated! Use ItemHolder instead.");
		SetSingleton();
		//Run through all defined objects and construct a quick lookup dict 
		foreach (GameObject obj in itemPrefabs)
		{
			prefabs.Add(obj.GetComponent<ItemInfo>().itemName, obj);
			//print(obj.GetComponent<ItemInfo>().itemName);
		}
		//print(string.Format("all {0} prefabs loaded", prefabs.Count));
	}//Awake()
	#endregion

	void LoadChangedPrefabs()
	{//Records any new prefabs on this
		print("Loading changed prefabs");
		GameObject[] lastLevelPrefabs = PrefabList.instance.itemPrefabs;
		//Run through every prefab in this level
		foreach (var pref in itemPrefabs)
		{
			//Run through all the prefabs of the last level, and note if this prefab was there too
			bool inLast = false;
			foreach (var lastPref in lastLevelPrefabs)
			{
				if (lastPref == pref) inLast = true;
				break;
			}//foreach (item in last level)
			//If this prefab wasnt in the old level, add it to the new items list
			if (!inLast) {
				newPrefabs.Add(pref);
				Debug.Log("Item \"" + pref.GetComponent<ItemInfo>().itemName + "\" is new for this level!");
			}//if
		}//foreach (item in this level)
	}//LoadLastPrefabs()

	public GameObject GetPrefab(string itemName)
	{
		try {
			return prefabs[itemName];
		} catch {
			return null;
		}//try-catch
	}//GetPrefab(string)
}//PrefabList