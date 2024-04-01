using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

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
        }

    }

    public void NextLevel(string level)
    {
        StartCoroutine(LoadLevel(level));
    }

    private IEnumerator LoadLevel(string level)
    {
        CheckLevelCompletetion(level);

        Cursor.visible = false;
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transitionAnim.SetTrigger("Start");
    }

    private void CheckLevelCompletetion(string level)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main Menu") && level == "Factory")
        {
            //if the player is returning to the factory (mostly done after completing a level), reward them with a completion point,  which will unlock more levels in the MapUI
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range") && MapUI.instance.playerLevelCompletion == 0)
            {
                MapUI.instance.playerLevelCompletion++;
            }
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City") && MapUI.instance.playerLevelCompletion == 1)
            {
                MapUI.instance.playerLevelCompletion++;
            }
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2") && MapUI.instance.playerLevelCompletion == 2)
            {
                MapUI.instance.playerLevelCompletion++;
            }

            PlayerPrefs.SetInt("levelsComplete", MapUI.instance.playerLevelCompletion);
        }
        else
        {
            return;
        }
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

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
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
