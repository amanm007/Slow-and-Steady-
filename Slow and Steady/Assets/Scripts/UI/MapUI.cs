using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MapUI : MonoBehaviour
{
    public static MapUI instance;

    [SerializeField] private GameObject cityLock, bestbuyLock, houseLock;
    [SerializeField] private GameObject levelSelectMenu, levelInfoMenu;

    [SerializeField] private string levelOne_title, levelTwo_title;

    [SerializeField] private GameObject mapDisplay;
    [SerializeField] private List<Sprite>[] map;
    private Image mapSelection;

    [SerializeField] private string levelOne_info, levelTwo_info;

    [SerializeField] private TMP_Text title, info;

    private string levelSelection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        levelSelectMenu.SetActive(false);
        levelInfoMenu.SetActive(false);
        cityLock.SetActive(true); bestbuyLock.SetActive(true); houseLock.SetActive(true);

        mapSelection = mapDisplay.GetComponent<Image>();
    }

    private void Update()
    {
        CheckLevelCompletion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelSelectMenu.SetActive(true);
        Cursor.visible = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        levelSelectMenu.SetActive(false);

        if(levelInfoMenu.activeSelf == true)
        {
            levelInfoMenu.SetActive(false);
        }
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
        levelInfoMenu.SetActive(true);
        
        if(level == "Silicon Valley")
        {
            title.text = levelOne_title;
            info.text = levelOne_info;
        }
        else if (level == "Best Buy2")
        {
            title.text = levelTwo_title;
            info.text = levelTwo_info;
        }

        levelSelection = level;
    }
    public void PlayLevel()
    {
        SceneController.instance.NextLevel(levelSelection);
        levelSelection = null;
    }
}
