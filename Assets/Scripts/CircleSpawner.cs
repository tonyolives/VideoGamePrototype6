using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public float spawnInterval = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCircle), 0f, spawnInterval);
    }

    void SpawnCircle()
    {
        Vector2 randomPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        Instantiate(circlePrefab, randomPos, Quaternion.identity);
    }
}
