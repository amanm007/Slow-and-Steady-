using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtractionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text extractionTimer;
    private float timeToExtract;
    private float elapsedTime;

    [SerializeField] private Image extractBar;
    private float lerpSpeed;

    void Start()
    {
        timeToExtract = 30f;
        elapsedTime = 3f;
    }

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WaveBarFiller();

            timeToExtract -= Time.deltaTime;
            int timeLeftUntilExtract = Mathf.FloorToInt(timeToExtract % 60);


            if (timeLeftUntilExtract >= 0)
            {
                extractionTimer.text = timeLeftUntilExtract.ToString();
            }


            else if (timeLeftUntilExtract <= 0)
            {
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

    void WaveBarFiller()
    {
        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        Debug.Log(seconds);
        extractBar.fillAmount = Mathf.Lerp(extractBar.fillAmount, seconds / 30f, lerpSpeed);
    }

}
