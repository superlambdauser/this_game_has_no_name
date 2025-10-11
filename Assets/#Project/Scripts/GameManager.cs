using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        // Singleton pattern
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }


        #region Unity Methods
    void Awake()
    {
        if (instance == null) // Is there already an instance of gameManager ?
        {
            DontDestroyOnLoad(gameObject); // Keeping the same object between scenes
            instance = this; // Instantiate the gameManager as one and only gameManager
        }
        else if (instance != this) // If so, destroy unnecessary gameManager
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
        #endregion

    
        #region Custom Methods
        #endregion
}