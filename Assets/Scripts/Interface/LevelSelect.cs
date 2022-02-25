using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // TODO: gray out levels player hasn't unlocked yet
    }

    public void LevelClicked(int level) {
        //TODO: check if player has unlocked level
        if (level == 0)
        {
            //TUTORIAL LEVEL (We could also just call this Level0 and keep the below code for everything)
        } else {
            SceneManager.LoadScene("Level" + level.ToString());
        }
        Destroy(GameObject.Find("MainMenuMusic"));
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
