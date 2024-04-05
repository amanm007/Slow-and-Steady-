using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MapUI : MonoBehaviour
{
    public static MapUI instance;

    [Header("Lock Icons")]
    [SerializeField] private GameObject cityLock;
    [SerializeField] private GameObject bestbuyLock;
    [SerializeField] private GameObject houseLock;

    [Header("Menus")]
    [SerializeField] private Animator mapSelectAnim;
    [SerializeField] private Animator mapInfoAnim;

    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject levelInfoMenu;

    [Header("Menu Info")]
    [SerializeField] private string levelOne_title;
    [SerializeField] private string levelTwo_title;
    [SerializeField] private string levelOne_info;
    [SerializeField] private string levelTwo_info;
    [SerializeField] private TMP_Text title, info;

    [Header("Menu Icons")]
    [SerializeField] private GameObject cityIcon;
    [SerializeField] private GameObject bestbuyIcon;
    [SerializeField] private GameObject houseIcon;

    private string levelSelection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cityLock.SetActive(true); bestbuyLock.SetActive(true); houseLock.SetActive(true);
        cityIcon.SetActive(false); bestbuyIcon.SetActive(false); houseIcon.SetActive(false);
    }

    private void Update()
    {
        CheckLevelCompletion();
        Debug.Log(mapSelectAnim);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mapSelectAnim.SetTrigger("Start");
        Cursor.visible = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Cursor.visible = false; 
    }

    private void CheckLevelCompletion()
    {
        if (LevelManager.instance.playerLevelCompletion == 1)
        {
            cityLock.SetActive(false);
            
        }
        else if (LevelManager.instance.playerLevelCompletion == 2)
        {
            cityLock.SetActive(false);
            bestbuyLock.SetActive(false);
        }
        else if (LevelManager.instance.playerLevelCompletion == 3)
        {
            cityLock.SetActive(false);
            bestbuyLock.SetActive(false);
            houseLock.SetActive(false);
        }
    }

    public void OpenLevelInfo(string level)
    {
        mapSelectAnim.SetTrigger("End");
        mapInfoAnim.SetTrigger("Start");
        cityIcon.SetActive(false); bestbuyIcon.SetActive(false); houseIcon.SetActive(false);

        if (level == "Silicon Valley")
        {
            title.text = levelOne_title;
            info.text = levelOne_info;
            cityIcon.SetActive(true);
        }
        else if (level == "Best Buy2")
        {
            title.text = levelTwo_title;
            info.text = levelTwo_info;
            bestbuyIcon.SetActive(true);
        }

        levelSelection = level;
    }

    public void BackToLevelSelect()
    {
        mapInfoAnim.SetTrigger("End");
        mapSelectAnim.SetTrigger("Start");
    }

    public void CloseMenu()
    {
        mapSelectAnim.SetTrigger("End");
    }
    public void PlayLevel()
    {
        SceneController.instance.NextLevel(levelSelection);
        levelSelection = null;
    }
}
