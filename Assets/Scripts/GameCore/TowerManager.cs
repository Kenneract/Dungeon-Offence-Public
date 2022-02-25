using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Author: Kennan and Kevin
* Date: Mar.9.2021
* Purpose: Tower management: placing a tower in a given position, and keeping track of tower objects.
*/

public class TowerManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip buildTower;

    [Space]
    public List<GameObject> towerList = new List<GameObject>(); //Make this invisible in the inspector?


    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static TowerManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TowerManager found.");
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

    void Awake() 
    {
        SetSingleton();
    }//Awake()

    // Start is called before the first frame update
    void Start()
    {
        // Fill in if something needs to start
    }

    // Update is called once per frame
    void Update()
    {
        //Fill in if something needs to update
    }

    public void TowerExpire(GameObject tower)
    {//Manually called by a tower (passing self) when it dies
        towerList.Remove(tower);
    }//TowerExpire()

    public bool SpawnTower(GameObject towerPrefab, Vector2 position)
    {
        // Spawn the tower
        RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up);

        try{
            //Need an IF that will check if there are any towers to be used based on the selected slot in the hotbar
            if (hit.collider.gameObject.CompareTag("TileMap")){
                GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity, this.transform); //Spawn as child to self
                towerList.Add(tower);
                SoundManager.instance.PlayGlobal(buildTower);
                return true;
            }
            else{
                return false;
            }
        }
        catch{
            return false;
        }
    }//SpawnTower(GameObject, Vector2)
}//TowerManager