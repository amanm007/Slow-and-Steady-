using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MapUI : MonoBehaviour
{
    public static MapUI instance;

    [SerializeField] Animator mapAnim;

    [SerializeField] private GameObject cityLock, bestbuyLock, houseLock;
    public int playerLevelCompletion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cityLock.SetActive(true); bestbuyLock.SetActive(true); houseLock.SetActive(true);
    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        playerLevelCompletion = PlayerPrefs.GetInt("levelsComplete");
    }

    private void Update()
    {
        CheckLevelCompletion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mapAnim.SetTrigger("Start");
        Cursor.visible = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Cursor.visible = false;
        mapAnim.SetTrigger("End");
    }

    private void CheckLevelCompletion()
    {
        Debug.Log(playerLevelCompletion);

        if (playerLevelCompletion == 1)
        {
            cityLock.SetActive(false);
        }
        else if (playerLevelCompletion >= 1 && playerLevelCompletion == 2)
        {
            cityLock.SetActive(false);
            bestbuyLock.SetActive(false);
        }
        else if (playerLevelCompletion >= 2 && playerLevelCompletion == 3)
        {
            cityLock.SetActive(false);
            bestbuyLock.SetActive(false);
            houseLock.SetActive(false);
        }
    }
}
