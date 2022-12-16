using System.Collections;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{
    [Header("Player Stats")]
    public float playerHealth;
    public float playerEnergy;
    public float playerHunger;
    public float damageTakenFromBomb;
    public float energyGainedFromItem;
    public float hungerGainedFromFood;
    public float healthGainedFromItem;
    public float pointsGainedFromItem;

    [Header("Affect Player Stats")]
    public float damageTakenFromCrates;
    public float energyUsedToSwitchDirection;
    public float energyUsedPerHalfSecondSwim;
    public float energyUsedToBounceTheBall;
    public float energyConsumedWhenStunned;

    [Header("Affect Player Points")]
    public float pointsGainedFromCrates;

    public float playerTotalPoints = 0;
    private float storedHealth;
    private float storedEnergy;
    private bool playerIsMoving = false;

    public static PlayerManagerScript Instance;

    /*For Powerups*/
    public bool isInvulnerable = false;
    private float invulnerabilityTime = 5f;


    private float slowMotionTime = 5f;

    [Header("Scene Tuf only")]
    public bool Scenetuf;
    public TufCharacterScript TCS;



    void Start()
    {
        // Simple singleton;
        Instance = this;

        storedHealth = playerHealth;
        storedEnergy = playerEnergy;

        InvokeRepeating("DecreasePlayerEnergy", 0.5f, 0.5f);
    }

    void Update()
    {
        CapPlayerData();

        /*Temporarily set to key bind I. Can be purchased at the shop? 
         Maybe activated by key I after purchasing at the shop*/
        powerupByKeyboard();

    }

    /*FOR TESTING PURPOSES: Activate powerups by keyboard*/
    /*CAN ACTUALLY BE REMOVED NOW THAT BUTTONS ARE BEING ADDED*/
    void powerupByKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            enableInvulnerability();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            enableSlowMotion();
        }


    }
   


    /*create a separate function for fast motion*/
    public void enableSlowMotion()
    {
        Time.timeScale = 0.5f;
        print("slow motion is ON");
        StartCoroutine(turnOffSlowMotion());

    }


    IEnumerator turnOffSlowMotion()
    {
        yield return new WaitForSeconds(slowMotionTime);
        Time.timeScale = 1f;
        print("slow motion is OFF");

    }

    public void enableInvulnerability()
    {
        isInvulnerable = true;
        print("Invulnerability: " + isInvulnerable);

        StartCoroutine(turnOffInvulnerability());
    }

    IEnumerator turnOffInvulnerability()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        print("Invulnerability ended!");
        isInvulnerable = false;


    }


    void CapPlayerData()
    {
        if (playerHealth > storedHealth)
            playerHealth = storedHealth;
        else if (playerHealth < 0)
            playerHealth = 0;

        if (playerEnergy > storedEnergy)
            playerEnergy = storedEnergy;
        else if (playerEnergy < 0)
            playerEnergy = 0;
    }

    void DecreasePlayerEnergy()
    {
        // Only decrease player energy when player is moving.
        if (playerIsMoving == true)
            EditPlayerEnergy(false, energyUsedPerHalfSecondSwim);
    }

    void CheckPlayerHealth()
    {
        if (playerHealth <= 0)
        {
            Time.timeScale = 0;
            GameUIManagerScript.Instance.DisplayGameEndUI();
        }
    }

    #region Public Functions

    public void TakeDamageFromCrates()
    {
        if (!isInvulnerable)
        {
            EditPlayerHealth(false, damageTakenFromCrates);
        }

    }

    public void SwitchDirectionEnergy()
    {
        EditPlayerEnergy(false, energyUsedToSwitchDirection);
    }

    public void BounceBallEnergy()
    {
        EditPlayerEnergy(false, energyUsedToBounceTheBall);
    }

    public void StunnedEnergyLost()
    {
        if (!isInvulnerable)
        {
            EditPlayerEnergy(false, energyConsumedWhenStunned);
        }
    }

    public void GainPointsFromCrates()
    {
        EditPlayerPoints(true, pointsGainedFromCrates);
    }

    // _____________________________________________________________ // 

    public void TakeDamageFromBomb()
    {
        if (!isInvulnerable)
        {
            EditPlayerHealth(false, damageTakenFromBomb);
        }
    }

    public void GainEnergyFromItem()
    {
        EditPlayerEnergy(true, energyGainedFromItem);
    }

    public void GainHungerFromFood()
    {
        EditPlayerHunger(true, hungerGainedFromFood);
    }

    public void HealFromItem()
    {
        EditPlayerHealth(true, healthGainedFromItem);
    }

    public void GainPointsFromItem()
    {
        EditPlayerPoints(true, pointsGainedFromItem);
    }

    // _____________________________________________________________ // 

    public void TakeDamageFromSpear(float damage)
    {
        if (!isInvulnerable)
        {
            EditPlayerHealth(false, damage);
        }
    }

    public void GainPoints(float numberOfPoints)
    {
        EditPlayerPoints(true, numberOfPoints);
    }

    #endregion

    #region Public Controls

    public void TogglePlayerMoving(bool toggle)
    {
        playerIsMoving = toggle;
    }

    public void EditPlayerHealth(bool togglePlusMinus, float value)
    {
        if (togglePlusMinus == true)
            playerHealth = playerHealth + value;
        else if (togglePlusMinus == false)
        {
            playerHealth = playerHealth - value;
            CheckPlayerHealth();
        }
    }

    public void EditPlayerEnergy(bool togglePlusMinus, float value)
    {
        if (togglePlusMinus == true)
            playerEnergy = playerEnergy + value;
        else if (togglePlusMinus == false)
            playerEnergy = playerEnergy - value;
    }

    public void EditPlayerHunger(bool togglePlusMinus, float value)
    {
        if (togglePlusMinus == true)
        {
            playerHunger = playerHunger + value;
            if (Scenetuf == true)
            {
                TCS.DecreaseHunger();
            }
            else if (Scenetuf == true && TCS.currHungerValue <= 0)
            {
                Scenetuf = false;
            }

        }
        else if (togglePlusMinus == false)
            playerHunger = playerHunger - value;
    }

    public void EditPlayerPoints(bool togglePlusMinus, float value)
    {
        if (togglePlusMinus == true)
            playerTotalPoints = playerTotalPoints + value;
        else if (togglePlusMinus == false)
            playerTotalPoints = playerTotalPoints - value;
    }

    public void InteractWithItem(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {

            // Check what item did the player hit.
            Debug.Log("UPDATE: INTERACTED WITH ITEM.");
            collision.gameObject.GetComponent<ItemDataScript>().CheckItemData();

        }
    }

    #endregion
}
