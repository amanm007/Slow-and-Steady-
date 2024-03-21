using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrapTextScript : MonoBehaviour
{
    Text text;
    public static int scrapAmount;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = scrapAmount.ToString();
    }
}
