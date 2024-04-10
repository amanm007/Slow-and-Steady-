using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtractionManager : MonoBehaviour
{
    public static ExtractionManager instance;

    [SerializeField] private TMP_Text extractionTimer;
    private static int timeToExtract;
    private float elapsedTime, elapsedTimeFill;
    private float timeLeftUntilExtract;

    [SerializeField] private Image extractBar;
    private float lerpSpeed;

    public bool levelOneComplete, levelTwoComplete, levelThreeComplete;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        timeToExtract = 30;
        elapsedTime = timeToExtract;
        elapsedTimeFill = 1;
        extractionTimer.text = timeToExtract.ToString();
        extractBar.fillAmount = 0.025f;

        levelOneComplete = false; levelTwoComplete = false; levelThreeComplete = false;
    }

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            elapsedTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timeLeftUntilExtract = seconds;

            elapsedTimeFill += Time.deltaTime;
            int secondsFill = Mathf.FloorToInt(elapsedTimeFill % 60);

            if (timeLeftUntilExtract >= 0)
            {
                LevelManager.instance.objectiveDisplay.text = LevelManager.instance.whileExtractingObjective;
                ExtractionBarFiller((float)secondsFill);
                extractionTimer.text = timeLeftUntilExtract.ToString();
            }

            else if (timeLeftUntilExtract <= 0)
            {
                Debug.Log("extraction");

                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City") && levelOneComplete == false)
                {
                    levelOneComplete = true;
                }
                else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2") && levelTwoComplete == false)
                {
                    levelTwoComplete = true;
                }
                else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Inside Warehouse") && levelThreeComplete == false)
                {
                    levelThreeComplete = true;
                }

                SceneController.instance.NextLevel("Factory");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player has left the extraction zone");
        }
    }

    private void ExtractionBarFiller(float seconds)
    {
        extractBar.fillAmount += 0.000525f;
        extractBar.fillAmount = Mathf.Lerp(extractBar.fillAmount, (float)seconds / timeToExtract, lerpSpeed);
    }

}
