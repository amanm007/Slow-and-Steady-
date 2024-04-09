using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip shot, rechamber, playerDamage, enemyDamage, enemyDeath, newWave, 
        transitionIn, transitionOut, healthPickup, noteShot, keyShot;
    public AudioClip hubMusic, cityMusic, bestbuyMusic, factoryMusic;

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, 0.3f);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlaySFXDelayed(AudioClip clip, float delay)
    {
        sfxSource.clip = clip;
        sfxSource.PlayDelayed(delay);
    }

    public void Start()
    {
        sfxSource.clip = transitionIn;
        sfxSource.PlayDelayed(2.5f);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Shooting Range") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Factory"))
        {
            musicSource.clip = hubMusic;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Silicon Valley") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City"))
        {
            musicSource.clip = cityMusic;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2"))
        {
            musicSource.clip = bestbuyMusic;
        }
        musicSource.PlayDelayed(3.5f);
    }
    public void Update()
    {
        if (GameObject.Find("CRT TV").GetComponent<SlowMotionAbility>().isSlowMotionActive)
        {
            musicSource.pitch = 0.7f;
        }
        else
        {
            musicSource.pitch = 1f;
        }
    }
}
