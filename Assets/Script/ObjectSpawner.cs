using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector2 spawnScale;
    [SerializeField] private Vector2 itemScale;
    [SerializeField] private float minSpawnCooldown = 1.0f;
    [SerializeField] private float maxSpawnCooldown = 3.0f;
    [SerializeField] private bool applyForceWhenSpawn = false;
    [SerializeField] private Vector2 forceDirection;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    [SerializeField] private int crateEndValue;
    [SerializeField] private float bombChance;
    [SerializeField] private float energyChance;
    [SerializeField] private float foodChance;
    [SerializeField] private float healChance;
    [SerializeField] private float pointsChance;

    private List<GameObject> spawnedObject = new List<GameObject>();

    private Coroutine spawnCoroutine;

    public float Randoms;

    public bool Animbool;
    public Animator anim;
    private void Start()
    {

        if (Animbool == true)
        {
            spawnCoroutine = StartCoroutine(SpawningCoroutines());
        }
        else
        {
            spawnCoroutine = StartCoroutine(SpawningCoroutine());
        }

    }

    private void Destroy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }
    //new
    public void ThrowCrates()
    {

        GameObject toSpawn = SpawnObject();
        toSpawn.transform.localScale = new Vector3(spawnScale.x, spawnScale.y, 1);

        toSpawn.GetComponent<FallingCrate>().SetStats(itemScale, bombChance, energyChance, foodChance, healChance, pointsChance, crateEndValue);

        spawnedObject.Add(toSpawn);

        if (applyForceWhenSpawn)
            AddForceToObject(toSpawn);
    }
    //old
    private IEnumerator SpawningCoroutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnCooldown, maxSpawnCooldown);

            yield return new WaitForSeconds(waitTime);
            if (Animbool == false)
            {
                anim.SetTrigger("ThrowCrate");
            }



            //GameObject toSpawn = SpawnObject();
            //         toSpawn.transform.localScale = new Vector3(spawnScale.x, spawnScale.y, 1);

            //         toSpawn.GetComponent<FallingCrate>().SetStats(itemScale, bombChance, energyChance, foodChance, healChance, pointsChance, crateEndValue);

            //spawnedObject.Add(toSpawn);

            //if (applyForceWhenSpawn)
            //	AddForceToObject(toSpawn);
        }
    }
    private IEnumerator SpawningCoroutines()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnCooldown, maxSpawnCooldown);

            yield return new WaitForSeconds(waitTime);

            GameObject toSpawn = SpawnObject();
            toSpawn.transform.localScale = new Vector3(spawnScale.x, spawnScale.y, 1);

            toSpawn.GetComponent<FallingCrate>().SetStats(itemScale, bombChance, energyChance, foodChance, healChance, pointsChance, crateEndValue);

            spawnedObject.Add(toSpawn);

            if (applyForceWhenSpawn)
                AddForceToObject(toSpawn);
        }
    }

    private GameObject SpawnObject()
    {
        return Instantiate(spawnPrefab, spawnPosition.transform.position, Quaternion.identity);
    }

    private void AddForceToObject(GameObject gameObject)
    {
        Rigidbody2D tempRb2d = gameObject.GetComponent<Rigidbody2D>();
        if (tempRb2d == null)
        {
            tempRb2d = gameObject.AddComponent<Rigidbody2D>();
        }

        tempRb2d.AddForce(forceDirection * Random.Range(minForce, maxForce), ForceMode2D.Impulse);
    }
}
