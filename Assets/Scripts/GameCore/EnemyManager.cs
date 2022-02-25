using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
* Author: Kennan (& Joshua)
* Date: Apr.8.2021
* Summary: Core object. Allowing configuring waves in inspector, spawns given waves at a spawnpoint, reports when all enemies are dead, and stores a reference to all enemies in the level. (All enemies reference this object.)
*/

[Serializable]
public struct EnemySpawn {
    public GameObject prefab;
    public int quantity;
}//EnemySpawn

[Serializable]
public class WaveData {
    public EnemySpawn[] enemySpawns;
}//WaveData

public class EnemyManager : MonoBehaviour
{
    [Tooltip("Time between individual enemy spawns")]
    public float spawnTime = 0.25f;
    [Space]
    [Tooltip("Each element represents a wave. Within each element is a list of enemy spawns. They are spawned in the order they are defined")]
    public WaveData[] waves; //Stores Vector2's representing (Quantity : EnemyPrefab) pairs.

    private GameObject enemySpawnpoint; //The object at which the enemies should be spawned at. Only used for position; no scripts
    public static List<GameObject> enemyList = new List<GameObject>(); //TODO: HIDE THIS FROM THE INSPECTOR (or maybe not?), BUT KEEP IT PUBLIC. (Required for Kevin's tower aiming). ALSO MAKE IT NOT STATIC - SINGLETON MAKES THIS REDUNDANT
    private GameManager gameManager; //For reporting when all enemies are dead
    private bool isSpawning = false;

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static EnemyManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EnemyManager found.");
            //Destroy(this);
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

    private void AutofillRefs()
    {//Searches the scene for the GameManager, and grabs a reference to them
        //Grab a reference to the GameManager
        try {
            gameManager = GameManager.instance;
        } catch {
            Debug.LogWarning("WARNING: EnemyManager could not find reference to GameManager. Ensure the GameManager exists in this scene.");
        }//try-catch
        //Try and automatically find the spawnpoint
        try {
            enemySpawnpoint = GameObject.Find("EnemySpawnpoint");
        } catch {
            Debug.LogWarning("WARNING: EnemyManager could not find an EnemySpawnpoint. Ensure an EnemySpawnpoint exists in this scene.");
        }//try-catch
    }//AutofillRefs()

    void Start()
    {// Start is called before the first frame update
        AutofillRefs();
    }//Start()

    public int GetNumWaves()
    {//Returns the number of defines waves
        return waves.Length;
    }//GetNumWaves()

    void Update()
    {// Update is called once per frame
        
    }//Update()

    public void EnemyExpire(GameObject enemy)
    {//Manually called by any enemies (passing themselves) when they die.
        enemyList.Remove(enemy);
        CheckWaveEnd();
    }//EnemyExpire(GameObject)

    void CheckWaveEnd()
    {
        if (enemyList.Count == 0 && !isSpawning) gameManager.OnWaveOver(); //If no enemies left, note the wave is over
    }//CheckWaveEnd()

    public void SpawnWave(int waveNum) {
        StartCoroutine(_SpawnWave(waveNum));
    }
   public IEnumerator _SpawnWave(int waveNum)
    {//Spawns the given wave, which is defined in the [waves] field.
        // Get data for this wave.
        EnemySpawn[] data = waves[waveNum-1].enemySpawns;
        isSpawning = true;
        
        foreach (EnemySpawn enemyType in data)
        {// Run through every enemy type, and spawn the given number of them.
            Vector3 prefabSize = enemyType.prefab.GetComponent<SpriteRenderer>().bounds.size;
            for (int i=0; i < enemyType.quantity; i++)
            {//Loop for as many of this enemy as should be spawned (qnt = y)
                // Determine physical offset
                //Debug.Log("Enemy #" + enemyList.Count.ToString() + " Dist is " + dist);
                Vector2 spawnPos = enemySpawnpoint.transform.position;

                //Debug.Log("Spawning Enemy #" + enemyList.Count.ToString() + " at (" + spawnPos.x.ToString() + ", " + spawnPos.y.ToString() + ")");
                //yield return new WaitForSeconds(0.25f);
                // Spawn the enemy
                GameObject enemy = Instantiate(enemyType.prefab, spawnPos, Quaternion.identity, this.transform); //Spawn as child to self
                enemyList.Add(enemy);
                yield return new WaitForSeconds(spawnTime);
            }//for

        }//foreach
        isSpawning = false;
        CheckWaveEnd();
    }//SpawnWave(int)
}//EnemyManager




//Enemimes will need an OnDestroy() method which tells the global EnemyManager that it has died. (Kevin / Josh)