using UnityEngine;
using System.Collections.Generic;

public class MosquitoSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] mosquitoPrefabs; // {normal, doubleDamage, hazyVision}

    [Header("Spawn Area")]
    public Camera mainCamera;

    private List<int> spawnBag;
    private int currentBagIndex = 0;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        InitializeSpawnBag();
    }

    // Initialize the bag with desired ratios
    private void InitializeSpawnBag()
    {
        spawnBag = new List<int>();
        AddToBag(0, 5); // Normal (5 entries)
        AddToBag(1, 2); // Double Damage (2 entries)
        AddToBag(2, 2); // Hazy Vision (2 entries)

        ShuffleBag();
    }

    // Add entries to bag
    private void AddToBag(int enemyType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            spawnBag.Add(enemyType);
        }
    }

    // Shuffle to make random selection
    private void ShuffleBag()
    {
        for (int i = 0; i < spawnBag.Count; i++)
        {
            int randomIndex = Random.Range(0, spawnBag.Count);
            int temp = spawnBag[i];
            spawnBag[i] = spawnBag[randomIndex];
            spawnBag[randomIndex] = temp;
        }
        currentBagIndex = 0;
    }

    // Public method for GameManager to call
    public void SpawnEnemy()
    {
        if (currentBagIndex >= spawnBag.Count)
        {
            ShuffleBag();
        }

        int enemyIndex = spawnBag[currentBagIndex];
        currentBagIndex++;

        Vector3 spawnPosition = GetRandomScreenPosition();
        Instantiate(mosquitoPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
    }

    // expose bag contents
    public List<string> GetSpawnBagContents()
    {
        List<string> bagContents = new List<string>();
        foreach (int type in spawnBag)
        {
            string typeName = type switch
            {
                0 => "Normal",
                1 => "DoubleDamage",
                2 => "HazyVision",
                _ => "Unknown"
            };
            bagContents.Add(typeName);
        }
        return bagContents;
    }


    // Get a random position within the camera view
    private Vector3 GetRandomScreenPosition()
    {
        float screenX = Random.Range(0f, Screen.width);
        float screenY = Random.Range(0f, Screen.height);
        Vector3 screenPosition = new Vector3(screenX, screenY, mainCamera.nearClipPlane);
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
