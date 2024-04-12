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
    [SerializeField] private GameObject warehouseLock;

    [Header("Menus")]
    [SerializeField] private Animator mapSelectAnim;
    [SerializeField] private Animator mapInfoAnim;

    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject levelInfoMenu;

    [Header("Menu Info")]
    [SerializeField] private string levelOne_title;
    [SerializeField] private string levelTwo_title;
    [SerializeField] private string levelThree_title;
    [SerializeField] private string levelOne_info;
    [SerializeField] private string levelTwo_info;
    [SerializeField] private string levelThree_info;
    [SerializeField] private TMP_Text title, info;

    [Header("Menu Icons")]
    [SerializeField] private GameObject cityIcon;
    [SerializeField] private GameObject bestbuyIcon;
    [SerializeField] private GameObject warehouseIcon;

    private string levelSelection;

    [SerializeField] private GameObject eToInteract;
    private bool canInteract, hasInteracted;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cityLock.SetActive(true); bestbuyLock.SetActive(true); warehouseLock.SetActive(true);
        cityIcon.SetActive(false); bestbuyIcon.SetActive(false); warehouseIcon.SetActive(false);
    }

    private void Start()
    {
        eToInteract.SetActive(false);
        canInteract = false;
        hasInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eToInteract.SetActive(true);
        canInteract = true;
    }

    private void Update()
    {
        if (canInteract == true && Input.GetKeyDown(KeyCode.E) && hasInteracted == false)
        {
            Debug.Log("e");

            Cursor.visible = true;
            mapSelectAnim.SetTrigger("Start");
            PlayerMovement.instance.pauseMovement = true;

            hasInteracted = true;
            canInteract = false;
        }

        CheckLevelCompletion();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        eToInteract.SetActive(false);
        hasInteracted = false;
        canInteract = false;
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
            warehouseLock.SetActive(false);
        }
    }

    public void OpenLevelInfo(string level)
    {
        Cursor.visible = true;
        mapSelectAnim.SetTrigger("End");
        mapInfoAnim.SetTrigger("Start");
        cityIcon.SetActive(false); bestbuyIcon.SetActive(false); warehouseIcon.SetActive(false);

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
        else if (level == "Inside Warehouse")
        {
            title.text = levelThree_title;
            info.text = levelThree_info;
            warehouseIcon.SetActive(true);
        }

        levelSelection = level;
    }

    public void BackToLevelSelect()
    {
        Cursor.visible = true;
        mapInfoAnim.SetTrigger("End");
        mapSelectAnim.SetTrigger("Start");
    }

    public void CloseMenu()
    {
        Cursor.visible = false;
        mapSelectAnim.SetTrigger("End");
        PlayerMovement.instance.pauseMovement = false;
        hasInteracted = false;
        canInteract = true;
    }
    public void PlayLevel()
    {
        SceneController.instance.NextLevel(levelSelection);
        levelSelection = null;
        PlayerMovement.instance.pauseMovement = false;
    }
}
