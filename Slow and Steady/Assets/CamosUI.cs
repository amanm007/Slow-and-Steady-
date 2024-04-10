using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamosUI : MonoBehaviour
{
    [SerializeField] Animator camoUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        camoUI.SetTrigger("Start");
    }
    public void CloseMenu()
    {
        camoUI.SetTrigger("End");
    }
}
