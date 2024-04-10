using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Animator upgradeAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        upgradeAnim.SetTrigger("Start");
        UpgradeManager.instance.OpenMenu();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    public void CloseMenu()
    {
        upgradeAnim.SetTrigger("End");
        UpgradeManager.instance.CloseMenu();
    }
}
