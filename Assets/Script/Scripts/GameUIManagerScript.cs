using TMPro;
using UnityEngine;

public class GameUIManagerScript : MonoBehaviour
{
    public TextMeshProUGUI playerHealthUI;
    public TextMeshProUGUI playerEnergyUI;
    public TextMeshProUGUI playerHungerUI;
    public TextMeshProUGUI playerPointsUI;
    public TextMeshProUGUI playerboxleftui;
    public TextMeshProUGUI tuffoodleftui;
    public TextMeshProUGUI[] gameEndPointsUI;

    public int boxleft = 5;

    [Header("First scene")]
    public bool Firstscene;

    public GameObject pauseMenuUI;
    public GameObject gameOverPanel;
    public GameObject WinPanel;


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
        if (Firstscene == true)
        {
            playerboxleftui.text = "BOX LEFT : " + boxleft;
        }
        playerPointsUI.text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;


        if (boxleft == 0)
        {
            playerboxleftui.enabled = false;
        }
    }

    public void TogglePauseMenuUI(bool toggle)
    {
        pauseMenuUI.SetActive(toggle);
    }

    public void DisplayGameEndUI()
    {
        gameEndPointsUI[0].text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;
        gameEndPointsUI[1].text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;
        gameOverPanel.SetActive(true);
    }
    public void DisplayWinUI()
    {
        gameEndPointsUI[0].text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;
        gameEndPointsUI[1].text = "Points: " + (int)PlayerManagerScript.Instance.playerTotalPoints;
        WinPanel.SetActive(true);
    }
}
