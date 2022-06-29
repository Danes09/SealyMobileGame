using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public Slider healthSlider;

    public int maxHealth;

    [SerializeField]private int currentHealth;

    void Start()
    {
        //temp//
        PlayerPrefs.SetInt("health", 0);

        SetDefs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetInt("health", 0);
            Debug.Log(PlayerPrefs.GetInt("health", 0));
        }
    }

    void SetDefs()
    {
        currentHealth = PlayerPrefs.GetInt("health", 0);
        currentHealth = 0;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        Debug.Log(PlayerPrefs.GetInt("health", 0));


    }

    public void buyHealth()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += 5;
            PlayerPrefs.SetInt("health", currentHealth);
            healthSlider.value = currentHealth;
            Debug.Log("Health Upgrade");
        }
        else
        {
            Debug.Log("Health full");
        }
    }
}
