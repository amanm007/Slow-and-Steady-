using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager instance;

    public int scrap;
    [SerializeField] private TMP_Text scrapDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        scrap = PlayerPrefs.GetInt("scraps");
    }

    private void OnGUI()
    {
        scrapDisplay.text = scrap.ToString();
    }

    public void ChangeScraps(int amount)
    {
        scrap += amount;
        PlayerPrefs.SetInt("scraps", scrap);
    }

    public int GetScrapAmount()
    {
        return scrap;
    }
}
