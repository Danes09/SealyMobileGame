using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearScript : MonoBehaviour
{
    public GameObject projectileSpear;
    public GameObject projectileCannonball;
    public GameObject projectileBomb;
    public GameObject projectileCrate;
    
    public float spearDamage = 5.0f;
    public float spearSpeed = 1.0f;
    public float cannonballDamage = 5.0f;
    public float cannonballSpeed = 1.0f;
    public float bombDamage = 5.0f;
    public float bombSpeed = 1.0f;
    public float crateDamage = 5.0f;
    public float crateSpeed = 1.0f;

    public float shootRate = 5.0f;
    public float destroyProjectileTimer = 5.0f;

    private float shootCooldown = 0.0f;
    
    void Awake()
    {
        shootCooldown = shootRate;
    }

    void Update()
    {
        LoopAttack();
    }

    void LoopAttack()
    {
        if (shootCooldown > 0.0f)
            shootCooldown -= Time.deltaTime;
        else if (shootCooldown <= 0.0f)
        {
            // Randomize an object for the polar bear to attack with.
            int funcToChoose = Random.Range(0,4);

            switch (funcToChoose)
            {
                case 0:
                    ThrowCrate();
                    break;
                case 1:
                    ThrowBomb();
                    break;
                case 2:
                    ThrowCannonball();
                    break;
                case 3:
                    ThrowSpear();
                    break;
            }
        }
    }

    void ThrowCrate()
    {
        Debug.Log("Polar Bear Update: Throw Crate");
        Vector3 dir = PlayerManagerScript.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject crate = Instantiate(projectileCrate, this.transform.position, Quaternion.identity);

        crate.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
        crate.GetComponent<ProjectileScript>().SetupProjectile(destroyProjectileTimer, crateSpeed, crateDamage);

        shootCooldown = shootRate;
    }

    void ThrowBomb()
    {
        Debug.Log("Polar Bear Update: Throw Bomb");
        Vector3 dir = PlayerManagerScript.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject bomb = Instantiate(projectileBomb, this.transform.position, Quaternion.identity);

        bomb.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
        bomb.GetComponent<ProjectileScript>().SetupProjectile(destroyProjectileTimer, bombSpeed, bombDamage);

        shootCooldown = shootRate;
    }

    void ThrowCannonball()
    {
        Debug.Log("Polar Bear Update: Throw Cannonball");
        Vector3 dir = PlayerManagerScript.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject cannonball = Instantiate(projectileCannonball, this.transform.position, Quaternion.identity);

        cannonball.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
        cannonball.GetComponent<ProjectileScript>().SetupProjectile(destroyProjectileTimer, cannonballSpeed, cannonballDamage);

        shootCooldown = shootRate;
    }

    void ThrowSpear()
    {
        Debug.Log("Polar Bear Update: Throw Spear");
        Vector3 dir = PlayerManagerScript.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject spear = Instantiate(projectileSpear, this.transform.position, Quaternion.identity);

        spear.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
        spear.GetComponent<ProjectileScript>().SetupProjectile(destroyProjectileTimer, spearSpeed, spearDamage);

        shootCooldown = shootRate;
    }

    public void StartlePolarBear()
    {
        Debug.Log("ATK CD BEFORE" + shootCooldown);
        // Polar Bear will reset the attack cooldown.
        shootCooldown = shootRate;
        Debug.Log("ATK CD AFTER" + shootCooldown);
    }
}
