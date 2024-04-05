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

    public GameObject projectilePrefab; 
    public Transform shootPoint; 
    public float shootingRange = 20f; 
    public float shootingCooldown = 5f; 

    public LineRenderer pathLineRenderer; 
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

    private enum State { Roaming, Chasing }
    private State state;


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
        pathLineRenderer.enabled = false;
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
                    StartCoroutine(PrepareAndShoot());
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
        Vector2 direction = (target.position - shootPoint.position).normalized;

        // path
        pathLineRenderer.startColor = Color.red;
        pathLineRenderer.endColor = Color.red;
        pathLineRenderer.startWidth = 0.1f; 
        pathLineRenderer.endWidth = 0.1f; 
        pathLineRenderer.enabled = true;
        pathLineRenderer.SetPosition(0, shootPoint.position);
        pathLineRenderer.SetPosition(1, shootPoint.position + (Vector3)direction * shootingRange);

       
        yield return new WaitForSeconds(lineDisplayTime);

        pathLineRenderer.enabled = false;

        //shooting lrendere
        shootLineRenderer.startColor = Color.blue;
        shootLineRenderer.endColor = Color.blue;
        shootLineRenderer.startWidth = 0.3f; 
        shootLineRenderer.endWidth = 0.3f; 
        shootLineRenderer.enabled = true;
        shootLineRenderer.SetPosition(0, shootPoint.position);
        shootLineRenderer.SetPosition(1, shootPoint.position + (Vector3)direction * shootingRange);


        yield return new WaitForSeconds(1f); 
        shootLineRenderer.enabled = false;
    }

    void ShootProjectile(Vector2 direction)
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }


}