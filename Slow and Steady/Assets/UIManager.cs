using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject extractionUI, waveUI, extractionZone;

    void Start()
    {
        waveUI.SetActive(false);
        extractionUI.SetActive(false);
        extractionZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2"))
        {
            if (WaveSpawner.instance.currWave < 2)
            {
                waveUI.SetActive(true);
            }
            else if (WaveSpawner.instance.currWave == 2)
            {
                waveUI.SetActive(false);
                extractionUI.SetActive(true);
                extractionZone.SetActive(true);
            }
        }
    }
}
