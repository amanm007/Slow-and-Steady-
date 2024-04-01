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

    private static bool practiceComplete, cityComplete, bestbuyComplete;

    private void Awake()
    {
        Cursor.visible = false;
        if(instance == null)
        {
            instance = this;
        }

        practiceComplete = false;
        cityComplete = false; 

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
        SceneManager.LoadSceneAsync(level);
        yield return new WaitForSeconds(1f);
        transitionAnim.SetTrigger("Start");
    }

    private void CheckLevelCompletetion(string level)
    {
        if(level == "Factory")
        {
            //if the player returns from a level that is required to progress (unlock a new level), then add it to the players rewards
            //booleans only allow entering the if statements once
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range") && practiceComplete == false)
            {
                MapUI.instance.playerLevelCompletion++;
                practiceComplete = true;
            }
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City") && cityComplete == false)
            {
                MapUI.instance.playerLevelCompletion++;
                cityComplete = true;
            }

            PlayerPrefs.SetInt("levelsComplete", MapUI.instance.playerLevelCompletion);
        }
        else
        {
            Debug.Log("non progression level");
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

    public void MainMenu()
    {
        StartCoroutine(LoadLevel("Main Menu"));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
