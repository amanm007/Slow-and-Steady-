using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Animator tutorialAnim;
    // Start is called before the first frame update
    private bool tutorialShown = false;

    private void Update()
    {
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        if (LevelManager.instance.playerLevelCompletion == 0 && tutorialShown == false)
        {
            Cursor.visible = true;
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
