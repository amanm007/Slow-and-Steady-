using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector to the child's SpriteRenderer
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Color originalColor;
    private bool isFlashing = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color
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
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        // TODO: Add any death effects or animations here
        Destroy(gameObject);
    }
}