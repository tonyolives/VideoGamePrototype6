using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WaveUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI spawnBagText;

    [Header("References")]
    public GameManager gameManager;
    public MosquitoSpawner mosquitoSpawner;

    private void Update()
    {
        if (gameManager == null || mosquitoSpawner == null) return;

        UpdateWaveText();
        UpdateEnemiesLeftText();
        UpdateSpawnBagText();
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave: {gameManager.GetCurrentWave()}";
    }

    private void UpdateEnemiesLeftText()
    {
        int enemiesRemaining = gameManager.GetEnemiesRemainingInWave();
        enemiesLeftText.text = $"Enemies Left: {enemiesRemaining}";
    }

    private void UpdateSpawnBagText()
    {
        List<string> bagContents = mosquitoSpawner.GetSpawnBagContents();
        spawnBagText.text = "Spawn Bag: " + string.Join(", ", bagContents);
    }
}
