using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlockerScript : MonoBehaviour
{
    public bool blockFood = false;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            if (blockFood)
            {
                if (collision.gameObject.GetComponent<ItemDataScript>().itemType == ItemDrop.ITEM_FOOD)
                {
                    Debug.Log("UPDATE: BLOCKER INTERACTED WITH FOOD.");
                    collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
        }
    }
    
    /*
    public void InteractWithItem(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            if (blockFood)
            {
                if (collision.gameObject.GetComponent<ItemDataScript>().itemType == ItemDrop.ITEM_FOOD)
                {
                    Debug.Log("UPDATE: BLOCKER INTERACTED WITH FOOD.");
                    collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                }
            }
        }
    }
    */
}
