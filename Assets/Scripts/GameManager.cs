using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawner Reference")]
    public MosquitoSpawner mosquitoSpawner;

    [Header("Wave System")]
    public float initialSpawnInterval = 2f;  // Initial time between spawns
    public int initialEnemiesPerWave = 5;     // Initial enemies per wave
    public float spawnIntervalRangeReduction = 0.5f; // How much to reduce interval per wave
    public int enemiesIncreasePerWave = 2;    // How many more enemies spawn each wave
    public float minimumSpawnInterval = 0.05f; // Cap to avoid insane spawn rates
                                            //Let's make them, suffer >:)

    private int currentWave = 0;
    private float spawnTimer;
    public int enemiesToSpawnInWave;
    private int enemiesSpawned;

    private void Start()
    {
        StartNewWave();
    }

    private void Update()
    {
        if (enemiesSpawned < enemiesToSpawnInWave)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                enemiesSpawned++;

                // Set spawn timer to a random value within the range
                spawnTimer = Random.Range(Mathf.Max(initialSpawnInterval - (currentWave - 1) * spawnIntervalRangeReduction, 0), minimumSpawnInterval);
            }
        }
        else
        {
            // Once all enemies in wave spawn, wait for next wave
            if (Object.FindObjectsByType<Flight>(FindObjectsSortMode.None).Length == 0)
            {
                StartNewWave();
            }
        }
    }

    private void StartNewWave()
    {
        currentWave++;
        enemiesToSpawnInWave = initialEnemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave;
        enemiesSpawned = 0;

        spawnTimer = 1f; // Give a short pause between waves
        Debug.Log($"Starting Wave {currentWave} with {enemiesToSpawnInWave} enemies!");
    }

    // expose current wave count
    public int GetCurrentWave()
    {
        return currentWave;
    }

    // get enemies remaining in wave
    public int GetEnemiesRemainingInWave()
    {
        return enemiesToSpawnInWave - enemiesSpawned + Object.FindObjectsByType<Flight>(FindObjectsSortMode.None).Length;
    }

    public void SpawnEnemy()
    {
        if (mosquitoSpawner != null)
        {
            mosquitoSpawner.SpawnEnemy();
        }
        else
        {
            Debug.LogWarning("Mosquito reference not set on GameManager.");
        }
    }

    public void SetSpawnInterval(float newInterval)
    {
        initialSpawnInterval = newInterval;
    }
}
