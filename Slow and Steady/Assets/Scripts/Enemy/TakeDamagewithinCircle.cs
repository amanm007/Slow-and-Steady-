using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamagewithinCircle : MonoBehaviour
{
    public float slowdownRadius = 5f;
    // public PlayerMovement playerMovementScript;
    public PlayerHealth playerh;
    private float damage=1f;

    private Transform playerTransform;

    private void Start()
    {


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //playerMovementScript = player.GetComponent<PlayerMovement>();
            playerh = player.GetComponent<PlayerHealth>();

            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerh != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);


            if (distanceToPlayer <= slowdownRadius)
            {

                // playerMovementScript.ModifySpeed(0.5f);
                playerh.TakeDamage(damage);
            }
            else
            {

                //playerMovementScript.ModifySpeed(1f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, slowdownRadius);
    }

}