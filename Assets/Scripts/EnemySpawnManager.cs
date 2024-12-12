using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform player; // Reference to the player

    [Header("Initial Spawn")]
    [SerializeField] private int initialSpawnCount = 10; // Number of enemies to spawn at the start
    [SerializeField] private Vector2 initialSpawnArea = new Vector2(10f, 10f); // Initial spawn area size

    [Header("Wave Spawn")]
    [SerializeField] private int enemiesPerWave = 5; // Number of enemies per wave
    [SerializeField] private float waveInterval = 10f; // Time interval between waves
    [SerializeField] private float waveSpawnRadius = 15f; // Radius around the player for wave spawns
    [SerializeField] private float minimumSpawnDistance = 5f; // Minimum distance from the player for all spawns

    private float nextWaveTime = 0f;
    private bool initialSpawnComplete = false;

    void Start()
    {
        // Perform the initial spawn
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemyInArea(initialSpawnArea);
        }

        // Set the time for the first wave spawn
        nextWaveTime = Time.time + waveInterval;
        initialSpawnComplete = true;
    }

    void Update()
    {
        if (initialSpawnComplete && Time.time >= nextWaveTime)
        {
            SpawnWave();
            nextWaveTime = Time.time + waveInterval; // Schedule the next wave
        }
    }

    void SpawnEnemyInArea(Vector2 area)
    {
        Vector2 spawnPosition;
        int attempts = 0;

        do
        {
            // Generate a random position within the specified area
            spawnPosition = new Vector2(
                Random.Range(-area.x, area.x),
                Random.Range(-area.y, area.y)
            );
            attempts++;
        }
        while (Vector2.Distance(spawnPosition, player.position) < minimumSpawnDistance && attempts < 10);

        // Instantiate the enemy at the valid position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemyAroundPlayer();
        }
    }

    void SpawnEnemyAroundPlayer()
    {
        if (player != null)
        {
            Vector2 spawnPosition;
            int attempts = 0;

            do
            {
                // Generate a random position around the player within the wave spawn radius
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                spawnPosition = (Vector2)player.position + randomDirection * Random.Range(minimumSpawnDistance, waveSpawnRadius);
                attempts++;
            }
            while (Vector2.Distance(spawnPosition, player.position) < minimumSpawnDistance && attempts < 10);

            // Instantiate the enemy at the valid position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
