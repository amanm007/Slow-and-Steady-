using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;
    [SerializeField] private Animator transitionAnim;
    [HideInInspector] public bool levelZeroComplete;
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (instance == null)
        {
            instance = this;
        }
        levelZeroComplete = false;
    }

    public void NextLevel(string level)
    {
        CheckLevelCompletetion(level);
        StartCoroutine(LoadLevel(level));
    }

    private IEnumerator LoadLevel(string level)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        audioManager.PlaySFX(audioManager.transitionIn, 0.3f);
        transitionAnim.SetTrigger("Start");
    }

    private void CheckLevelCompletetion(string level) 
    { 
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range") && level == "Factory")
        {
            levelZeroComplete = true;
        }
    }


    public void PlayGame()
    {
        StartCoroutine(LoadLevel("Intro Cutscene"));
    }

    public void Factory()
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
        SaveSystem.instance.SaveData();
        StartCoroutine(LoadLevel("Main Menu"));
    }

    public void Quit()
    {
        SaveSystem.instance.SaveData();
        Application.Quit();
    }
}
