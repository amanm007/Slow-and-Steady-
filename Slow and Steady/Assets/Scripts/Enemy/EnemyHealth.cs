using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 1;
    public int currentHealth;
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector to the child's SpriteRenderer
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public float damageDuration = 0.2f;
    public delegate void EnemyDefeatedAction();
    public event EnemyDefeatedAction OnDefeated;

    private Color originalColor;

    private ScrapManager scrapManager;
    private EnemyManager enemyManager;

    public AudioManager audioManager;

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
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        scrapManager = ScrapManager.instance;
        enemyManager = EnemyManager.instance;
    }

    private void Update()
    {
        if (GameObject.Find("CRT TV").GetComponent<SlowMotionAbility>().isSlowMotionActive)
        {
            flashDuration = 0.03f;
            damageDuration = 0.06f;
        }
        else
        {
            flashDuration = 0.1f;
            damageDuration = 0.2f;
        }
    }
    
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Start the flash effect
        StartCoroutine(FlashEffect());
        audioManager.PlaySFX(audioManager.enemyDamage);

        if (currentHealth <= 0)
        {
            audioManager.PlaySFX(audioManager.enemyDeath);
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
       // OnDefeated?.Invoke();
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
        
        Destroy(gameObject);
    }
}