using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Animator upgradeAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        upgradeAnim.SetTrigger("Open");
        Cursor.visible = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Cursor.visible = false;
        upgradeAnim.SetTrigger("Close");
    }
}
