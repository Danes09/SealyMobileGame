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
    //Fill Water
    public GameObject Pipe;
    public ZoneTransitionManager zone;
    public float waterFillYOffset = 2f;
    public float waterFillTime = 0.5f;
    [SerializeField] public bool isFill = false;
    public GameObject JumpCooldown;
    public GameObject Walldectect;

    private CameraControlScript camControlScript;
    private bool gamePaused = false;

    public string gameLevelName;

    public int playerGameEndPoints = 0;
    public int gameEndCondition = 0;

    public int buttonPoints = 5;

    [HideInInspector] public bool tufRevived = false;

    public static GameManagerScript Instance;

    
    public GameObject[] tutorialBubbles; //delete the above one if we're going with multiple 

    private GameObject[] tutorialCheck;
    int exceptArrayId;

    private float timeElapsed;


    private int currentSceneIndex;


    void Start()
    {
        
        // Simple Singleton.
        Instance = this;

        // Sets the Script accordingly.
        camControlScript = this.GetComponent<CameraControlScript>();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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

        updateTimeElapsed();

        

        // Checks if the player has met the game end requirements.
        CheckGameEnd();

        increaseGameSpeedForInfiniteLevel();
    }

    private void FixedUpdate()
    {
        if (isFill == true)
        {
            Pipe.SetActive(true);
            StartCoroutine(FillWaterTime());
            //StartCoroutine(FillWaterInEnd());
        } 

    }

    void updateTimeElapsed()
    {
        timeElapsed += Time.deltaTime; //done to findout how much time has passed. This shall be used to for increasing game speed accordingly.. in the infinite levels

        //print("time elapsed:" + timeElapsed);

    }

    public void increaseGameSpeedForInfiniteLevel()
    {
        //timeElapsed=


/* guide for multiplier of speed? 
            if (timer > 1)
        {

            score += (1 * scoreMultiplier);
            //score += 1;
            scoreText.text = "" + score;
            timer = 0;

        }
*/

        if (currentSceneIndex % 8 == 0) //if the scene index is a multiple of 8
        {
            // for every 10 seconds, increase game speed by 0.10
            {
                Time.timeScale += 0.10f;
            }

        }

    }

    /*void move()
    {
        if (transform.position.y >= 21)
        {
            moveDirection = Vector3.down;
        }
        else if (transform.position.y <= -0.01)
        {
            moveDirection = Vector3.up;
        }
        // implicit else... if it's in between, it should keep moving in the same direction it last was...

        transform.Translate(moveDirection * Time.deltaTime * speed1);
    }*/

    void FillWater()
    {
        if (zone.waterSurface.transform.position.y <= 17)
        {
            zone.waterBody.transform.Translate(Vector3.up * (waterFillYOffset / waterFillTime) * Time.deltaTime);
            zone.waterSurface.transform.Translate(Vector3.up * (waterFillYOffset / waterFillTime) * Time.deltaTime);
        }
        else if (zone.waterSurface.transform.position.y >= 17)
        {
            isFill = false;
        }
        JumpCooldown.SetActive(false);
        Walldectect.SetActive(true);
    }

    /*IEnumerator FillWaterInEnd()
    {
        yield return new WaitForSeconds(4);

        isFill = false;
    }*/

    IEnumerator FillWaterTime()
    {
        yield return new WaitForSeconds(2);
        FillWater();
    }



/*If all the tutorial bubbles are closed, open the first one*/
   public void checkIfTutorialNeeded()
    {

        if (Input.GetKey(KeyCode.T) && 
            !tutorialBubbles[0].activeInHierarchy &&
            !tutorialBubbles[1].activeInHierarchy &&
            !tutorialBubbles[2].activeInHierarchy &&
            !tutorialBubbles[3].activeInHierarchy)
        {

            openTutorial0();

        }   
        /*If even one tutorial bubble is open, close all of them when Y is pressed*/
        
        else if(Input.GetKey(KeyCode.Y) && 
            (tutorialBubbles[0].activeInHierarchy||
            tutorialBubbles[1].activeInHierarchy||
            tutorialBubbles[2].activeInHierarchy||
            tutorialBubbles[3].activeInHierarchy) )
        {
            closeAllTutorials();
        }


    }

    public void openTutorial0()
    {
        /*TEMPORARY DISABLE*/
        //Time.timeScale = 0; 

        tutorialBubbles[0].SetActive(true);
        tutorialBubbles[1].SetActive(false);
        tutorialBubbles[2].SetActive(false);
        tutorialBubbles[3].SetActive(false);
    }

    public void openTutorial1()
    {
        /*TEMPORARY DISABLE*/
        //Time.timeScale = 0; 

        tutorialBubbles[0].SetActive(false);
        tutorialBubbles[1].SetActive(true);
        tutorialBubbles[2].SetActive(false);
        tutorialBubbles[3].SetActive(false);
    }



    public void openTutorial2()
    {
        /*TEMPORARY DISABLE*/
        //Time.timeScale = 0; 
        

        tutorialBubbles[0].SetActive(false);
        tutorialBubbles[1].SetActive(false);
        tutorialBubbles[2].SetActive(true);
        tutorialBubbles[3].SetActive(false);
    }


    public void openTutorial3()
    {
        /*TEMPORARY DISABLE*/
        //Time.timeScale = 0; 

        tutorialBubbles[0].SetActive(false);
        tutorialBubbles[1].SetActive(false);
        tutorialBubbles[2].SetActive(false);
        tutorialBubbles[3].SetActive(true);

    }

    /* Use a button to close all tutorials*/
    public void closeAllTutorials()
    {
        tutorialBubbles[0].SetActive(false);
        tutorialBubbles[1].SetActive(false);
        tutorialBubbles[2].SetActive(false);
        tutorialBubbles[3].SetActive(false);

        Time.timeScale = 1;
    }



    //tutorialCheck= GameObject.FindGameObjectWithTag(TagManager.TUTORIAL_TAG);

    //tutorialCheck = GameObject.FindGameObjectsWithTag(TagManager.TUTORIAL_TAG);
    /* CLOSED TO TRY OUT STACKOVERFLOW code
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
        } CLOSED TO TRY OUT CODE GIVEN*/



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






    public  void CheckGameEnd()
    {
        if (playerGameEndPoints == gameEndCondition)
        {
            // Game Ends
            // Do level end UI here.

            // Temp placement to end the game.
            //ReturnToMainMenu();
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
