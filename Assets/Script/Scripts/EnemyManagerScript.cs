using System.Collections;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public GameObject mainObject;
    public float enemyHealth;
    public float enemyArmorHP;
    public int enemyValue;
    public float enemyPoints;
    public bool isMiniBoss = false;
    public bool isPolarBear = false;
    public bool respawnThisEnemy = false;
    public bool stopResOnTufFull = false;

    private bool invulnerable = false;
    private Vector2 originalPos;
    private Vector2 respawnedPos;
    private float oriEnemyHealth;
    private float oriEnemyArmorHP;


    public SpriteRenderer armorBar;
    public SpriteRenderer healthBar;

    //private float enemy

    [Header("Scene Tuf only")]
    public bool Scenetuf;
    public CapsuleCollider2D EnemyCollider;
    public TufCharacterScript TCS;
    private void Update()
    {
        if (Scenetuf == true)
        {
            if (TCS.currHungerValue <= 0)
            {
                EnemyCollider.enabled = true;
                Scenetuf = false;
            }
        }
        SetFill(enemyArmorHP / oriEnemyArmorHP);
        SetFills(enemyHealth / oriEnemyHealth);
        // armorBar.SetFill(1) = enemyArmorHP / oriEnemyArmorHP;
        // healthBar.fillAmount = enemyHealth / oriEnemyHealth;
    }
    public void SetFill(float amount)
    {
        armorBar.material.SetFloat("_Cutoff", amount);
    }

    public void SetFills(float amount)
    {
        healthBar.material.SetFloat("_Cutoff", amount);

    }

    void Start()
    {
        oriEnemyHealth = enemyHealth;
        oriEnemyArmorHP = enemyArmorHP;
        originalPos = this.gameObject.transform.position;
        respawnedPos = new Vector2(originalPos.x, originalPos.y + 10.0f);
    }

    void CheckDeathStatus()
    {
        if (enemyHealth > 0)
            return;

        if (invulnerable)
            return;

        if (enemyHealth <= 0)
        {
            if (!respawnThisEnemy)
            {
                GameManagerScript.Instance.AddGameEndPoints(enemyValue);
                PlayerManagerScript.Instance.GainPoints(enemyPoints);

                // Enemy Death Here
                // Can stall the death time for death animation
                Destroy(mainObject);
            }
            else
            {
                if (stopResOnTufFull)
                {
                    if (GameManagerScript.Instance.tufRevived)
                    {
                        GameManagerScript.Instance.AddGameEndPoints(enemyValue);
                        PlayerManagerScript.Instance.GainPoints(enemyPoints);

                        // Enemy Death Here
                        // Can stall the death time for death animation
                        Destroy(mainObject);
                    }
                    else
                    {
                        // Respawn & slide the character down.
                        Respawn();
                    }
                }
                else
                {
                    // Respawn & slide the character down.
                    Respawn();
                }
            }
        }
    }

    void Respawn()
    {
        invulnerable = true;
        enemyHealth = oriEnemyHealth;
        enemyArmorHP = oriEnemyArmorHP;
        this.gameObject.transform.position = respawnedPos;
        StartCoroutine(LerpPosition(originalPos, 2));
    }

    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(0.5f);
        invulnerable = false;
    }

    public void EnemyIsHit(float damage)
    {
        if (invulnerable)
            return;

        if (enemyArmorHP > 0)
        {
            // If enemy has armor, damage the armor instead.
            enemyArmorHP = enemyArmorHP - damage;

            if (enemyArmorHP <= 0)
                enemyArmorHP = 0;
        }
        else if (enemyArmorHP <= 0)
        {
            // Enemy take damage because no armor left to protect.
            enemyHealth = enemyHealth - damage;
        }

        if (isMiniBoss)
            this.GetComponent<MiniBossScript>().StartleMiniBoss();
        if (isPolarBear)
            this.GetComponent<PolarBearScript>().StartlePolarBear();

        Debug.Log("UPDATE: Enemy took damage.");
        CheckDeathStatus();
    }
}
