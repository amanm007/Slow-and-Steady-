using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject extractionUI, waveUI;

    void Start()
    {
        waveUI.SetActive(false);
        extractionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City"))
        {
            if (WaveSpawner.instance.currWave < 3)
            {
                waveUI.SetActive(true);
            }
            else if (WaveSpawner.instance.currWave == 3)
            {
                waveUI.SetActive(false);
                extractionUI.SetActive(true);
            }
        }
    }
}
