using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerInfiniteLevel : MonoBehaviour
{
    public GameObject crate;

    public float randX;
    public Vector2 whereToSpawn;
    public float spawnRate = 2f;
    public float nextSpawn = 0f;

    public float leftMostPosition = -2.0f;
    public float rightMostPosition = 2.0f;

    private void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time+spawnRate;
            randX = Random.Range(leftMostPosition,rightMostPosition);
            whereToSpawn = new Vector2(randX, transform.position.y);
            Instantiate(crate, whereToSpawn, Quaternion.identity);

        }

    }





}
