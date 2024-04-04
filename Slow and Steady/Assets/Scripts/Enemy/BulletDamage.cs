using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float bulletDamage = 1f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check for collision with the player
        if (collider.CompareTag("Player"))
        {
            var playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Player takes damage from bullet.");
                playerHealth.TakeDamage(bulletDamage);
            }

            // Destroy the bullet after applying damage
            Destroy(gameObject);
        }
/*        else if (collider.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }*/
    }
}