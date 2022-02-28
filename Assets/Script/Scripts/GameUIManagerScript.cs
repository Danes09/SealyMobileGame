using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManagerScript : MonoBehaviour
{
    public Text playerHealthUI;
    public Text playerEnergyUI;
    public Text playerHungerUI;
    public Text playerPointsUI;
    public Text gameEndPointsUI;

    public GameObject pauseMenuUI;
    public GameObject gameOverPanel;
    
    public static GameUIManagerScript Instance;
    void Start()
    {
        // Simple singleton
        Instance = this;
    }
    
    void Update()
    {
        UpdatePlayerStatsUI();
    }

    void UpdatePlayerStatsUI()
    {
        playerHealthUI.text = "HP: \n" + (int)PlayerManagerScript.Instance.playerHealth;
        playerEnergyUI.text = "E: \n" + (int)PlayerManagerScript.Instance.playerEnergy;
        playerHungerUI.text = "HGR: \n" + (int)PlayerManagerScript.Instance.playerHunger;
        playerPointsUI.text = "Points: \n" + (int)PlayerManagerScript.Instance.playerTotalPoints;
    }

    public void TogglePauseMenuUI(bool toggle)
    {
        pauseMenuUI.SetActive(toggle);
    }

    public void DisplayGameEndUI()
    {
        gameEndPointsUI.text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;
        gameOverPanel.SetActive(true);
    }
}
