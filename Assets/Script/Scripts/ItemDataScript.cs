using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataScript : MonoBehaviour
{
    public ItemDrop itemType;

    void Start()
    {
        if (itemType != ItemDrop.OBJECT_BUTTON)
            Destroy(this.gameObject, 10.0f);
    }

    public void CheckItemData()
    {
        if (itemType == ItemDrop.ITEM_BOMB)
        {
            // This item is ITEM_BOMB
            Debug.Log("Update: Interacted with Bomb");
            ItemInteract_Bomb();
        }
        else if (itemType == ItemDrop.ITEM_ENERGY)
        {
            // This item is ITEM_ENERGY
            Debug.Log("Update: Interacted with Energy");
            ItemInteract_Energy();
        }
        else if (itemType == ItemDrop.ITEM_FOOD)
        {
            // This item is ITEM_FOOD
            Debug.Log("Update: Interacted with Food");
            ItemInteract_Food();
        }
        else if (itemType == ItemDrop.ITEM_HEAL)
        {
            // This item is ITEM_HEAL
            Debug.Log("Update: Interacted with Heal");
            ItemInteract_Heal();
        }
        else if (itemType == ItemDrop.ITEM_POINTS)
        {
            // This item is ITEM_POINTS
            Debug.Log("Update: Interacted with Points");
            ItemInteract_Points();
        }
        else if (itemType == ItemDrop.OBJECT_BUTTON)
        {
            // This item is OBJECT_BUTTON
            Debug.Log("Update: Interacted with Button");
            ObjectInteract_Button();
        }
    }

    void ObjectInteract_Button()
    {
        // #Critical: Button gives 5 Game End points.
        GameManagerScript.Instance.AddGameEndPoints(GameManagerScript.Instance.buttonPoints);
        Destroy(this.gameObject);
    }

    void ItemInteract_Bomb()
    {
        PlayerManagerScript.Instance.TakeDamageFromBomb();
        Destroy(this.gameObject);
    }

    void ItemInteract_Energy()
    {
        PlayerManagerScript.Instance.GainEnergyFromItem();
        Destroy(this.gameObject);
    }

    void ItemInteract_Food()
    {
        PlayerManagerScript.Instance.GainHungerFromFood();
        Destroy(this.gameObject);
    }

    void ItemInteract_Heal()
    {
        PlayerManagerScript.Instance.HealFromItem();
        Destroy(this.gameObject);
    }

    void ItemInteract_Points()
    {
        PlayerManagerScript.Instance.GainPointsFromItem();
        Destroy(this.gameObject);
    }
}
