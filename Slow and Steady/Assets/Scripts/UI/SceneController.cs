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
        Cursor.visible = false;
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
/*        else
        {
            Destroy(gameObject);
        }*/
    }

    public void NextLevel(string level)
    {
        StartCoroutine(LoadLevel(level));
    }

    private IEnumerator LoadLevel(string level)
    {
        Cursor.visible = false;
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(level);
        yield return new WaitForSeconds(1f);
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

    public void BestBuy()
    {
        StartCoroutine(LoadLevel("Best Buy2"));
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
