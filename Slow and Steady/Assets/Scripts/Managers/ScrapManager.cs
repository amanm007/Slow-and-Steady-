using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager instance;

    public int scrap;
    [SerializeField] private TMP_Text scrapDisplayUpgrades, scrapDisplayCamos;

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
        scrap = 999;
    }

    private void OnGUI()
    {
        scrapDisplayUpgrades.text = scrap.ToString();
        scrapDisplayCamos.text = scrap.ToString();
    }

    public void ChangeScraps(int amount)
    {
        if(scrap < 999)
        {
            scrap += amount;
        }
        PlayerPrefs.SetInt("scraps", scrap);
    }

    public int GetScrapAmount()
    {
        return scrap;
    }
}
