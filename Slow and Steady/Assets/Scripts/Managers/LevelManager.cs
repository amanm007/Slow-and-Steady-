using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] public TMP_Text objectiveDisplay;

    private string factoryObjective = "Go to the Shooting Range";
    private string shootingRangeObjective = "Practice your skills and head back to the HUB";
    private string mapSelectObjective = "Select a level on the map";
    private string levelOneObjective = "Kill all the enemies and travel to the next zone";
    private string waveObjective = "Survive 3 Waves";
    private string extractionObjective = "Go to the Extraction Zone";
    private string warehouseObjective = "Enter the warehouse";
    [HideInInspector] public string whileExtractingObjective = "Wait to be Extracted";
    private string noObjective = "No current objectives";
    [HideInInspector]public int playerLevelCompletion;

    private bool levelOneLocked;
    private bool levelTwoLocked;
    private bool levelThreeLocked;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        levelOneLocked = true;
        levelTwoLocked = true;
        levelThreeLocked = true;
        playerLevelCompletion = PlayerPrefs.GetInt("levelsComplete");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjective();
    }

    private void UpdateObjective()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Factory"))
        {
            if(playerLevelCompletion == 0)
            {
                objectiveDisplay.text = factoryObjective;
            }
            else
            {
                objectiveDisplay.text = mapSelectObjective;
            }
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range"))
        {
            objectiveDisplay.text = shootingRangeObjective;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Silicon Valley"))
        {
            objectiveDisplay.text = levelOneObjective;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside Warehouse"))
        {
            objectiveDisplay.text = warehouseObjective;
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Inside Warehouse")) {
            if (WaveSpawner.instance.currWave < 3)
            {
                objectiveDisplay.text = waveObjective;
            }

            else if (WaveSpawner.instance.currWave == 3)
            {
                objectiveDisplay.text = extractionObjective;
            }
        }
        else
        {
            objectiveDisplay.text = noObjective;
        }
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Factory"))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range") && playerLevelCompletion == 0 && levelOneLocked == true)
            {
                playerLevelCompletion++;
                levelOneLocked = false;
            }
            else if (ExtractionManager.instance != null)
            {
                if (ExtractionManager.instance.levelOneComplete == true && playerLevelCompletion == 1 && levelTwoLocked == true)
                {
                    playerLevelCompletion++;
                    levelTwoLocked = false;
                }
                else if (ExtractionManager.instance.levelTwoComplete == true && playerLevelCompletion == 2 && levelThreeLocked == true)
                {
                    playerLevelCompletion++;
                    levelThreeLocked = false;
                }
            }
            PlayerPrefs.SetInt("levelsComplete", playerLevelCompletion);
        }


    }
}
