using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
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
        EnemyManager.instance.SetCount(enemiesInWave);
        currWave++; // Increase the wave index
        if (currWave >= enemies.Count) // If all waves are completed, restart or stop spawning
        {
            // Stop spawning or restart from the first wave
            // currWave = 0; // Uncomment this line to loop back to the first wave
            return; // Uncomment this line to stop spawning
        }

        if (currWave < 2)
        {
            waveAnim.SetTrigger("Start");
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
        switch (currWave)
        {
            case 0:
                AddEnemiesToSpawn(enemies[0].enemyPrefab, 5); // 2 enemies of type 0
                AddEnemiesToSpawn(enemies[1].enemyPrefab, 5);
                AddEnemiesToSpawn(enemies[2].enemyPrefab, 1);

                break;
            case 1:
                AddEnemiesToSpawn(enemies[0].enemyPrefab, 8);
                AddEnemiesToSpawn(enemies[1].enemyPrefab, 5); // 2 enemies of type 1
                AddEnemiesToSpawn(enemies[2].enemyPrefab, 4);
                AddEnemiesToSpawn(enemies[3].enemyPrefab, 4);

                break;
/*            case 2:
                // Mix of all enemy types
                AddEnemiesToSpawn(enemies[0].enemyPrefab, 6);
                AddEnemiesToSpawn(enemies[1].enemyPrefab, 6);
                AddEnemiesToSpawn(enemies[2].enemyPrefab, 8);
                AddEnemiesToSpawn(enemies[3].enemyPrefab, 4);
                break;*/

            case 2:
                AddEnemiesToSpawn(enemies[0].enemyPrefab, 5);
                AddEnemiesToSpawn(enemies[1].enemyPrefab, 5);
                break;
            default:
                Debug.Log("Undefined wave number.");
                break;
        }

        spawnInterval = waveDuration / enemiesToSpawn.Count; // Calculate the time between each spawn
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
