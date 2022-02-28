using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] movePoints;
    private Vector2 currentDirection;
    private int nextTargetArray = 0;
    private int tempArray = 0;

    void Start()
    {
        if (movePoints.Length < 2)
        {
            Debug.Log("ERROR: Move Points need to have more than 1 transform.");
            return;
        }

        nextTargetArray = RandomizeNextPoint();
        currentDirection = ((Vector2)movePoints[nextTargetArray].position - (Vector2)this.transform.position).normalized;
    }
    
    void Update()
    {
        this.transform.Translate(currentDirection * movementSpeed * Time.deltaTime, Space.World);

        CheckPosition(movePoints[nextTargetArray].position);
    }

    void CheckPosition(Vector2 targetPosition)
    {
        if (Vector2.Distance(targetPosition, this.transform.position) < movementSpeed * Time.deltaTime)
        {
            nextTargetArray = RandomizeNextPoint();
            currentDirection = ((Vector2)movePoints[nextTargetArray].position - (Vector2)this.transform.position).normalized;
        }
    }

    int RandomizeNextPoint()
    {
        tempArray = Random.Range(0, movePoints.Length);

        if (tempArray == nextTargetArray)
        {
            while (tempArray == nextTargetArray)
            {
                tempArray = Random.Range(0, movePoints.Length);
            }

            // Return the value after it's confirmed the array is different.
            Debug.Log("NextArray: " + tempArray + " - PrevArray: " + nextTargetArray);
            return tempArray;
        }
        else
        {
            // Immediately return because the array is different.
            Debug.Log("NextArray: " + tempArray + " - PrevArray: " + nextTargetArray);
            return tempArray;
        }
    }

    public void ChangeMovespeed(float speedChange)
    {
        movementSpeed += speedChange;
    }

    public void ChangeMovePoints(float xChange, float yChange, float zChange)
    {
        foreach(Transform i in movePoints)
        {
            // Change all together.
            i.position.Set(i.position.x + xChange, i.position.y + yChange, i.position.z + zChange);
        }
    }
}
