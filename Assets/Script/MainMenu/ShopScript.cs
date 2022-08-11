using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public Slider healthSlider;

    public int maxHealth;

    [SerializeField]private int currentHealth;
    int cash;
    public TextMeshProUGUI cashText;

    public GameObject shopUI;


    void Start()
    {
        //temp//
        PlayerPrefs.SetInt("health", 0);

        SetDefs();
    }

   /* private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetInt("health", 0);
            Debug.Log(PlayerPrefs.GetInt("health", 0));
        }
    }*/

    public void OpenShop()
    {
        shopUI.SetActive(true);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }

    void SetDefs()
    {
        cash = 1000;
        cashText.text = cash + "$";
        currentHealth = PlayerPrefs.GetInt("health", 0);
        currentHealth = 0;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        Debug.Log(PlayerPrefs.GetInt("health", 0));


    }

    public void buyHealth(int price)
    {
        if(currentHealth < maxHealth)
        {
            if(cash >= price)
            {
                cash -= price;
                cashText.text = cash + "$";
                currentHealth += 5;
                PlayerPrefs.SetInt("health", currentHealth);
                healthSlider.value = currentHealth;
                Debug.Log("Health Upgrade");
            }
            else
            {
                Debug.Log("out of cash");
            }
        }
        else
        {
            Debug.Log("Health full");
        }
    }
}
