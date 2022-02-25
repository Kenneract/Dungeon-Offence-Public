using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Manages wave spawning, grace periods, player health + lives, and dictating when to load the next level. Also keeps a record of all Menus so can freeze player.
*/

//TODO: Figure out some way of having multiple, customizable enemies in the spawnn pool

public class GameManager : MonoBehaviour
{

    [Header("Health")]
    public int playerHealth = 100;
    public int friendHealth = 100;
    public int lives = 2;
    [Space]
    [Header("Waves")]
    [Tooltip("How long (seconds) from the start of the level to the first wave")]
    public int startGraceTime = 15; //How long (seconds) from the start of the level to the first wave
    public int waveGraceTime = 15; //How long (seconds) between waves
    [Space]
    [Header("Sounds")]
    public AudioClip waveComplete;
    public AudioClip waveStart;
    public AudioClip levelCompleteSound;
    public AudioClip friendHurt;

    [HideInInspector] public int curWave = 0; //What wave we're currently on
    [HideInInspector] public bool waveActive = false;
    [HideInInspector] public float curGraceTime = 0f; //Should be private; how long until the next wave appears
    [HideInInspector] public bool levelComplete = false;

    private int menusOpen = 0; //Stores the number of windows/menues that are currently open. If at least one is open, several game objects will disable themselves
    private bool gameEnded = false;

    #region Callbacks
    public delegate void OnMenuOpened();
    public OnMenuOpened onMenuOpenedCallback; //For when AT LEAST ONE Menu is opened
    public delegate void OnMenusClosed();
    public OnMenusClosed onMenusClosedCallback; //For when EVERY Menu is closed
    public delegate void OnGameStart();
    public OnGameStart onGameStartCallback;
    #endregion

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static GameManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found.");
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

        //Sanity check that theres an exit for this level
        if (GameObject.Find("PlayerExit") == null)
        {
            Debug.LogWarning("GameManager has noticed PlayerExit might be missing from this level.");
        }//if

    }//Awake()

    public void MenuOpen()
    {//Manually called when some menu is opened.
        menusOpen++;
        if (menusOpen == 1 && onMenuOpenedCallback != null)
        {
            //this was the first menu
            onMenuOpenedCallback.Invoke();
        }//if 
    }//MenuOpen

    public void MenuClose()
    {//Manually called when a menu is closed.
        menusOpen--;
        menusOpen = Mathf.Max(0, menusOpen); //Set lower bound
        if (menusOpen == 0 && onMenusClosedCallback != null)
        {
            //Last menu has closed
            onMenusClosedCallback.Invoke();
        }//if
    }//MenuClose

    public bool AreMenusOpen()
    {//Returns true if any number of windows are open
        return (menusOpen > 0);
    }//AreMenusOpen

    // Start is called before the first frame update
    void Start()
    {
        curGraceTime = startGraceTime;
        HUDManager.instance.UpdateWaveTime();
        HUDManager.instance.UpdateHealthHUD();
    }//Start()

    // Update is called once per frame
    void Update()
    {
        if (!levelComplete) {
            if (curGraceTime > 0f) {
                curGraceTime -= Time.deltaTime;
                HUDManager.instance.UpdateWaveTime();
            }
            if (curGraceTime <= 0f && !waveActive) {
                StartWave();
                if (onGameStartCallback != null) onGameStartCallback.Invoke();
            }
        }//if

    }//Update()

    public void OnPlayerInExit()
    {//Event that is manually called when the player enters the exit zone
        if(levelComplete)
        {
            PlayerController.instance.frozen = true; //Freeze the player
            LevelManager.instance.TransistionToNextLevel(); //Black out screen and switch levels
        }//if
    }//OnPlayerInExit()

    public void ForceStart()
    {
        curGraceTime = 0;
    }//ForceStart()

    void StartWave()
    {
        waveActive = true;
        curWave++;
        EnemyManager.instance.SpawnWave(curWave);
        SoundManager.instance.PlayGlobal(waveStart);
        HUDManager.instance.UpdateWaveTime();
    }//StartWave()

    public void OnWaveOver()
    {//Called by the enemy manager
        waveActive = false;
        curGraceTime = waveGraceTime;
        if (curWave == EnemyManager.instance.GetNumWaves()) { 
            EndLevel();
        } else {
            SoundManager.instance.PlayGlobal(waveComplete);
            HUDManager.instance.WaveQueued();
        }
    }//OnWaveOver()

    void EndLevel() {
        levelComplete = true;
        HUDManager.instance.UpdateWaveTime();
        SoundManager.instance.PlayGlobal(levelCompleteSound);
    }//EndLevel()

    public void OnPlayerDamage(int damage)
    {// Manually called by Player Controller when taking damage
        playerHealth -= damage;
        CheckRevive();
        CheckGameOver();
        HUDManager.instance.UpdateHealthHUD();
    }//OnPlayerDamage()

    public void OnEnemyBreach(int damage)
    {// Manually called by EnemyGoal when an enemy reaches it
        friendHealth -= damage;
        CheckRevive();
        CheckGameOver();
        HUDManager.instance.UpdateHealthHUD();
        SoundManager.instance.PlayGlobal(friendHurt);
    }//onEnemyBreach()

    void CheckRevive()
    {//If needed, removes a life and restores player/friend health to max.
        if (lives > 0){
            if (playerHealth <= 0) {
                playerHealth = 100;
                lives--;
            } else if (friendHealth <= 0) {
                friendHealth = 100;
                lives--;
            }//end if
        }//if have lives
    }//CheckRevive()

    void CheckGameOver()
    {//Ends the game if player or friend are dead
        if ((playerHealth <= 0 || friendHealth <= 0) && !gameEnded) {
            Debug.Log("Game lost.");
            HUDManager.instance.ShowDeadWindow();
            gameEnded = true;
        }//if
    }//CheckGameOver()



    public void GameOver()
    {
        //Delete the inventory so no items carry over - thats only for when they beat the level
		Destroy(InventoryManager.instance.gameObject); //Destroy invenntory so it doesnt reload the items
		Destroy(ItemHolder.instance.gameObject); //Destroy item holder so it doesnt think we loaded into a new level and show the popup

        //DISPLAY SOME KIND OF GAMEOVER MESSAGE? WITH A "YOU DIED" OR "YOUR FRIEND DIED"

        //Reload scene
        LevelManager.instance.TransistionReload();
    }//GameOver()
}
