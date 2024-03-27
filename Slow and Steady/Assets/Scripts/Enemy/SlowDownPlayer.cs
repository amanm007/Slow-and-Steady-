using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownPlayer : MonoBehaviour
{
    public float slowdownRadius = 5f; 
    public PlayerMovement playerMovementScript; 

    private void Start()
    {
        
      
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if (playerMovementScript != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerMovementScript.transform.position);


            if (distanceToPlayer <= slowdownRadius)
            {

                playerMovementScript.ModifySpeed(0.5f);
            }
            else
            {

                playerMovementScript.ModifySpeed(1f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // You can change this to any color you like
        Gizmos.DrawWireSphere(transform.position, slowdownRadius);
    }

}