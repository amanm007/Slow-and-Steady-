using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private string emptyText = " ";
    private string initialTitle, initialInfo, initialCost;

/*    [Header("Upgrade Buttons")]
    [SerializeField] private Button healthButton;
    [SerializeField] private GameObject rechargeButton;
    [SerializeField] private GameObject speedButton;*/


    [Header("Titles")]
    [SerializeField] private string health_title;
    [SerializeField] private string recharge_title;
    [SerializeField] private string speed_title;

    [Header("Info")]
    [SerializeField] private string health_info;
    [SerializeField] private string recharge_info;
    [SerializeField] private string speed_info;

    [Header("Value")]
    private float health_upgrade_value;
    private float recharge_upgrade_value;
    private float speed_upgrade_value;

    [Header("Cost")]
    private float healthCost;
    private float rechargeCost;
    private float speedCost;

    [SerializeField] private TMP_Text title, info, cost;

    private int health_upgrade_level, recharge_upgrade_level, speed_upgrade_level;

    private bool healthUpgradeSelected, rechargeUpgradeSelected, speedUpgradeSelected;
   

    private int upgradeCost;
    private string insufficientFunds = "insufficient funds";
    private string upgradePurchased = "enjoy the new upgrade";
    private string selectMessage = "please select an upgrade";
    private string upgradeState = "maximum upgrades purchased";

    [SerializeField] private GameObject buy;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        
        title.text = emptyText;
        info.text = selectMessage;
        cost.text = emptyText;
        buy.SetActive(false);

        health_upgrade_value = 1f;
        recharge_upgrade_value = 2f;
        speed_upgrade_value = 1f;

        healthCost = 150f;
        rechargeCost = 300f;
        speedCost = 500f;

        healthUpgradeSelected = false;
        rechargeUpgradeSelected = false;
        speedUpgradeSelected = false;

    }

    private void Update()
    {
        CheckUpgradeSelected();
    }

    public void OpenMenu()
    {
        Cursor.visible = true;

        title.text = emptyText;
        info.text = selectMessage;
        cost.text = emptyText;
        buy.SetActive(false);
    }

    public void CloseMenu()
    {
        Cursor.visible = false;

        title.text = emptyText;
        info.text = emptyText;
        cost.text = emptyText;

        buy.SetActive(false);
    }

    public void HealthUpgrade()
    {
        buy.SetActive(true);
        title.text = health_title;
        info.text = health_info + " " + health_upgrade_value.ToString();
        cost.text = healthCost.ToString();
        upgradeCost = (int)healthCost;
        healthUpgradeSelected = true;
    }

    public void RechargeUpgrade()
    {
        buy.SetActive(true);
        title.text = recharge_title;
        info.text = recharge_info + " " + recharge_upgrade_value.ToString() + "% faster";
        cost.text = rechargeCost.ToString();
        upgradeCost = (int)rechargeCost;
        rechargeUpgradeSelected = true;
    }

    public void SpeedUpgrade()
    {
        buy.SetActive(true);
        title.text = speed_title;
        info.text = speed_info + " " + speed_upgrade_value.ToString();
        cost.text = speedCost.ToString();
        upgradeCost = (int)speedCost;
        speedUpgradeSelected = true;
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

    private void CheckUpgradeSelected()
    {
        Debug.Log(healthUpgradeSelected + "_" + rechargeUpgradeSelected + "_" + speedUpgradeSelected);
        if (healthUpgradeSelected)
        {
            rechargeUpgradeSelected = false;
            speedUpgradeSelected = false;
        }
        if (rechargeUpgradeSelected)
        {
            healthUpgradeSelected = false;
            speedUpgradeSelected = false;
        }
        if (speedUpgradeSelected)
        {
            healthUpgradeSelected = false;
            rechargeUpgradeSelected = false;
        }
/*        else
        {
            healthUpgradeSelected = false;
            rechargeUpgradeSelected = false;
            speedUpgradeSelected = false;
        }*/
    }

    private void CheckUpgradeLevel()
    {
        if (healthUpgradeSelected == true)
        {
            PlayerHealth.instance.SetMaxHealth(health_upgrade_value);
            healthUpgradeSelected = false;
        }

        //recharge 
        else if (rechargeUpgradeSelected == true)
        {
            Debug.Log("recharge purchased");
            if (recharge_upgrade_level < 5)
            {
                recharge_upgrade_level++;
                SlowMotionAbility.instance.SetRecoveryRate(recharge_upgrade_value);
                rechargeUpgradeSelected = false;
            }
        }
        else if (speedUpgradeSelected == true)
        {
            if (speed_upgrade_level < 5)
            {
                speed_upgrade_level++;
                PlayerMovement.instance.SetSpeedUpgrade(speed_upgrade_value);
                speedUpgradeSelected = false;
            }
        }
    }
    private IEnumerator ShowErrorMessage()
    {
        buy.SetActive(false);
        title.text = emptyText;
        info.text = insufficientFunds;
        cost.text = emptyText;
        yield return new WaitForSeconds(2f);
        title.text = initialTitle;
        info.text = initialInfo;
        cost.text = initialCost;
        buy.SetActive(true);
    }

    private IEnumerator ShowStateMessage()
    {
        buy.SetActive(false);
        title.text = emptyText;
        info.text = upgradeState;
        cost.text = emptyText;
        buy.SetActive(true);
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator ShowPurchaseMessage()
    {
        title.text = emptyText;
        info.text = upgradePurchased;
        cost.text = emptyText;
        buy.SetActive(false);
        yield return new WaitForSeconds(3f);
    }
}
