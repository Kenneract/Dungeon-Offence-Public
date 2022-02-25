using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static MainMenuMusic instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MainMenuMusic found.");
            Destroy(this.gameObject);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetSingleton();
        DontDestroyOnLoad(this.gameObject);
    }
}
