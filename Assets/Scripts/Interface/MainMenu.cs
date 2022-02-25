using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NewGameClicked() {
        SceneManager.LoadScene("Level1"); //or level 0? Maybe have two buttons, one for new game (w/ tutorial) and just new game (level1)
        Destroy(GameObject.Find("MainMenuMusic"));
    }

    public void LevelSelectClicked() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitClicked() {
        Application.Quit();
    }
}
