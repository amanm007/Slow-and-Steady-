using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float detectionRange = 50f; // Range to start chasing the player
    private Vector3 startingPosition;
    public float roamRadius = 10f; // Adjust as needed

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 roamPosition;
    private State state;
  //  public float pathUpdateCooldown = 1f; // Time in seconds between path updates
  // private float pathUpdateTimer;

    private enum State
    {
        Roaming,
        Chasing
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        state = State.Roaming;
        roamPosition = GetRoamingPosition();
        InvokeRepeating("UpdatePath", 0f, 0.1f);
       // pathUpdateTimer = pathUpdateCooldown;
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
    /*
    private void Update()
    {
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0f)
        {
            UpdatePath();
            pathUpdateTimer = pathUpdateCooldown;
        }
    }
    */
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

        if (reachedEndOfPath)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        // Check for state transition
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);
        if (distanceToPlayer <= detectionRange)
        {
            state = State.Chasing;
        }
        else if (state == State.Chasing && distanceToPlayer > detectionRange)
        {
            // Optionally switch back to Roaming when the player is lost
            state = State.Roaming;
            currentWayPoint = 0; // Reset path following
            roamPosition = GetRoamingPosition(); // Get a new roaming position
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
}
