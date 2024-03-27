using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownPlayer : MonoBehaviour
{
    public float slowdownRadius = 5f; // Radius within which the player's speed is halved
    public LayerMask playerLayer; // Layer mask to detect the player
    public CircleCollider2D slowDownCollider;

    private void Awake()
    {
        // Assuming this script is attached to the GameObject with the circle collider
        slowDownCollider = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0) // Check if the collided object is the player
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ModifySpeed(0.5f); // Halve the player's speed
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0) // Check if the collided object is the player
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ModifySpeed(1f); // Reset the player's speed to normal
            }
        }
    }

}