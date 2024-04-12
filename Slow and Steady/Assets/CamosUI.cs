using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamosUI : MonoBehaviour
{
    [SerializeField] Animator camoUI;
    [SerializeField] private GameObject eToInteract;
    private bool canInteract, hasInteracted;


    private void Start()
    {
        eToInteract.SetActive(false);
        canInteract = false;
        hasInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eToInteract.SetActive(true);
        canInteract = true;
    }

    private void Update()
    {
        if (canInteract == true && Input.GetKeyDown(KeyCode.E) && hasInteracted == false)
        {
            camoUI.SetTrigger("Start");
            PlayerMovement.instance.pauseMovement = true;
            hasInteracted = true;
            canInteract = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        camoUI.SetTrigger("End");
        Cursor.visible = false;
        PlayerMovement.instance.pauseMovement = false;
        hasInteracted = false;
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        eToInteract.SetActive(false);
        hasInteracted = false;
        canInteract = false;
    }
}
