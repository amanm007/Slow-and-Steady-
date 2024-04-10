using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("levelsComplete", LevelManager.instance.playerLevelCompletion);
        PlayerPrefs.SetInt("scraps", ScrapManager.instance.scrap);
        PlayerPrefs.SetFloat("maxHealth", PlayerHealth.instance.maxHealth);
        PlayerPrefs.SetFloat("recharge", SlowMotionAbility.instance.energyRecoveryRate);
        PlayerPrefs.SetFloat("speed", PlayerMovement.instance.SPEED);
    }

    public void LoadData()
    {
        LevelManager.instance.playerLevelCompletion = PlayerPrefs.GetInt("levelsComplete");
        ScrapManager.instance.scrap = PlayerPrefs.GetInt("scraps");
        PlayerHealth.instance.maxHealth = PlayerPrefs.GetFloat("maxHealth");
        SlowMotionAbility.instance.energyRecoveryRate = PlayerPrefs.GetFloat("recharge");
        PlayerMovement.instance.SPEED = PlayerPrefs.GetFloat("speed");
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
