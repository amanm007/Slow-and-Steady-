using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveDisplay;

    private string levelObjective = "Kill enemies and go to next zone";
    private string waveObjective = "Survive 3 Waves";
    private string extractionObjective = "Go to extraction zone";
    private string noObjective = "No current objectives";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjective();
    }

    private void UpdateObjective()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Silicon Valley"))
        {
            objectiveDisplay.text = levelObjective;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City")) {
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


    }
}
