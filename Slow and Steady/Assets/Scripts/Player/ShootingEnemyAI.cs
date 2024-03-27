using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShootingEnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float shootSpeed = 30f;
    public float nextWaypointDistance = 3f;
    public float detectionRange = 50f;
    private Vector3 startingPosition;
    public float roamRadius = 10f;

    public GameObject projectilePrefab; // Projectile to shoot
    public Transform shootPoint; // Where projectiles are fired from
    public float shootingRange = 20f; // Range within which to start shooting
    public float shootingCooldown = 2f; // Time between shots

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 roamPosition;
    private float shootingTimer;
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
        projectilePrefab = GameObject.FindGameObjectWithTag("Bullet");

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        state = State.Roaming;
        roamPosition = GetRoamingPosition();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        shootingTimer = shootingCooldown; // Initialize the shooting timer
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
        if (state == State.Chasing)
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0f && Vector2.Distance(transform.position, target.position) <= shootingRange)
            {
                ShootProjectile();
                shootingTimer = shootingCooldown; // Reset the timer
            }
        }
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startingPosition;
        return new Vector3(randomDirection.x, randomDirection.y);
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        reachedEndOfPath = currentWayPoint >= path.vectorPath.Count;

        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);

            if (direction.x > 0f)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0f)
            {
                spriteRenderer.flipX = true;
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWaypointDistance)
            {
                currentWayPoint++;
            }
        }

        // Check for state transition
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, target.position);

            if (distanceToPlayer <= detectionRange)
            {
                state = State.Chasing;
            }
            else if (state == State.Chasing && distanceToPlayer > detectionRange)
            {
                state = State.Roaming;
                currentWayPoint = 0; // Reset path following
                roamPosition = GetRoamingPosition(); // Get a new roaming position
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

    void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Vector2 shootingDirection = (target.position - shootPoint.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = shootingDirection * shootSpeed;

            float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}