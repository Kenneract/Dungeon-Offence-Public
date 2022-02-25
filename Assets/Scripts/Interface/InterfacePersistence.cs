using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfacePersistence : MonoBehaviour
{
    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static InterfacePersistence instance;
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
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
