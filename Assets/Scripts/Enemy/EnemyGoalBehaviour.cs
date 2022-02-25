using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Author: Kennan
* Date: Feb.24.2021
*/

// Purpose: Placed on the enemy goal area - once they walk into it, they are deleted, and the game manager is notified something got through.

public class EnemyGoalBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private SoundManager soundManager;
    public AudioClip friendHurt;

    void Start()
    {// Start is called before the first frame update
        //Grab a reference to the GameManager, to report when an enemy gets through.
        try {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        } catch //(NullReferenceException e)
        {
            Debug.LogWarning("WARNING: EnemyGoalBehaviour could not find reference to GameManager. Ensure the GameManager exists in this scene.");
        }
        try {
            soundManager = SoundManager.instance;
        } catch {
            Debug.LogWarning("WARNING: EnemyGoalBehavior could not find reference to SoundManager. Ensure the SoundManager exists in this scene.");
        }
    }//Start()

    private void OnTriggerEnter2D(Collider2D other) 
    {//Removes colliding object, and tells the GameManager something got through
        if (other.gameObject.tag == "Enemy") {
            gameManager.OnEnemyBreach(other.GetComponent<Enemy>().friendDamage); //Tell the game manager something got through
            Destroy(other.gameObject); //Destroy the enemy (not kill; theyre off screen so doesnt matter)
        }
    }//OnTriggerEnter2D()

}//EnemyGoalBehaviour
