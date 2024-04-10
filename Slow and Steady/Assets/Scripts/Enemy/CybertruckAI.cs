using UnityEngine;
using System.Collections.Generic;

public class CybertruckBossAI : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public List<GameObject> enemyPrefabs;
    public Transform[] spawnPoints; 
    public Transform[] headlightPositions; 
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float shootingCooldown = 1f;
    public float spawnCooldown = 3f;
    public float health = 100f;
    public float laserSpeed = 20f;

    private Rigidbody2D rb;
    private float shootingTimer;
    private float spawnTimer;
    private EnemyHealth enemyHealth;
    private bool isDead = false;
    public int enemiesToSpawnStage2 = 3;
    public int enemiesToSpawnStage3 = 5;
    private bool shouldSpawnEnemies = true;

    private enum BossState { Stage1, Stage2, Stage3, Dead }
    private BossState currentState = BossState.Stage1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shootingTimer = shootingCooldown;
        spawnTimer = spawnCooldown;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (isDead) return;

       
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found!");
            return;
        }

        
        HandleStateTransitions();

        if (target.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        HandleShooting();
        HandleSpawning();
    }

    void HandleStateTransitions()
    {
        int health = enemyHealth.currentHealth;
        int maxHealth = enemyHealth.maxHealth;

       
        if (health <= 0 && currentState != BossState.Dead)
        {
            EnterStageDead();
        }
        else if (health <= (maxHealth * 0.5) && currentState == BossState.Stage2)
        {
            EnterStage3();
        }
        else if (health <= (maxHealth * 0.75) && currentState == BossState.Stage1)
        {
            EnterStage2();
        }
    }
    void EnterStage2()
    {
        currentState = BossState.Stage2;
        shootingCooldown *= 1.5f;
        spawnCooldown = 2f;
        shouldSpawnEnemies = true; 
    }

    void EnterStage3()
    {
        currentState = BossState.Stage3;
        spawnCooldown = 1f;
        shouldSpawnEnemies = true; 
    }

    void EnterStageDead()
    {
        currentState = BossState.Dead;
        isDead = true;
        Destroy(gameObject); 
        
    }

    void HandleShooting()
    {
        if (currentState == BossState.Dead) return;

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            foreach (var headlightPosition in headlightPositions)
            {
                ShootProjectileFrom(headlightPosition);
            }
            shootingTimer = shootingCooldown;
        }
    }

    void ShootProjectileFrom(Transform shootPoint)
    {
        
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = (target.position - shootPoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(projectile, 3f);
    }

    void HandleSpawning()
    {
        // Early return if not the time to spawn or in the dead stage
        if (!shouldSpawnEnemies || currentState == BossState.Stage1 || currentState == BossState.Dead) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            int enemiesToSpawn = 0;

            if (currentState == BossState.Stage2)
            {
                enemiesToSpawn = enemiesToSpawnStage2;
            }
            else if (currentState == BossState.Stage3)
            {
                enemiesToSpawn = enemiesToSpawnStage3;
            }

            // Reset spawn timer and spawn enemies if needed
            if (enemiesToSpawn > 0)
            {
                SpawnEnemies(enemiesToSpawn);
                spawnTimer = spawnCooldown;
                shouldSpawnEnemies = false; // Prevent spawning again until conditions are reset
            }
        }
    }

    void SpawnEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (spawnPoints.Length == 0) return; // Check if there are spawn points

            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Choose a random spawn point
            var selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]; // Choose a random enemy prefab
            Instantiate(selectedEnemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage); 
        
    }
}
