using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSystem : MonoBehaviour
{
    [Header("Player Settings")]
    public float maxHealth = 100f;
    public float swatDuration = 0.25f; // How long the swatter stays visible

    [Header("References")]
    public GameObject swatterPrefab;
    public Slider healthBar; // Assign your UI health bar slider here
    public Image damageFlashImage; // Assign a fullscreen UI Image for damage flash
    public float flashDuration = 0.2f; // How long the flash lasts

    private float currentHealth;
    private Camera mainCamera;
    private Coroutine damageFlashRoutine;

    private void Start()
    {
        mainCamera = Camera.main;
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (damageFlashImage != null)
        {
            damageFlashImage.color = new Color(1, 0, 0, 0); // Ensure transparent initially
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnSwatter();
        }
    }

    private void SpawnSwatter()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        GameObject swatter = Instantiate(swatterPrefab, mouseWorldPos, Quaternion.identity);
        Destroy(swatter, swatDuration); // Destroy after 0.25 seconds
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (damageFlashRoutine != null)
        {
            StopCoroutine(damageFlashRoutine);
        }
        damageFlashRoutine = StartCoroutine(FlashDamageEffect());

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        if (damageFlashImage != null)
        {
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / flashDuration); // Fades from 50% red to transparent
                damageFlashImage.color = new Color(1, 0, 0, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }
            damageFlashImage.color = new Color(1, 0, 0, 0); // Ensure fully transparent at end
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add any game over logic here
    }
}
