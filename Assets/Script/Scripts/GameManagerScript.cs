using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemDrop
{
    ITEM_BOMB = 0,
    ITEM_ENERGY = 1,
    ITEM_FOOD = 2,
    ITEM_HEAL = 3,
    ITEM_POINTS = 4,

    OBJECT_BUTTON = 101,

    TOTAL = 999
};

public class GameManagerScript : MonoBehaviour
{
    private CameraControlScript camControlScript;
    private bool gamePaused = false;

    public string gameLevelName;

    public int playerGameEndPoints = 0;
    public int gameEndCondition = 0;

    public int buttonPoints = 5;

    [HideInInspector] public bool tufRevived = false;

    public static GameManagerScript Instance;

    public GameObject tutorialBubble;
    public GameObject[] tutorialBubbles; //delete the above one if we're going with multiple 

    private GameObject[] tutorialCheck;



    void Start()
    {
        // Simple Singleton.
        Instance = this;

        // Sets the Script accordingly.
        camControlScript = this.GetComponent<CameraControlScript>();
    }

    /* int exceptionObj = 2; //Position of object in the array
        for (int i = 0; i < myObjs.Length; i++)
        {
            if (i != exceptionObj) // If not the exception ID
                myObjs[i].SetActive(false);
        }
     */



    void Update()
    {
        checkIfTutorialNeeded();

        // Checks if the player has met the game end requirements.
        CheckGameEnd();

    }


    /*If the tutorial bubble is active, pressing T unpauses the game and closes it. If it's not active, it opens the tutorial
     bubble and pauses*/
    void checkIfTutorialNeeded()
    {
        int i;
        int which = 3;






        //tutorialCheck= GameObject.FindGameObjectWithTag(TagManager.TUTORIAL_TAG);

        //tutorialCheck = GameObject.FindGameObjectsWithTag(TagManager.TUTORIAL_TAG);

        if (Input.GetKeyDown(KeyCode.T) && !tutorialCheck[].activeInHierarchy) // OPEN if not open
        {


            Time.timeScale = 0;
            print("tutorial opened!");
        }

        if (tutorialCheck[].activeInHierarchy && Input.GetKeyDown(KeyCode.T)) //CLOSE the tutorial if open
        { 

            Time.timeScale = 1;
            //tutorialBubbles[i].SetActive(false);
            print("tutorial CLOSED!");



            for (i = 0; i < tutorialBubbles.Length; i++)
            {
                // only the one matching i == which will be on, all others will be off
                tutorialBubbles[i].SetActive(i == which);
            }



            /*for (int l=0; l<tutorialBubbles.Length; l++)
            {
                tutorialBubbles[l].SetActive(false);
                print("tutorial CLOSED for loop!");
            }/*



        }

        if (tutorialCheck.activeInHierarchy && Input.GetKeyDown(KeyCode.G))
        {
            tutorialBubbles[0].SetActive(false);

                for  (i = 1; i < tutorialBubbles.Length; i++)
                {
                    tutorialBubbles[i].SetActive(true);
                    i++;



                }
            
        }
       
        
        
        
        //OLD
        
        /*        for (int i = 0; i < tutorialBubbles.Length; i++)
                {*/

            /*if (Input.GetKeyDown(KeyCode.T) && tutorialBubbles[i].activeInHierarchy)
            {

                tutorialBubbles[i].SetActive(false);
                // set tutorial active to false
                Time.timeScale = 1;
            }

            else if (Input.GetKeyDown(KeyCode.T) && !tutorialBubbles[i].activeInHierarchy)
            {
                tutorialBubbles[i].SetActive(true);

                Time.timeScale = 0;

            }*/

            /*}*/
        }

    }



     public  void CheckGameEnd()
    {
        if (playerGameEndPoints == gameEndCondition)
        {
            // Game Ends
            // Do level end UI here.

            // Temp placement to end the game.
            ReturnToMainMenu();
        }
    }

     public void AddGameEndPoints(int value)
    {
        // Adds points that contribute to the game end points.
        playerGameEndPoints = playerGameEndPoints + value;

        // Checks if the points hit allows the player to progress upwards in the level.
        camControlScript.CheckReqForProgression(playerGameEndPoints);
    }

    public void PauseToggle()
    {
        if (gamePaused == false)
        {
            Debug.Log("Pause Game");
            gamePaused = true;
            GameUIManagerScript.Instance.TogglePauseMenuUI(true);
            Time.timeScale = 0;
        }
        else if (gamePaused == true)
        {
            Debug.Log("Resume Game");
            gamePaused = false;
            GameUIManagerScript.Instance.TogglePauseMenuUI(false);
            Time.timeScale = 1;
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(gameLevelName);
    }
}
