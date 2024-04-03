using UnityEngine;

public class RandomFlyFlying : MonoBehaviour
{
    public float flyingSpeed = 5f; // Speed when flying within the radius
    public float fleeingSpeed = 10f; // Speed when fleeing from the player
    public float movementRadius = 5f; // Movement radius
    public float directionChangeIntervalMin = 0.5f; // Minimum interval to change direction
    public float directionChangeIntervalMax = 1f; // Maximum interval to change direction
    public float detectionRadius = 1f; // Radius to detect the player
    public string playerTag = "Player"; // Tag of the player object

    private Rigidbody rb;
    private bool isFleeing = false;
    private Transform playerTransform;
    private Vector3 startDestination;
    private Vector3 randomDirection;
    private float directionChangeTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        startDestination = transform.position;

        // Set the initial random direction
        randomDirection = GetRandomDirection();
        directionChangeTimer = Random.Range(directionChangeIntervalMin, directionChangeIntervalMax);
    }

    private void Update()
    {
        if (!isFleeing)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
            {
                Vector3 fleeDirection = transform.position - playerTransform.position;
                rb.AddForce(fleeDirection.normalized * fleeingSpeed, ForceMode.VelocityChange);
                isFleeing = true;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, playerTransform.position) > detectionRadius)
            {
                isFleeing = false;
                Vector3 returnDirection = startDestination - transform.position;
                rb.AddForce(returnDirection.normalized * flyingSpeed, ForceMode.VelocityChange);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isFleeing)
        {
            if (rb.velocity.magnitude < flyingSpeed)
            {
                Vector3 desiredDirection;
                directionChangeTimer -= Time.fixedDeltaTime;
                if (directionChangeTimer <= 0f)
                {
                    randomDirection = GetRandomDirection();
                    directionChangeTimer = Random.Range(
                        directionChangeIntervalMin,
                        directionChangeIntervalMax
                    );
                }

                desiredDirection = randomDirection;
                rb.AddForce(desiredDirection * flyingSpeed, ForceMode.VelocityChange);
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.Normalize();
        return randomDir;
    }

    private void LateUpdate()
    {
        // Keep the object within the movement radius
        Vector3 center = startDestination;
        if (Vector3.Distance(center, transform.position) > movementRadius)
        {
            Vector3 direction = center - transform.position;
            rb.AddForce(direction.normalized * (flyingSpeed * 0.5f), ForceMode.VelocityChange);
        }
    }
}
