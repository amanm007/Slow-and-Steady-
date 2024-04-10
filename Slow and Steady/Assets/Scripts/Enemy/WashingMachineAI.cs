using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class WashingMachineAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float detectionRange = 50f;
    private Vector3 startingPosition;
    public float roamRadius = 10f;
    public float shootSpeed = 5f;

    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootingRange = 20f;
    public float shootingCooldown = 5f;

    public LineRenderer aimLineRenderer;
    public LineRenderer shootLineRenderer;
    public float lineDisplayTime = 1f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 roamPosition;
    private float shootingTimer;
    private bool isPreparingToShoot = false;
    private SpriteRenderer spriteRenderer;
    private Vector3 lockedPosition;
    private Vector3 lockedAimPosition; // For storing locked aim position


    private enum State { Roaming, Chasing }
    private State state;


    private void Start()
    {
        InitializeEnemy();

    }
    void InitializeEnemy()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        projectilePrefab = GameObject.FindGameObjectWithTag("Laser");

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        state = State.Roaming;
        roamPosition = GetRoamingPosition();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        shootingTimer = shootingCooldown; // Initialize the shooting timer
        aimLineRenderer.enabled = false;
        shootLineRenderer.enabled = false;

    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if ((target != null))
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
    }

    private void Update()
    {
        if (state == State.Chasing)
        {
            if ((target != null))
            {
                shootingTimer -= Time.deltaTime;
                if (shootingTimer <= 0f && Vector2.Distance(transform.position, target.position) <= shootingRange)
                {
                    lockedPosition = target.position;
                    StartCoroutine(PrepareAndShoot());
                    shootingTimer = shootingCooldown;
                }
                 shootingTimer -= Time.deltaTime;
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
                currentWayPoint = 0;
                roamPosition = GetRoamingPosition();
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



    IEnumerator PrepareAndShoot()
    {

        isPreparingToShoot = true;
       
        // Display aim line
        ShowAimLine(Color.red, 0.1f, lockedPosition);

        yield return new WaitForSeconds(lineDisplayTime);

        // Hide aim line
        aimLineRenderer.enabled = false;

        for (int i = 0; i < 3; i++)
        {
            ShootProjectile(lockedPosition);
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds between shots
        }

        isPreparingToShoot = false;
        shootingTimer = shootingCooldown;
    }
    void ShowAimLine(Color lineColor, float lineWidth, Vector3 lockedPosition)
    {
        aimLineRenderer.startColor = lineColor;
        aimLineRenderer.endColor = lineColor;
        aimLineRenderer.startWidth = lineWidth;
        aimLineRenderer.endWidth = lineWidth;
        aimLineRenderer.SetPosition(0, shootPoint.position);
        aimLineRenderer.SetPosition(1, lockedPosition);
        aimLineRenderer.enabled = true;
    }
    private void ShowAimLine(Vector2 direction)
    {
        aimLineRenderer.enabled = true;
        aimLineRenderer.SetPosition(0, shootPoint.position);
        aimLineRenderer.SetPosition(1, lockedAimPosition); // Use locked aim position
    }
    void ShootProjectile(Vector3 lockedPosition)
    {
        Vector2 direction = (lockedPosition - shootPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        
        projectile.GetComponent<Rigidbody2D>().velocity = direction * shootSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(projectile, 3f); 
    }

}
    