using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private string emptyText = " ";
    private string initialTitle, initialInfo, initialCost;

    [Header("Titles")]
    [SerializeField] private string health_title;
    [SerializeField] private string recharge_title;
    [SerializeField] private string ammo_title;

    [Header("Info")]
    [SerializeField] private string health_info;
    [SerializeField] private string recharge_info;
    [SerializeField] private string ammo_info;

    [Header("Value")]
    [SerializeField] private float health_upgrade_value;
    [SerializeField] private float recharge_upgrade_value;
    [SerializeField] private float ammo_upgrade_value;

    [Header("Cost")]
    [SerializeField] private float health_cost;
    [SerializeField] private float recharge_cost;
    [SerializeField] private float ammo_cost;

    [SerializeField] private TMP_Text title, info, cost;

    private int health_upgrade_level, recharge_upgrade_level, ammo_upgrade_level;
   

    private int upgradeCost;
    private string insufficientFunds = "insufficient funds";
    private string upgradePurchased = "enjoy the new upgrade";
    private string selectMessage = "please select an upgrade";

    [SerializeField] private GameObject buy;

    private void Start()
    {
        title.text = emptyText;
        info.text = emptyText;
        cost.text = emptyText;
        buy.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        title.text = emptyText;
        info.text = emptyText;
        cost.text = emptyText;
        ShowSelectMessage();
        buy.SetActive(false);
    }

    public void HealthUpgrade()
    {
        buy.SetActive(true);
        title.text = health_title;
        info.text = health_info + health_upgrade_value.ToString() + "%";
        cost.text = health_cost.ToString() + " scraps";
        upgradeCost = (int)health_cost;
    }

    public void RechargeUpgrade()
    {
        buy.SetActive(true);
        title.text = recharge_title;
        info.text = recharge_info + recharge_upgrade_value.ToString() + " seconds";
        cost.text = recharge_cost.ToString() + " scraps";
        upgradeCost = (int)recharge_cost;
    }

    public void AmmoUpgrade()
    {
        buy.SetActive(true);
        title.text = ammo_title;
        info.text = ammo_info + ammo_upgrade_value.ToString() + " rounds";
        cost.text = ammo_cost.ToString() + " scraps";
        upgradeCost = (int)ammo_cost;
    }

    public void BuyUpgrade()
    {
        initialTitle = title.text;
        initialInfo = info.text;
        initialCost = cost.text;

        if (ScrapManager.instance.GetScrapAmount() < upgradeCost)
        {
            StartCoroutine(ShowErrorMessage());
        }
        else if(ScrapManager.instance.GetScrapAmount() >= upgradeCost)
        {
            CheckUpgradeLevel();
            StartCoroutine(ShowPurchaseMessage());
            ScrapManager.instance.ChangeScraps(-upgradeCost);
        }

    }

    private void CheckUpgradeLevel()
    {
        //health
        if(upgradeCost == health_cost)
        {
            if (health_upgrade_level == 0)
            {
                Debug.Log("health lvl 1");
                health_upgrade_level++;
                health_upgrade_value += health_upgrade_value * 0.25f;
                health_cost += health_cost * 2;
            }
            else if (health_upgrade_level == 1)
            {
                Debug.Log("health lvl 2");
                health_upgrade_level++;
                health_upgrade_value += health_upgrade_value * 0.25f;
                health_cost += health_cost * 2;
            }
            else if (health_upgrade_level == 2)
            {
                //health_upgrade++;
                health_cost += health_cost * 2;
            }
        }

        //recharge 
        else if (upgradeCost == recharge_cost)
        {
            if (recharge_upgrade_level == 0)
            {
                recharge_upgrade_level++;
                recharge_upgrade_value += recharge_upgrade_value * 0.25f;
                recharge_cost += recharge_cost * 0.25f;
            }
            else if (recharge_upgrade_level == 1)
            {
                recharge_upgrade_level++;
                recharge_upgrade_value += recharge_upgrade_value * 0.25f;
                recharge_cost += recharge_cost * 0.25f;
            }
            else if (recharge_upgrade_level == 2)
            {
                //health_upgrade++;
                recharge_cost += recharge_cost * 0.25f;
            }
        }
    }
    private IEnumerator ShowErrorMessage()
    {
        title.text = emptyText;
        info.text = insufficientFunds;
        cost.text = emptyText;
        yield return new WaitForSeconds(2f);
        title.text = initialTitle;
        info.text = initialInfo;
        cost.text = initialCost;
    }

    private IEnumerator ShowPurchaseMessage()
    {
        buy.SetActive(false);
        title.text = emptyText;
        info.text = upgradePurchased;
        cost.text = emptyText;
        yield return new WaitForSeconds(2f);
        ShowSelectMessage();
    }

    private void ShowSelectMessage()
    {
        Debug.Log("select");
        buy.SetActive(false);
        title.text = emptyText;
        info.text = selectMessage;
        cost.text = emptyText;
    }
}
