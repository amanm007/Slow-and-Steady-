using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Animator cutscene;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(17f);
        cutscene.StopPlayback();
        SceneController.instance.NextLevel("Factory");
    }
}
