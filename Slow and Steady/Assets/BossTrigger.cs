using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Animator bossAnimator;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("stage1");
            }
        }
    }
}
