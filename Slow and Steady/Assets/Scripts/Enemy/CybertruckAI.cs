using UnityEngine;
using System.Collections.Generic;
using Pathfinding;


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
    private Animator animator;
    private Seeker seeker;
    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float nextWaypointDistance = 3f;
    public float detectionRange = 50f;
    public float roamRadius = 10f;
    private SpriteRenderer spriteRenderer;

    private enum BossState { Roaming, Chasing, Stage1, Stage2, Stage3, Dead }
    private BossState currentState = BossState.Roaming;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shootingTimer = shootingCooldown;
        spawnTimer = spawnCooldown;
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (currentState == BossState.Dead || target == null) return;

        if (currentState == BossState.Roaming || currentState == BossState.Stage1)
        {
            if (roamPosition == Vector3.zero || Vector2.Distance(transform.position, roamPosition) < nextWaypointDistance)
            {
                roamPosition = GetRoamingPosition();
            }
            seeker.StartPath(rb.position, roamPosition, OnPathComplete);
        }
        else if (currentState == BossState.Chasing || currentState == BossState.Stage2 || currentState == BossState.Stage3)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startingPosition;
        return new Vector3(randomDirection.x, randomDirection.y);
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
        HandleMovement();
        HandleShooting();
        HandleSpawning();
    }

    private void HandleMovement()
    {
        if (path == null || currentState == BossState.Dead) return;

        reachedEndOfPath = currentWayPoint >= path.vectorPath.Count;
        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized * speed;
            rb.velocity = direction;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWaypointDistance)
            {
                currentWayPoint++;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    void HandleStateTransitions()
    {
        if (enemyHealth == null || isDead) return;

        int health = enemyHealth.currentHealth;
        int maxHealth = enemyHealth.maxHealth;


        if (health <= 0 && currentState != BossState.Dead)
        {
            EnterStageDead();
        }
        else if (health <= (maxHealth * 0.5) && currentState != BossState.Stage3 && currentState != BossState.Dead)
        {
            EnterStage3();
        }
        else if (health <= (maxHealth * 0.75) && currentState == BossState.Stage1)
        {
            EnterStage2();
        }
        else
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget <= detectionRange && currentState != BossState.Chasing)
            {
                currentState = BossState.Chasing;

            }
            else if (distanceToTarget > detectionRange && currentState != BossState.Roaming)
            {
                currentState = BossState.Roaming;

            }
        }
    }
    void EnterStage2()
    {
        currentState = BossState.Stage2;
        shootingCooldown = 1.5f;
        speed = 10;
        spawnCooldown = 2f;
        laserSpeed = 35f;
        shouldSpawnEnemies = true;
        //  animator.SetTrigger("stage1");
    }

    void EnterStage3()
    {
        currentState = BossState.Stage3;
        spawnCooldown = 1f;
        shouldSpawnEnemies = true;
        shootingCooldown = 1.5f;
        speed = 15;
        laserSpeed = 40f;
        animator.SetTrigger("stage2");
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


            if (enemiesToSpawn > 0)
            {
                SpawnEnemies(enemiesToSpawn);
                spawnTimer = spawnCooldown;
                shouldSpawnEnemies = false;
            }
        }
    }

    void SpawnEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (spawnPoints.Length == 0) return;

            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Instantiate(selectedEnemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage);

    }
    private void FixedUpdate()
    {
        if (path == null || currentState == BossState.Dead)
            return;

        reachedEndOfPath = currentWayPoint >= path.vectorPath.Count;
        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);

            if (direction.x > 0f)
                spriteRenderer.flipX = false;
            else if (direction.x < 0f)
                spriteRenderer.flipX = true;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWaypointDistance)
                currentWayPoint++;
        }

        // Check for state transition
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, target.position);
            if (distanceToPlayer <= detectionRange)
                currentState = BossState.Chasing;
            else if (currentState == BossState.Chasing && distanceToPlayer > detectionRange)
                currentState = BossState.Roaming;
        }
    }


}