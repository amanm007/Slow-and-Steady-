using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager instance;

    private int scrap;
    [SerializeField] private TMP_Text scrapDisplay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnGUI()
    {
        scrapDisplay.text = scrap.ToString();
    }

    public void ChangeScraps(int amount)
    {
        scrap += amount;
    }
}
