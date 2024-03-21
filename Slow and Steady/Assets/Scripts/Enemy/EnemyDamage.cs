using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 1f;
    public float bulletdamage = 1f;
    public float continuousDamageInterval = 2.5f; 
    private Coroutine damageCoroutine; // To track the continuous damage coroutine

    

    private void Start()
    {
        
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Player takes damage on collision.");
                playerHealth.TakeDamage(damageAmount);

                
            }
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check for player entering the continuous damage area
        if (collider.CompareTag("Player") && damageCoroutine == null)
        {
            //Debug.Log("Player enters continuous damage area.");
            damageCoroutine = StartCoroutine(ApplyContinuousDamage(collider.gameObject));
        }
     
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Check for player exiting the continuous damage area
        if (collider.CompareTag("Player") && damageCoroutine != null)
        {
            //Debug.Log("Player exits continuous damage area.");
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator ApplyContinuousDamage(GameObject player)
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        // Loop to apply damage at set intervals
        while (true)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                // Optional: Play sound effect on continuous damage
                // audioManager.PlaySFXWithLowSound(audioManager.poison_effect, volume);
            }
            yield return new WaitForSeconds(continuousDamageInterval);
        }
    }
    
}