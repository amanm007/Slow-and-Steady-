using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private int enemy;
    [SerializeField] private TMP_Text enemyDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnGUI()
    {
        enemyDisplay.text = enemy.ToString();
    }

    public void SetCount(int inital)
    {
        enemy += inital;
    }
    public void ChangeCount(int amount)
    {
        enemy -= amount;
    }

    public int GetCount()
    {
        return enemy;
    }
}
