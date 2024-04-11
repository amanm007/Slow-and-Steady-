using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CamoManager : MonoBehaviour
{
    public static CamoManager instance;

    private string emptyText = " ";
    private string initialTitle, initialInfo, initialCost;
    private Sprite initialSniper;

    /*    [Header("Upgrade Buttons")]
        [SerializeField] private Button healthButton;
        [SerializeField] private GameObject rechargeButton;
        [SerializeField] private GameObject speedButton;*/


    private string green_title = "green";
    private string red_title = "red";
    private string pink_title = "pink";
    private string gold_title = "gold";

    private string health_info;
    private string recharge_info;
    private string speed_info;

    [Header("Cost")]
    [SerializeField] private float greenCost;
    [SerializeField] private float redCost;
    [SerializeField] private float pinkCost;
    [SerializeField] private float goldCost;

    [SerializeField] private TMP_Text title, info, cost;
    [SerializeField] private Sprite greenSniper, redSniper, pinkSniper, goldSniper, invisSniper;
    [SerializeField] private Image sniperCamo;
    private bool greenSelected, redSelected, pinkSelected, goldSelected;
    public bool greenEquipped, redEquipped, pinkEquipped, goldEquipped;


    private int camoCost;
    private string insufficientFunds = "insufficient funds";
    private string camoPurchased = "enjoy the new camo";
    private string selectMessage = "please select a camo";
    private string camoState = "already purchased";

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

        greenSelected = false;
        redSelected = false;
        pinkSelected = false;
        goldSelected = false;

        greenEquipped = false;
        redEquipped = false;
        pinkEquipped = false;
        goldEquipped = false;

        sniperCamo = GameObject.FindGameObjectWithTag("Camo").GetComponent<Image>();
        sniperCamo.sprite = invisSniper;
    }

    private void Update()
    {
        CheckCamoSelected();
    }

    public void OpenMenu()
    {
        Cursor.visible = true;
        sniperCamo.sprite = invisSniper;
        title.text = emptyText;
        info.text = selectMessage;
        cost.text = emptyText;
        buy.SetActive(false);
    }

    public void CloseMenu()
    {
        Cursor.visible = false;
        sniperCamo.sprite = invisSniper;
        title.text = emptyText;
        info.text = emptyText;
        cost.text = emptyText;

        buy.SetActive(false);
    }

    public void GreenCamo()
    {
        buy.SetActive(true);
        title.text = green_title;
        info.text = emptyText;
        sniperCamo.sprite = greenSniper;
        cost.text = greenCost.ToString();
        camoCost = (int)greenCost;
        greenSelected = true;
    }

    public void RedCamo()
    {
        buy.SetActive(true);
        title.text = red_title;
        info.text = emptyText;
        sniperCamo.sprite = redSniper;
        cost.text = redCost.ToString();
        camoCost = (int)redCost;
        redSelected = true;
    }

    public void PinkCamo()
    {
        buy.SetActive(true);
        title.text = pink_title;
        info.text = emptyText;
        sniperCamo.sprite = pinkSniper;
        cost.text = pinkCost.ToString();
        camoCost = (int)pinkCost;
        pinkSelected = true;
    }

    public void GoldCamo()
    {
        buy.SetActive(true);
        title.text = gold_title;
        info.text = emptyText;
        sniperCamo.sprite = goldSniper;
        cost.text = goldCost.ToString();
        camoCost = (int)goldCost;
        goldSelected = true;
    }

    public void BuyCamo()
    {
        initialTitle = title.text;
        initialInfo = info.text;
        initialCost = cost.text;
        initialSniper = sniperCamo.sprite;

        if (ScrapManager.instance.GetScrapAmount() < camoCost)
        {
            StartCoroutine(ShowErrorMessage());
        }
        else if (ScrapManager.instance.GetScrapAmount() >= camoCost)
        {
            CheckCamoPurchase();
            StartCoroutine(ShowPurchaseMessage());
            ScrapManager.instance.ChangeScraps(-camoCost);
        }

    }

    private void CheckCamoSelected()
    {
        if (greenSelected)
        {
            redSelected = false;
            pinkSelected = false;
            goldSelected = false;
        }
        else if (redSelected)
        {
            greenSelected = false;
            pinkSelected = false;
            goldSelected = false;
        }
        else if (pinkSelected)
        {
            greenSelected = false;
            redSelected = false;
            goldSelected = false;
        }
        else if (goldSelected)
        {
            greenSelected = false;
            redSelected = false;
            pinkSelected = false;
        }
        else
        {
            greenSelected = false;
            redSelected = false;
            pinkSelected = false;
            goldSelected = false;

            title.text = emptyText;
            info.text = selectMessage;
            cost.text = emptyText;
        }

    }

    private void CheckCamoPurchase()
    {
        if (greenSelected == true)
        {
            greenEquipped = true;
            StartCoroutine(ShowStateMessage());
        }

        if (redSelected == true)
        {
            redEquipped = true;
            StartCoroutine(ShowStateMessage());  
        }
        if (pinkSelected == true)
        {
            pinkEquipped = true;
            StartCoroutine(ShowStateMessage());
        }
        if (goldSelected == true)
        {
            goldEquipped = true;
            StartCoroutine(ShowStateMessage());
        }
    }
    private IEnumerator ShowErrorMessage()
    {

        buy.SetActive(false);
        title.text = emptyText;
        info.text = insufficientFunds;
        sniperCamo.sprite = invisSniper;
        cost.text = emptyText;
        yield return new WaitForSeconds(2f);
        title.text = initialTitle;
        info.text = initialInfo;
        cost.text = initialCost;
        sniperCamo.sprite = initialSniper;
        buy.SetActive(true);
    }

    private IEnumerator ShowStateMessage()
    {
        buy.SetActive(false);
        title.text = emptyText;
        info.text = camoState;
        sniperCamo.sprite = invisSniper;
        cost.text = emptyText;
        yield return new WaitForSeconds(2f);
        title.text = initialTitle;
        info.text = initialInfo;
        cost.text = initialCost;
        sniperCamo.sprite = initialSniper;
        buy.SetActive(true);
    }

    private IEnumerator ShowPurchaseMessage()
    {
        title.text = emptyText;
        info.text = camoPurchased;
        sniperCamo.sprite = invisSniper;
        cost.text = emptyText;
        buy.SetActive(false);
        yield return new WaitForSeconds(3f);
    }
}
