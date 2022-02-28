using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossScript : MonoBehaviour
{
    public GameObject projectileSpear;
    public float damageToPlayer = 5.0f;
    public float shootRate = 5.0f;
    public float spearSpeed = 1.0f;
    public float destroySpearTimer = 5.0f;

    private float spearCooldown = 0.0f;
    
    void Awake()
    {
        spearCooldown = shootRate;
    }

    void Update()
    {
        ThrowSpear();
    }

    void ThrowSpear()
    {
        if (spearCooldown > 0.0f)
            spearCooldown -= Time.deltaTime;
        else if (spearCooldown <= 0.0f)
        {
            Vector3 dir = PlayerManagerScript.Instance.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            GameObject spear = Instantiate(projectileSpear, this.transform.position, Quaternion.identity);

            spear.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
            spear.GetComponent<ProjectileScript>().SetupProjectile(destroySpearTimer, spearSpeed, damageToPlayer);

            spearCooldown = shootRate;
        }
    }

    public void StartleMiniBoss()
    {
        Debug.Log("SPEAR CD BEFORE" + spearCooldown);
        // Mini boss will reset the spear.
        spearCooldown = shootRate;
        Debug.Log("SPEAR CD AFTER" + spearCooldown);
    }
}
