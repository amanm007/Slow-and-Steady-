using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class RouterAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float detectionRange = 10f;
    private Vector3 startingPosition;
    public float roamRadius = 10f;
    public float spawnCooldown = 5f;
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public int enemiesToSpawn = 3; // Number of enemies to spawn

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 roamPosition;
    private float spawnTimer;
    private State state;

    private SpriteRenderer spriteRenderer;
    private enum State
    {
        Roaming,
        Chasing
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        state = State.Roaming;
        roamPosition = GetRoamingPosition();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        spawnTimer = spawnCooldown; // Initialize the spawn timer
    }

  
    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startingPosition;
        return new Vector3(randomDirection.x, randomDirection.y);
    }

    private void FixedUpdate()
    {
        if (path == null) return;

        reachedEndOfPath = currentWayPoint >= path.vectorPath.Count;

        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);

            if (direction.x > 0f) spriteRenderer.flipX = false;
            else if (direction.x < 0f) spriteRenderer.flipX = true;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWaypointDistance) currentWayPoint++;
        }

        // Check for state transition
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, target.position);

            if (distanceToPlayer <= detectionRange) state = State.Chasing;
            else if (state == State.Chasing && distanceToPlayer > detectionRange)
            {
                state = State.Roaming;
                currentWayPoint = 0; // Reset path following
                roamPosition = GetRoamingPosition(); // Get a new roaming position
            }
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            switch (state)
            {
                case State.Roaming:
                    float distanceToRoamPosition = Vector2.Distance(transform.position, roamPosition);
                    if (distanceToRoamPosition < nextWaypointDistance)
                    {
                        roamPosition = GetRoamingPosition();
                    }
                    seeker.StartPath(rb.position, roamPosition, OnPathComplete);
                    break;
                case State.Chasing:
                    seeker.StartPath(rb.position, target.position, OnPathComplete);
                    break;
            }
        }
    }
    private void Update()
    {
        if (state == State.Chasing && target != null)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && Vector2.Distance(transform.position, target.position) <= detectionRange)
            {
                SpawnEnemies();
                spawnTimer = spawnCooldown; // Reset the timer
            }
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
    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Choose a random enemy prefab to spawn
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            // Spawn the enemy near the router's position
            Vector3 spawnPos = transform.position + (Random.insideUnitSphere * 4f); // 2f is the radius around the Router
            spawnPos.z = 0; // Ensure it spawns on the same plane
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

}
