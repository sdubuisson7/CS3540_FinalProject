using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;  // the enemy prefab to instantiate
    public Vector3 spawnRange;      // the range within which to spawn enemies
    public float initialSpawnInterval = 5f;  // the initial time between spawns
    public float spawnIntervalDecreaseRate = 0.1f;  // the rate at which spawn interval decreases over time
    public float minSpawnInterval = 0.5f;  // the minimum time between spawns

    private float spawnTimer = 0f; //the timer to keep track of when to spawn enemies
    private float currentSpawnInterval; //the current interval at which enemies are being spawned at
    public int spawnLimit = 20;

    void Start()
    {
        //Sets the current enemy spawn interval to it's initial value
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            //Increase the spawn timer over time
            spawnTimer += Time.deltaTime;
            GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            print(currentEnemies.Length);
            //Check to see if spawn timer has reached the spawn interval
            if (spawnTimer >= currentSpawnInterval && currentEnemies.Length < spawnLimit)
            {

                SpawnEnemy(); //Runs the SpawnEnemy method
                spawnTimer = 0f; //Resets the spawn timer to 0
                currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseRate, minSpawnInterval); //Reduce the current spawn interval by the decrease rate and assign the new value
            }
        }
        
    }

    void SpawnEnemy()
    {
        //New vector with random X and Z values within predetermined range
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), spawnRange.y, Random.Range(-spawnRange.z, spawnRange.z));
        //Spawn's enemy at spawnPosition
        int spawnValue = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[spawnValue], spawnPosition, Quaternion.identity);
        enemy.transform.parent = gameObject.transform;
        
    }
}
