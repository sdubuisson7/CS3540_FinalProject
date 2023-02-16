using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // the prefab to instantiate
    public Vector3 spawnRange;      // the range within which to spawn enemies
    public float initialSpawnInterval = 5f;  // the initial time between spawns
    public float spawnIntervalDecreaseRate = 0.1f;  // the rate at which spawn interval decreases over time
    public float minSpawnInterval = 0.5f;  // the minimum time between spawns

    private float spawnTimer = 0f;
    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseRate, minSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), spawnRange.y, Random.Range(-spawnRange.z, spawnRange.z));
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
