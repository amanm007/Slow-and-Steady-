using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] Animator mapAnim;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        mapAnim.SetTrigger("Start");
        Debug.Log("start");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mapAnim.SetTrigger("End");
    }
}
