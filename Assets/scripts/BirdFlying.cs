using UnityEngine;

public class BirdFlying : MonoBehaviour
{
    public float forwardSpeed = 3f;
    public float detectionDistance = 1f;
    public float avoidObstacleDuration = 2f;
    public float flyForwardDurationMin = 5f;
    public float flyForwardDurationMax = 10f;
    public LayerMask obstacleLayer;
    public Transform player;

    private bool attack = false;
    private bool avoidingObstacles = false;
    private bool collidingWithPlayer = false;
    private float flyForwardDuration = 0f;
    private float attackCooldownTimer = 0f;

    private void Start()
    {
        SetRandomAttackTime();
    }

    private void Update()
    {
        if (attack)
        {
            if (collidingWithPlayer)
            {
                AttackPlayer();
            }
            else if (avoidingObstacles)
            {
                AvoidObstacles();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            MoveForward();
            DetectObstacles();
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            attack = true;
            SetRandomAttackTime();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void MoveTowardsPlayer()
    {
        flyForwardDuration -= Time.deltaTime;
        if (flyForwardDuration <= 0f)
        {
            avoidingObstacles = true;
            avoidObstacleDuration = 2f;
            flyForwardDuration = Random.Range(flyForwardDurationMin, flyForwardDurationMax);
        }
        else
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.LookAt(player);
            if (DetectObstaclesOnPath())
            {
                avoidingObstacles = true;
                avoidObstacleDuration = 2f;
                MoveUpwards();
            }
            else
            {
                MoveForward();
            }
        }
    }

    private void AvoidObstacles()
    {
        avoidObstacleDuration -= Time.deltaTime;
        if (avoidObstacleDuration <= 0f)
        {
            avoidingObstacles = false;
        }
        else
        {
            MoveUpwards();
        }
    }

    private void DetectObstacles()
    {
        if (!avoidingObstacles && DetectObstaclesOnPath())
        {
            avoidingObstacles = true;
            avoidObstacleDuration = 2f;
            MoveUpwards();
        }
    }

    private bool DetectObstaclesOnPath()
    {
        RaycastHit hit;
        if (
            Physics.Raycast(
                transform.position,
                transform.forward,
                out hit,
                detectionDistance,
                obstacleLayer
            )
        )
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void MoveUpwards()
    {
        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        avoidingObstacles = false;
        collidingWithPlayer = false;
        RotateRandomly();
        MoveForward();
        SetRandomAttackTime();
    }

    private void SetRandomAttackTime()
    {
        flyForwardDuration = Random.Range(flyForwardDurationMin, flyForwardDurationMax);
        attackCooldownTimer = flyForwardDuration;
    }

    private void RotateRandomly()
    {
        float randomAngleX = Random.Range(-180f, 180f);
        float randomAngleY = Random.Range(-180f, 180f);
        float randomAngleZ = Random.Range(-180f, 180f);
        transform.rotation = Quaternion.Euler(randomAngleX, randomAngleY, randomAngleZ);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attack = false;
            collidingWithPlayer = true;
        }
    }
}
