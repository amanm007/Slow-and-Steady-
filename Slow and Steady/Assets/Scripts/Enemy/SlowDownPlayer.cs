using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlowDownPlayer : MonoBehaviour
{
    public float slowdownRadius = 5f;
    public float damagePerSecond = 1f; 
    private PlayerMovement playerMovementScript;
    private PlayerHealth playerHealthScript;
    private Transform playerTransform;

    private bool isPlayerWithinRange = false;
    private Coroutine damageCoroutine;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
            playerHealthScript = player.GetComponent<PlayerHealth>();
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            isPlayerWithinRange = distanceToPlayer <= slowdownRadius;

            if (isPlayerWithinRange)
            {
                Debug.Log("slow");
                playerMovementScript.ModifySpeed(0.5f);
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(ApplyDamageOverTime());
                }
            }
            else
            {
                
                playerMovementScript.ModifySpeed(1f);
                if (damageCoroutine != null)
                {
                    StopCoroutine(damageCoroutine);
                    damageCoroutine = null;
                }
            }
        }
    }

    IEnumerator ApplyDamageOverTime()
    {
        while (isPlayerWithinRange)
        {
            playerHealthScript.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, slowdownRadius);
    }

}