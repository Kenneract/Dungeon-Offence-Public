using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
Author: Joshua
Date: Apr.10
Purpose: Handles In-Game level loading.
*/

//TODO: Somehow many the screen fadein/fadeout an animation or something. As long as its not dependent on Time.deltaTime


public class LevelManager : MonoBehaviour
{
    public int level;
    private GameObject levelPrefab;
    public Image fadeoutImage;
    public float fadeTime = 1f;
    
    private float timeRemain = 0.00f;
    private bool animRunning = false;
    private bool toCover = false;
    private string levelToLoad = "";
    [HideInInspector] public bool screenReady;

    #region Callbacks
    public delegate void OnScreenReady();
    public OnScreenReady onScreenReadyCallback; //For when the screen is completely clear of black stuff
    #endregion

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static LevelManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("Another LevelManager still exists; deleting self.");
            Destroy(this.gameObject);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion


    void Update()
    {   
        if (animRunning)
        {
            //Countdown if animation is running
            timeRemain -= Time.deltaTime;

            //Check if time is up
            if (timeRemain <= 0.00f)
            {//Animation has just stopped
                animRunning = false;
                if (!toCover) {
                    fadeoutImage.enabled = false; //disable the image if we just removed it
                    if (onScreenReadyCallback != null) onScreenReadyCallback.Invoke(); //Run the callback
                    screenReady = true;
                }//if
                if (levelToLoad != "") SceneManager.LoadScene(levelToLoad);

            } else {
                //Animation is still running
                float prog = timeRemain/fadeTime;
                if (toCover)
                {
                    fadeoutImage.color = new Color(0.0f,0.0f,0.0f, 1.0f-prog);
                } else {
                    fadeoutImage.color = new Color(0.0f,0.0f,0.0f, prog );
                }//if
            }//if
        }//if
    }//Update()


    void Awake()
    {
        SetSingleton();
        //find level prefabs
    }//Awake()

    void Start()
    {
        FadeUncover();
    }//Start()

    void FadeCover()
    {
        fadeoutImage.enabled = true;
        timeRemain = fadeTime;
        animRunning = true;
        toCover = true;
    }//CoverFade()

    void FadeUncover()
    {
        fadeoutImage.enabled = true; //Make sure its enabled
        timeRemain = fadeTime;
        animRunning = true;
        toCover = false;
        screenReady = false;     
    }//RevealFade()

    public void TransistionToNextLevel()
    {//Fades the screen to black, then loads the next level
        FadeCover();
        level++;
        levelToLoad = "Level" + level.ToString();
    }//TransistionToNextLevel()

    public void TransistionReload()
    {
        FadeCover();
        levelToLoad = SceneManager.GetActiveScene().name;
    }//TransistionReload()

    public void TransitionMainMenu()
    {
        FadeCover();
        levelToLoad = "MainMenu";
        //Destroy any persistent objects that dont belong on the menu
        Destroy(TowerMenuManager.instance.gameObject);
        Destroy(InventoryManager.instance.gameObject);
		Destroy(ItemHolder.instance.gameObject);


    }//TransitionMainMenu()

    public void TransistionToLevel(string lvlName)
    {
        FadeCover();
        levelToLoad = lvlName;
    }//TransistionToLevel()


    public void loadLevel(int newLevel)
    {
        Debug.LogWarning("Usage of loadLevel is discouraged. Use TransistionToLevel instead.");
        level = newLevel;
        SceneManager.LoadScene("Level" + level.ToString());
        if (levelPrefab != null) {
            Destroy(levelPrefab);
        }
        //levelPrefab = Instantiate(GameObject.Find("LevelStart"));
    }
    public void nextLevel()
    {
        Debug.LogWarning("Usage of nextLevel is discouraged. Use TransistionToNextLevel instead.");
        level++;
        SceneManager.LoadScene("Level" + level.ToString());
        if (levelPrefab != null) {
            Destroy(levelPrefab);
        }
        //levelPrefab = Instantiate(GameObject.Find("LevelStart"));
    }
    public void ReloadLevel()
    {
        Debug.LogWarning("Usage of ReloadLevel is discouraged. Use TransistionReload instead.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }//ReloadLevel()

}//LevelManager
