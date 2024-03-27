using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector to the child's SpriteRenderer
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public float damageDuration = 0.2f;

    private Color originalColor;
    private bool isFlashing = false;

    private ScrapManager scrapManager;
    private EnemyManager enemyManager;

    [SerializeField] private int value;
    [SerializeField] private Sprite damageSprite;

    private int enemiesKilled;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color
    }

    private void Start()
    {
        scrapManager = ScrapManager.instance;
        enemyManager = EnemyManager.instance;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Start the flash effect
        StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
        {
            // Delay the death to ensure the flash effect can be seen
            Invoke(nameof(Die), flashDuration);
        }
    }

    private IEnumerator FlashEffect()
    {
        Sprite initialSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = damageSprite;
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.sprite = initialSprite;
    }

    private void Die()
    {
        enemiesKilled++;
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Shooting Range"))
        {
            scrapManager.ChangeScraps(value);
            enemyManager.ChangeCount(enemiesKilled);
        }
        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>(); // Find the WaveSpawner in the scene
        if (waveSpawner != null)
        {
            waveSpawner.spawnedEnemies.Remove(gameObject);
        }
        // TODO: Add any death effects or animations here
        Destroy(gameObject);
    }
}