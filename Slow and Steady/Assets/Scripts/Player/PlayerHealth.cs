using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 10;
    //public GameObject otherObject;

     //public HealthBar healthBar;
    //  public Image[] healthpoints;
    public Image healthbar;
    float lerpSpeed;
    public SpriteRenderer spriteRenderer;
    // AudioManager audioManager;




    void Start()
    {
        health = maxHealth;
       // healthBar.SetMaxHealth(maxHealth);
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Factory"))
        {
            lerpSpeed = 3f * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            HealthBarFiller();
        }
    }

    void HealthBarFiller()
    {
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, (float)health / maxHealth, lerpSpeed);

        // for (int i=0; i < healthpoints.Length; i++)
        // {
        //    healthpoints[i].enabled = !DisplayHealthPoint(health, i);

        // }


    }


    bool DisplayHealthPoint(float health, float pointNumber)
    {
        return ((pointNumber * 3) >= health);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        //  audioManager.PlaySFX(audioManager.hurt);

        StartCoroutine(FlashColor()); // Flash the player sprite
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }
    IEnumerator FlashColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    private void Die()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = 0;
        }
        // Destroy the other object if it exists
        Destroy(gameObject);
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject); // Destroy the child object
        }
        //Invoke("ReloadScene", 2f);

        SceneController.instance.NextLevel("Factory");

    }
    public void IncreaseHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        // Update health bar and other related UI elements if necessary
    }
    private void ReloadScene()
    {
        Debug.Log("Reloading scene now");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}