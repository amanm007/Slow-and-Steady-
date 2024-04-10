using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [SerializeField] private Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        StartCoroutine(PauseMenu());
        transitionAnim.SetTrigger("Start");
        PlayerMovement.instance.pauseMovement = true;
    }

    private IEnumerator PauseMenu()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        transitionAnim.SetTrigger("End");
        Time.timeScale = 1f;
        PlayerMovement.instance.pauseMovement = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        //transitionAnim.SetTrigger("End");
        SceneController.instance.NextLevel("Main Menu");
        PlayerMovement.instance.pauseMovement = false;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SaveSystem.instance.SaveData();
        Application.Quit();
    }
}
