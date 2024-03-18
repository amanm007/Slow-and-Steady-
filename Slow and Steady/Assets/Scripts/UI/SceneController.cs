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
            DontDestroyOnLoad(gameObject);
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
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadSceneAsync(level);
        transitionAnim.SetTrigger("Start");
    }


    public void PlayGame()
    {
        StartCoroutine(LoadLevel("Intro Cutscene"));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadLevel("Main Menu"));
    }
}
