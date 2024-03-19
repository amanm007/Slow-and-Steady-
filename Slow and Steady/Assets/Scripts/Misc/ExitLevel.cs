using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Silicon Valley"))
            {
                SceneController.instance.NextLevel("Hoard City");
            }
        }

    }
}
