using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterShootingRange : MonoBehaviour
{
    public static bool inFactory;
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
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside Warehouse"))
        {
            SceneController.instance.NextLevel("Inside Warehouse");
        }
        else
        {
            eToInteract.SetActive(true);
            canInteract = true;
        }
    }

    private void Update()
    {
        if (canInteract == true && Input.GetKeyDown(KeyCode.E) && hasInteracted == false)
        {
            hasInteracted = true;
            canInteract = false;
        }

        if(hasInteracted == true)
        {
            CheckLevelChange();
        }
    }

    private void CheckLevelChange()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Factory"))
        {
            SceneController.instance.NextLevel("Shooting Range");
            inFactory = true;
            eToInteract.SetActive(false);
            hasInteracted = false;
            canInteract = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        eToInteract.SetActive(false);
        hasInteracted = false;
        canInteract = false;
    }

}
