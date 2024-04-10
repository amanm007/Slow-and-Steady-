using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [HideInInspector] public float maxHealth = 10f;
    [HideInInspector] public float health;


    //public GameObject otherObject;

    //public HealthBar healthBar;
    //  public Image[] healthpoints;
    public Image healthbar;
    float lerpSpeed;
    public SpriteRenderer spriteRenderer;
    // AudioManager audioManager;

    //[SerializeField] private Animator deathAnim;
    private bool isDead;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("maxHealth"))
        {
            maxHealth = PlayerPrefs.GetFloat("maxHealth");
        }

        SetHealth(maxHealth);
        // healthBar.SetMaxHealth(maxHealth);
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        HealthBarFiller();
        Debug.Log(maxHealth);

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Factory"))
        {
/*            lerpSpeed = 3f * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            HealthBarFiller();*/
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
        isDead = true;
        if (healthbar != null)
        {
            healthbar.fillAmount = 0;
        }
        //Invoke("ReloadScene", 2f);
        if(isDead == true)
        {
            StartCoroutine(DeathScreen());
        }
    }

    private IEnumerator DeathScreen()
    {
        isDead = false;
        Debug.Log("dead");
        //deathAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneController.instance.NextLevel("Factory");
        yield return new WaitForSeconds(1f);
        //deathAnim.SetTrigger("End");

        // Destroy the other object if it exists
/*        Destroy(gameObject);
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject); // Destroy the child object
        }*/
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

    public void SetMaxHealth(float upgradeValue)
    {
        maxHealth += upgradeValue;
        PlayerPrefs.SetFloat("maxHealth", maxHealth);
    }

    public void SetHealth(float value)
    {
        health = value;
    }
}