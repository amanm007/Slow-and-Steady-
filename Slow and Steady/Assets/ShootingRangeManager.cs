using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeManager : MonoBehaviour
{
    [SerializeField] private Animator tutorialAnim;
    // Start is called before the first frame update
    private bool tutorialShown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        if (LevelManager.instance.playerLevelCompletion == 1 && tutorialShown == false)
        {
            Cursor.visible = true;
            Debug.Log("tutorial");
            tutorialAnim.SetTrigger("Start");
            tutorialShown = true;
            PlayerMovement.instance.pauseMovement = true;
        }
        else
        {
            return;
        }
    }

    public void CloseTutorialScreen()
    {
        Cursor.visible = false;
        tutorialAnim.SetTrigger("End");
        PlayerMovement.instance.pauseMovement = false;
    }
}
