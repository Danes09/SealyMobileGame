using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void PlayGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
