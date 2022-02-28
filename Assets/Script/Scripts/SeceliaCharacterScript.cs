using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeceliaCharacterScript : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("SeceliaSpeak", 0.0f, 6.0f);
        InvokeRepeating("SeceliaShutUp", 2.0f, 6.0f);
    }
    
    void Update()
    {
        
    }

    void SeceliaSpeak()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    void SeceliaShutUp()
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
