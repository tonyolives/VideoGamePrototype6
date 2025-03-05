using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;  // Required for DepthOfField

public class PlayerSystem : MonoBehaviour
{
    [Header("Player Settings")]
    public float maxHealth = 100f;
    public float swatDuration = 0.25f;

    [Header("References")]
    public GameObject swatterPrefab;
    //public Slider healthBar;
    [SerializeField] Image healthBag;
    public Image damageFlashImage; 
    public float flashDuration = 0.2f;

    [Header("Hazy Vision Effect")]
    public Volume globalVolume; 
    public float hazyEffectDuration = 3f;

    private float currentHealth;
    private Camera mainCamera;
    private Coroutine damageFlashRoutine;
    private Coroutine hazyEffectRoutine;

    private DepthOfField depthOfField;  // Reference to DoF override in Volume

    private void Start()
    {
        mainCamera = Camera.main;
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (damageFlashImage != null)
        {
            damageFlashImage.color = new Color(1, 0, 0, 0);
        }

        // Try to get DepthOfField from Global Volume Profile
        if (globalVolume != null && globalVolume.profile.TryGet(out depthOfField))
        {
            depthOfField.active = false;  // Ensure DoF starts off
        }
        else
        {
            Debug.LogWarning("DepthOfField not found on Global Volume! Make sure it's added to the Volume Profile.");
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
        Destroy(swatter, swatDuration);
    }

    public void TakeDamage(float damageAmount, bool isHazy)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (damageFlashRoutine != null)
        {
            StopCoroutine(damageFlashRoutine);
        }
        damageFlashRoutine = StartCoroutine(FlashDamageEffect());

        if (isHazy)
        {
            if (hazyEffectRoutine != null)
            {
                StopCoroutine(hazyEffectRoutine);
            }
            hazyEffectRoutine = StartCoroutine(HazyVisionEffectRoutine());
        }

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
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / flashDuration);
                damageFlashImage.color = new Color(1, 0, 0, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }
            damageFlashImage.color = new Color(1, 0, 0, 0);
        }
        else {
            Debug.Log("damageFlashImage is null!");
        }
    }

    private IEnumerator HazyVisionEffectRoutine()
    {
        Debug.Log("Starting Hazy Vision Effect - Enabling Depth of Field");

        if (depthOfField != null)
        {
            depthOfField.active = true;

            yield return new WaitForSeconds(hazyEffectDuration);

            depthOfField.active = false;
        }
        else {
            Debug.Log("ERROR - no depthOfField found!");
        }
    }

    private void UpdateHealthBar()
    {
        // if (healthBar != null)
        // {
        //     healthBar.value = currentHealth / maxHealth;
        // }

        healthBag.fillAmount = currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
