using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TufCharacterScript : MonoBehaviour
{
    public float hungerBarValue;
    public float currHungerValue;
    
    void Start()
    {
        if (hungerBarValue <= 0)
            Debug.Log("ERROR: NEED TO SET TUF HUNGER VALUE.");

        currHungerValue = hungerBarValue;
        InvokeRepeating("TufSpeak", 0.0f, 6.0f);
        InvokeRepeating("TufShutUp", 2.0f, 6.0f);
    }
    
    void Update()
    {
        
    }

   public void DecreaseHunger()
    {
        // Decrease Tuf Hunger when consumed food.
        currHungerValue--;

        // Check if Hunger has reached 0.
        if (currHungerValue <= 0)
        {
            GameManagerScript.Instance.tufRevived = true;
            this.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            if (collision.gameObject.GetComponent<ItemDataScript>().itemType == ItemDrop.ITEM_FOOD)
            {
                Debug.Log("UPDATE: TUF INTERACTED WITH FOOD.");
                Destroy(collision.gameObject);
                DecreaseHunger();
            }
        }
    }

    void TufSpeak()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    void TufShutUp()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    /*
    public void InteractWithFood(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            if (collision.gameObject.GetComponent<ItemDataScript>().itemType == ItemDrop.ITEM_FOOD)
            {
                Debug.Log("UPDATE: TUF INTERACTED WITH FOOD.");
                DecreaseHunger();
            }

            Destroy(collision.gameObject);
        }
    }
    */
}
