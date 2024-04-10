using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterShootingRange : MonoBehaviour
{
    public static bool inFactory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Factory"))
            {
                SceneController.instance.NextLevel("Shooting Range");
                inFactory = true;
            }
            else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside Warehouse"))
            {
                SceneController.instance.NextLevel("Inside Warehouse");
            }
        }
    }
}
