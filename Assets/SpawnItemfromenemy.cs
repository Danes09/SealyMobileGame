using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemfromenemy : MonoBehaviour
{
    [SerializeField] private GameObject bombObject;
    [SerializeField] private GameObject energyObject;
    [SerializeField] private GameObject foodObject;
    [SerializeField] private GameObject healObject;
    [SerializeField] private GameObject pointsObject;

    private Vector2 itemScale = new Vector2(0.5f, 0.5f);
    private int gameEndValue = 0;
    private float bombChance = 1.5f;
    private float energyChance = 3.0f;
    private float foodChance = 4.5f;
    private float healChance = 6.0f;
    private float pointsChance = 7.5f;

    private float tempNumber;
    //void OnTriggerEnter(Collider other)
    //{

    //}
    public void SetStats(Vector2 tItemScale, float tBombChance, float tEnergyChance, float tFoodChance, float tHealChance, float tPointsChance, int tEndValue)
    {
        itemScale = tItemScale;
        gameEndValue = tEndValue;
        bombChance = tBombChance;
        energyChance = tEnergyChance;
        foodChance = tFoodChance;
        healChance = tHealChance;
        pointsChance = tPointsChance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(collision);
    }
    public void TakeDamage(Collision2D collision)
    {
        BouncingBall ball = collision.gameObject.GetComponent<BouncingBall>();
        if (ball)
        {
            SpawnItem();
            return;
        }
    }
    void SpawnItem()
    {
        if (gameEndValue != 0)
            GameManagerScript.Instance.AddGameEndPoints(gameEndValue);

        GameObject item = null;
        tempNumber = Random.Range(0.0f, 10.0f);

        if (tempNumber <= bombChance)
        {
            // Spawn Bomb
            item = Instantiate(bombObject, this.transform.position, Quaternion.identity);
        }
        else if (tempNumber <= energyChance && tempNumber > bombChance)
        {
            // Spawn Energy
            item = Instantiate(energyObject, this.transform.position, Quaternion.identity);
        }
        else if (tempNumber <= foodChance && tempNumber > energyChance)
        {
            // Spawn Food
            item = Instantiate(foodObject, this.transform.position, Quaternion.identity);
        }
        else if (tempNumber <= healChance && tempNumber > foodChance)
        {
            // Spawn Heal
            item = Instantiate(healObject, this.transform.position, Quaternion.identity);
        }
        else if (tempNumber <= pointsChance && tempNumber > healChance)
        {
            // Spawn Points
            item = Instantiate(pointsObject, this.transform.position, Quaternion.identity);
        }
        else
        {
            // Spawn Nothing
            Debug.Log("UPDATE: Crate Dropped Nothing");
        }

        if (item != null)
        {
            // Adjust the size of the item that dropped from the crate.
            item.transform.localScale = new Vector3(item.transform.localScale.x * itemScale.x, item.transform.localScale.y * itemScale.y, 1.0f);
        }

        // Only destroy after spawning item.
        PlayerManagerScript.Instance.GainPointsFromCrates();
    }
}
