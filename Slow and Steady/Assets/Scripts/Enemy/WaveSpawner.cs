using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;

    public List<Enemy> enemies = new List<Enemy>();
    public int currWave = -1; // Start before the first wave
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public Transform[] spawnLocation;
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    private static int enemiesInWave;
    private static int enemiesLeft;
    [SerializeField] private Animator waveAnim;
    [SerializeField] private Image waveBar;
    private float lerpSpeed;
    public AudioManager audioManager;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //GenerateWave();
        NextWave(); // Start the first wave
    }

    void FixedUpdate()
    {
        if (spawnTimer <= 0 && enemiesToSpawn.Count > 0)
        {
            SpawnEnemy();
        }

        spawnTimer -= Time.fixedDeltaTime;
        waveTimer -= Time.fixedDeltaTime;

        
        if (waveTimer <= 0)
        {
            // Check if all enemies are dead and the wave is over
            if (spawnedEnemies.Count == 0)
            {
                NextWave();
            }
        }
    }

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        enemiesLeft = EnemyManager.instance.GetCount();

        WaveBarFiller();
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnLocation.Length); // Get a random spawn point
        GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
        enemiesToSpawn.RemoveAt(0);
        spawnedEnemies.Add(enemy);
        spawnTimer = spawnInterval;
    }

    void WaveBarFiller()
    {
        waveBar.fillAmount = Mathf.Lerp(waveBar.fillAmount, (float)enemiesLeft / enemiesInWave, lerpSpeed);
    }

    void NextWave()
    {
        enemiesInWave = 0;
        //EnemyManager.instance.SetCount(enemiesInWave);
        currWave++; // Increase the wave index
        if (currWave >= enemies.Count) // If all waves are completed, restart or stop spawning
        {
            // Stop spawning or restart from the first wave
            // currWave = 0; // Uncomment this line to loop back to the first wave
            return; // Uncomment this line to stop spawning
        }

        if (currWave < 3)
        {
            waveAnim.SetTrigger("Start");
            if (currWave != 0)
            {
                audioManager.PlaySFX(audioManager.newWave, 0.2f);
            }
        }

        GenerateWave();

        EnemyManager.instance.SetCount(enemiesInWave);
    }

    void GenerateWave()
    {
        waveTimer = waveDuration; // Reset the wave timer for the new wave

        // Clear the lists for the new wave
        enemiesToSpawn.Clear();
        spawnedEnemies.RemoveAll(item => item == null); // Clean up any null references

        // Depending on the wave number, add specific enemies to the list
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hoard City"))
        { //manages wave spawn for level 1 
            /* 
            0 = tablet
            1 = tv
            2 = laptop
            3 = fridge
            4 = oven
            */
            switch (currWave)
            {
                case 0:
                    Debug.Log("LEVEL 1 SPAWNS W1");
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 1);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 1);
                    break;

                case 1:
                    Debug.Log("LEVEL 1 SPAWNS W2");
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 6);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    break;

                case 2:
                    Debug.Log("LEVEL 1 SPAWNS W3");
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 6);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 3);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    break;

                case 3:
                    Debug.Log("LEVEL 1 SPAWNS EX");
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4); // 2 enemies of type 1
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 3);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    break;

                default:
                    Debug.Log("Undefined wave number.");
                    break;
            }
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Best Buy2"))
        { //manages wave spawn for level 2
            /* 
            0 = tablet
            1 = tv
            2 = speaker
            3 = fridge
            4 = router
            5 = washing
            */
            switch (currWave)
            {
                case 0:
                    Debug.Log("LEVEL 2 SPAWNS W1");
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 1);
                    break;

                case 1:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 1);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 2);
                    break;

                case 2:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 6);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 5);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 2);
                    break;

                case 3:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4); // 2 enemies of type 1
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 6);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 2);
                    break;

                default:
                    Debug.Log("Undefined wave number.");
                    break;
            }
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Inside Warehouse"))
        { //manages wave spawn for level 3
            /* 
             0 = tablet
             1 = tv
             2 = speaker
             3 = fridge
             4 = laptop
             5 = oven
             6 = router
             7 = washing 
             */
            switch (currWave)
            {
                case 0:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 2);
                    break;

                case 1:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 6);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 3);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 1);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 1);
                    AddEnemiesToSpawn(enemies[6].enemyPrefab, 1);
                    AddEnemiesToSpawn(enemies[7].enemyPrefab, 1);
                    break;

                case 2:
                    AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                    AddEnemiesToSpawn(enemies[1].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[2].enemyPrefab, 4);
                    AddEnemiesToSpawn(enemies[3].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[4].enemyPrefab, 3);
                    AddEnemiesToSpawn(enemies[5].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[6].enemyPrefab, 2);
                    AddEnemiesToSpawn(enemies[7].enemyPrefab, 2);
                    break;

                case 3:

                    break;

                default:
                    Debug.Log("Undefined wave number.");
                    break;
            }
        }


        if (enemiesToSpawn.Count != 0)
        {
            spawnInterval = waveDuration / enemiesToSpawn.Count; // Calculate the time between each spawn
        }
        else
        {
            return;
        }

    }

    void AddEnemiesToSpawn(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemiesToSpawn.Add(enemyPrefab);
            enemiesInWave++;
        }
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
