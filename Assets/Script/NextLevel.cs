using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D oth)
    {
        if (oth.CompareTag("Player"))
        {
            // Debug.Log("touch");
            GameUIManagerScript.Instance.DisplayWinUI();

        }
    }

}
