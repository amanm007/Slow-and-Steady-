using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperCamoManager : MonoBehaviour
{
    public static SniperCamoManager instance;

    [SerializeField] private Sprite greenCamo, redCamo, pinkCamo, goldCamo, noCamo;
    private SpriteRenderer camoEquipped;

    public string currentCamoEquipped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        camoEquipped = GetComponent<SpriteRenderer>();
        currentCamoEquipped = PlayerPrefs.GetString("camo");
    }

    // Update is called once per frame
    void Update()
    {
        CheckCamoEquip();
    }

    private void CheckCamoEquip()
    {
        if(CamoManager.instance.greenEquipped == true)
        {
            currentCamoEquipped = "green";
        }
        else if (CamoManager.instance.redEquipped == true)
        {
            currentCamoEquipped = "red";
        }
        else if (CamoManager.instance.pinkEquipped == true)
        {
            currentCamoEquipped = "pink";
        }
        else if (CamoManager.instance.goldEquipped == true)
        {
            currentCamoEquipped = "gold";
        }
        PlayerPrefs.SetString("camo", currentCamoEquipped);

        SetSniperCamo();
    }

    private void SetSniperCamo()
    {
        if (currentCamoEquipped == "green")
        {
            camoEquipped.sprite = greenCamo;
        }
        else if (currentCamoEquipped == "red")
        {
            camoEquipped.sprite = redCamo;
        }
        else if (currentCamoEquipped == "pink")
        {
            camoEquipped.sprite = pinkCamo;
        }
        else if (currentCamoEquipped == "gold")
        {
            camoEquipped.sprite = goldCamo;
        }

        Debug.Log(currentCamoEquipped);
    }
}
