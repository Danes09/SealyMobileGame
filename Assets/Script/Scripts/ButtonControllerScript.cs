using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerScript : MonoBehaviour
{
    public GameObject button;
    public int pointsToActvivateButton;
    public bool isActivated = false;

    public List<GameObject> objectsToDisable = new List<GameObject>();
    
    void LateUpdate()
    {
        if (!isActivated)
            CheckControlStatus();
    }

    void CheckControlStatus()
    {
        if (GameManagerScript.Instance.playerGameEndPoints >= pointsToActvivateButton)
        {
            if (objectsToDisable.Count > 0)
            {
                foreach (GameObject i in objectsToDisable)
                    i.SetActive(false);
            }

            isActivated = true;
            button.SetActive(true);
        }
    }
}
