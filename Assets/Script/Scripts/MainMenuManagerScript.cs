using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject PlayButton;
    public GameObject SettingPanel;
    public void PlayGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Play()
    {
        MenuPanel.SetActive(false);
        PlayButton.SetActive(true);
    }
    public void BackPlay()
    {
        MenuPanel.SetActive(true);
        PlayButton.SetActive(false);
    }
    public void SettingButton()
    {
        SettingPanel.SetActive(true);
    }
    public void BackSettingButton()
    {
        SettingPanel.SetActive(false);
    }
}
