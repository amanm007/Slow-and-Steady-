using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    public void SaveData()
    {
        PlayerPrefs.SetInt("levelsComplete", LevelManager.instance.playerLevelCompletion);
        PlayerPrefs.SetInt("scraps", ScrapManager.instance.scrap);
    }

    public void LoadData()
    {
        LevelManager.instance.playerLevelCompletion = PlayerPrefs.GetInt("levelsComplete");
        ScrapManager.instance.scrap = PlayerPrefs.GetInt("scraps");
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
