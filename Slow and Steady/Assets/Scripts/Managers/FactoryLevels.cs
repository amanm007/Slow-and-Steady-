
using UnityEngine;
using UnityEngine.SceneManagement;

public class FactoryLevels : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private GameObject playerPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Factory"))
            {
                SceneController.instance.NextLevel("Shooting Range");
            }
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range"))
            {
                SceneController.instance.NextLevel("Factory");
                playerPos.transform.position = respawnPoint.transform.position;
            }
        }
    }
}
