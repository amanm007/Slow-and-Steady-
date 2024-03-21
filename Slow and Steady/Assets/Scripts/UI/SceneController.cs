using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;
    [SerializeField] private Animator transitionAnim;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void NextLevel(string level)
    {
        StartCoroutine(LoadLevel(level));
    }

    private IEnumerator LoadLevel(string level)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(level);
        transitionAnim.SetTrigger("Start");
    }


    public void PlayGame()
    {
        StartCoroutine(LoadLevel("Factory"));
    }

    public void City()
    {
        StartCoroutine(LoadLevel("Silicon Valley"));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadLevel("Main Menu"));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
