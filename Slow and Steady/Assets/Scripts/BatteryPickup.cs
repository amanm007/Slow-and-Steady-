using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float healthBoost = 2f; // Amount of health to increase
   // AudioManager audioManager;

    void Start()
    {
        // Get the AudioSource component
        //collectSound = GetComponent<AudioSource>();
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.health < playerHealth.maxHealth)
            {

                //audioManager.PlaySFX(audioManager.collecting);
                playerHealth.IncreaseHealth(healthBoost);


                Destroy(gameObject);
            }
        }
    }
}
