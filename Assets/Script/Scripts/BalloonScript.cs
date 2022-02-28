using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    private BalloonCageScript thisCageManager;
    
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
            this.gameObject.SetActive(false);
        }
    }
}
