using System.Collections;
using UnityEngine;

public class HorseMove : MonoBehaviour
{
    [SerializeField]
    Animator aniCntrlr;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private float movementSpeed = 3f;

    [SerializeField]
    private float rotationSpeed = 5f;

    private int currentWaypointIndex = 0;
    public bool isMoving = true;
    private float movementTimer = 0f;
    private float speedChangeTimer = 0f;
    private float originalSpeed = 0f;

    private void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }

        originalSpeed = movementSpeed;
        StartMovementTimer();
        StartSpeedChangeTimer();
        StartCoroutine(CheckWaypointRtn());
    }

    IEnumerator CheckWaypointRtn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.99f);
            Vector3 directionToTarget =
                waypoints[currentWaypointIndex].position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < (0.5f * 0.5f))
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        if (isMoving)
        {
            MoveToWaypoint();
        }

        UpdateSpeedChange();
    }

    private void MoveToWaypoint()
    {
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    public void HorseStartWalking()
    {
        if (!isMoving)
        {
            movementSpeed = originalSpeed;
            speedChangeTimer = Random.Range(2f, 6f);
            aniCntrlr.SetBool("walk", true);
        }
    }

    private void UpdateSpeedChange()
    {
        speedChangeTimer -= Time.deltaTime;

        if (speedChangeTimer <= 0f)
        {
            if (isMoving)
            {
                movementSpeed = 0f;
                speedChangeTimer = Random.Range(6f, 10f);
                aniCntrlr.SetBool("walk", false);
            }
            else
            {
                movementSpeed = originalSpeed;
                speedChangeTimer = Random.Range(2f, 6f);
                aniCntrlr.SetBool("walk", true);
            }

            isMoving = !isMoving;
        }
    }

    private void StartMovementTimer()
    {
        movementTimer = Random.Range(3f, 10f);
    }

    private void StartSpeedChangeTimer()
    {
        speedChangeTimer = Random.Range(2f, 6f);
    }
}
