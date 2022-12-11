using System.Collections;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    private BalloonCageScript thisCageManager;
    public Animator Redballoon;


    public void SetThisCageManager(BalloonCageScript cageManager)
    {
        // Set the manager for this balloon.
        thisCageManager = cageManager;
    }

    public void BalloonIsHit(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            thisCageManager.BalloonDestroyed();
            Redballoon.SetTrigger("BallonExplode");
            StartCoroutine(BalloonExplode());
        }
        //if (collision.gameObject.tag == "Player")//when player hit the balloon, balloon destroy
        //{
        //    thisCageManager.BalloonDestroyed();
        //    Redballoon.SetTrigger("BallonExplode");
        //    StartCoroutine(BalloonExplode());
        //}
    }
    IEnumerator BalloonExplode()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
    }
}
