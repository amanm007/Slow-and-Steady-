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
    }

    private IEnumerator PauseMenu()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Debug.Log("resume");
        transitionAnim.SetTrigger("End");
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneController.instance.NextLevel("Main Menu");
    }

    public void Quit()
    {
        SaveSystem.instance.SaveData();

    }
}
