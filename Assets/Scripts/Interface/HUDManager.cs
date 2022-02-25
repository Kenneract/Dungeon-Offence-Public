/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Updates the health, (someday map??), and wave time visual elements on screen.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// TODO: CONSIDER making this object automatically fill all its fields, as they are all child objects of this GameObject.

public class HUDManager : MonoBehaviour
{
    
    public Image playerHealthBar;
    public Image friendHealthBar;
    public Text livesText;
    public Text waveTimeText;
    public Texture2D cursorTexture;
    public GameObject forceButton;
    public GameObject deadWindow;
    public Text deathText;

    private PlayerController player; //TODO: THIS IS TEMPORARY UNTIL REAL INVENTORY IS ADDED
    private GameManager gameManager; //For reading health/lives/round time data

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static HUDManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of HUDManager found.");
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
    void Start()
    {// Start is called before the first frame update
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        //TEMPORARY
        try {
            player = PlayerController.instance;
        } catch {
            Debug.LogWarning("WARNING: HUDManager could not find reference to PlayerController. Ensure the Player exists in this scene.");
        }

    }//Start()

    public void OnForceWave()
    {//Manually called when the force wave button is clicked
        GameManager.instance.ForceStart();
        forceButton.SetActive(false);
    }//OnForceWave()

    public void WaveQueued()
    {//Manually called when the next wave gets queued.
        forceButton.SetActive(true);
    }//WaveQueued()

    public void UpdateWaveTime()
    {
        int curWave = GameManager.instance.curWave;
        if (!GameManager.instance.levelComplete) {
            if (!GameManager.instance.waveActive) {
                waveTimeText.text = "Wave " + (curWave+1).ToString() + " in " + ((int)GameManager.instance.curGraceTime).ToString() + "s";
            } else {
                waveTimeText.text = "Wave " + curWave.ToString();
                forceButton.SetActive(false);
            }
        } else {
            //waveTimeText.text = ((int)GameManager.instance.curEndTime).ToString() + "s to next level";
            waveTimeText.text = "Level Complete! Go to the Exit.";
        }
    }//UpdateWaveTime()

    public void UpdateHealthHUD()
    {
        playerHealthBar.fillAmount = GameManager.instance.playerHealth/100.0f;
        friendHealthBar.fillAmount = GameManager.instance.friendHealth/100.0f;
        livesText.text = GameManager.instance.lives.ToString();
    }//UpdateHealthHUD()

    public void ShowDeadWindow()
    {
        deadWindow.SetActive(true);
        GameManager.instance.MenuOpen(); //Stop any other menu from opening
        if (GameManager.instance.playerHealth <= 0)
        {
            deathText.text = "You died\n(you got hit too many times)";
        } else {
            deathText.text = "Your friend has died\n(too many enemies got through)";
        }
        Time.timeScale = 0.0f;
    }//ShowDeadWindow()

    public void OnReloadClick()
    {
        //deadWindow.SetActive(false);
        Time.timeScale = 1.0f; //MUST DO THIS SO FADEOUT WORKS
        GameManager.instance.GameOver();
        deadWindow.SetActive(false);
    }//OnReloadClick()

    public void OnMainMenuClick()
    {
        //deadWindow.SetActive(false);
        Time.timeScale = 1.0f; //MUST DO THIS SO FADEOUT WORKS
        deadWindow.SetActive(false);
        LevelManager.instance.TransitionMainMenu();
    }//OnReloadClick()

}//HUDManager