using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Animator upgradeAnim;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        if(canInteract == true && Input.GetKeyDown(KeyCode.E) && hasInteracted == false)
        { 
            Debug.Log("e");
            upgradeAnim.SetTrigger("Start");
            UpgradeManager.instance.OpenMenu();
            PlayerMovement.instance.pauseMovement = true;
            hasInteracted = true;
            canInteract = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        eToInteract.SetActive(false);
        hasInteracted = false;
        canInteract = false;
    }

    public void CloseMenu()
    {
        upgradeAnim.SetTrigger("End");
        UpgradeManager.instance.CloseMenu();
        PlayerMovement.instance.pauseMovement = false;
        hasInteracted = false;
        canInteract = true;
    }
}
