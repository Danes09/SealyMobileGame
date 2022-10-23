using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCageScript : MonoBehaviour
{
    [Tooltip("Determines how much the cage lowers it's height after each balloon has popped. MUST be a NEGATIVE value for lowering the cage.")]
    public float lowerCagePerBalloon = 0.0f;

    [Tooltip("Determines how much the cage slows down after each balloon has popped. MUST be a NEGATIVE value for slowing down the cage.")]
    public float slowCagePerBalloon = 0.0f;
    
    public List<GameObject> balloonObjects = new List<GameObject>();
    public GameObject trappedCharacter;
    public GameObject cage;
    public Rigidbody2D thisRigidbody;
    public EnemyMovementScript thisMovementScript;
    public int balloonPointValue;

    public int numOfBalloon = 0;

    void Start()
    {
        numOfBalloon = balloonObjects.Count;
        
        foreach (GameObject i in balloonObjects)
        {
            // Set the manager for the balloon script.
            i.GetComponent<BalloonScript>().SetThisCageManager(this);
        }
    }

    void CheckBalloonsStatus()
    {
        if (numOfBalloon > 0)
            return;

        if (numOfBalloon <= 0)
        {
            // All Balloons have been popped and can now drop the cage.
            // Can destroy the cage object.
            StartCoroutine("DropCage");
        }
    }

    // Call this function when a balloon pops.
    public void BalloonDestroyed()
    {
        numOfBalloon--;
        thisMovementScript.ChangeMovespeed(slowCagePerBalloon);
        thisMovementScript.ChangeMovePoints(0, lowerCagePerBalloon, 0);
        GameManagerScript.Instance.AddGameEndPoints(balloonPointValue);

        // Check the balloon status when a balloon pops.
        CheckBalloonsStatus();
    }

    IEnumerator DropCage()
    {
        cage.SetActive(false);
        trappedCharacter.SetActive(true);

        this.GetComponent<SpriteRenderer>().enabled = false;
        thisMovementScript.enabled = false;
        thisRigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        thisRigidbody.gravityScale = 1;
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }
}
