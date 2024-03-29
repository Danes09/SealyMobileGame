﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemDrop
{
    ITEM_BOMB = 0,
    ITEM_ENERGY = 1,
    ITEM_FOOD = 2,
    ITEM_HEAL = 3,
    ITEM_POINTS = 4,

    OBJECT_BUTTON = 101,

    TOTAL = 999
};

public class GameManagerScript : MonoBehaviour
{
    private CameraControlScript camControlScript;
    private bool gamePaused = false;

    public string gameLevelName;

    public int playerGameEndPoints = 0;
    public int gameEndCondition = 0;

    public int buttonPoints = 5;

    [HideInInspector] public bool tufRevived = false;

    public static GameManagerScript Instance;
    void Start()
    {
        // Simple Singleton.
        Instance = this;

        // Sets the Script accordingly.
        camControlScript = this.GetComponent<CameraControlScript>();
    }
    
    void Update()
    {
        // Checks if the player has met the game end requirements.
        CheckGameEnd();
    }

    void CheckGameEnd()
    {
        if (playerGameEndPoints == gameEndCondition)
        {
            // Game Ends
            // Do level end UI here.

            // Temp placement to end the game.
            ReturnToMainMenu();
        }
    }

    public void AddGameEndPoints(int value)
    {
        // Adds points that contribute to the game end points.
        playerGameEndPoints = playerGameEndPoints + value;

        // Checks if the points hit allows the player to progress upwards in the level.
        camControlScript.CheckReqForProgression(playerGameEndPoints);
    }

    public void PauseToggle()
    {
        if (gamePaused == false)
        {
            Debug.Log("Pause Game");
            gamePaused = true;
            GameUIManagerScript.Instance.TogglePauseMenuUI(true);
            Time.timeScale = 0;
        }
        else if (gamePaused == true)
        {
            Debug.Log("Resume Game");
            gamePaused = false;
            GameUIManagerScript.Instance.TogglePauseMenuUI(false);
            Time.timeScale = 1;
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(gameLevelName);
    }
}
