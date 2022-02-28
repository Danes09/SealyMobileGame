using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public bool destroyUponPlayerContact = true;
    public bool destroyUponBallContact = true;
    [HideInInspector] public float projectileSpeed = 1.0f;
    [HideInInspector] public float damage = 1.0f;
    [HideInInspector] public bool activateProjectile = false;
    
    void Update()
    {
        if (activateProjectile)
            transform.Translate(Vector3.up * Time.deltaTime * projectileSpeed);
    }

    public void SetupProjectile(float destoryTimer, float speedOfProjectile, float damageToPlayer)
    {
        projectileSpeed = speedOfProjectile;
        activateProjectile = true;
        damage = damageToPlayer;
        Destroy(this.gameObject, destoryTimer);
    }

    public void DealDamage(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponentInParent<PlayerMovement>();
        if (player)
        {
            PlayerManagerScript.Instance.TakeDamageFromSpear(damage);
            if (destroyUponPlayerContact)
                this.gameObject.SetActive(false);
            return;
        }
    }

    public void TakeDamage(Collision2D collision)
    {
        if (!destroyUponBallContact)
            return;

        BouncingBall ball = collision.gameObject.GetComponent<BouncingBall>();
        if (ball)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}
