using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public float lsrDamage = .01f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log($"Bullet collided with: {collider.gameObject.name} on layer {LayerMask.LayerToName(collider.gameObject.layer)}");
        if (collider.CompareTag("Player"))
        {
            var playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Player takes damage from bullet.");
                playerHealth.TakeDamage(lsrDamage);
            }

            // Destroy the bullet after applying damage
            Destroy(gameObject);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("BulletTraceLayer"))
        {

            Debug.Log("Bullet destroyed by hitting an obstacle.");
            Destroy(gameObject);
        }

    }

}