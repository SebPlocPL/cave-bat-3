using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireScript : MonoBehaviour
{
    [SerializeField]
    private PhysicMaterial iceMaterial;

    [SerializeField]
    private PhysicMaterial woodMaterial;

    [SerializeField]
    float rotateSpeed = 1f;

    [SerializeField]
    float moveSpeed = 1f;

    [SerializeField]
    float jumpForce = 5f;

    [SerializeField]
    float jumpInterval = 2f;

    [SerializeField]
    LayerMask playerMask; // You will need to set this in the inspector to include the Player
    private CapsuleCollider capsuleCollider;
    private int life = 3;
    private Transform playerTransform;
    private Rigidbody rb;
    private bool isCollidingWithPlayer = false;
    private float jumpTimer = 0f;
    private bool shouldFollow = false;

    [SerializeField]
    Animator aniCntrlr;

    [SerializeField]
    Projector lightProjctr;
    public bool attacking;

    [SerializeField]
    BiteScript biteScrpt;
    private bool turningToPlyr;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        capsuleCollider = GetComponent<CapsuleCollider>();
        // Start the coroutine
        StartCoroutine(CheckPlayerVisibilityRoutine());
    }

    public void VampAttack()
    {
        transform.LookAt(playerTransform, Vector3.up);
        if (Random.Range(0, 2) == 0)
        {
            aniCntrlr.SetTrigger("attack1");
        }
        else
        {
            aniCntrlr.SetTrigger("attack2");
        }
        StartCoroutine(VampAttackRtn());
    }

    IEnumerator VampAttackRtn()
    {
        yield return new WaitForSeconds(0.5f);
        if (biteScrpt.collidingObjects.Contains(playerTransform.gameObject))
        {
            if (playerTransform.GetComponent<MobileObjectMovement>().vampOn)
            {
                playerTransform.GetComponent<MobileObjectMovement>().TakeLife(34f);
            }
            else
            {
                playerTransform.GetComponent<MobileObjectMovement>().TakeLife(101f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    public void TakeLife()
    {
        life -= 1;
        if (life <= 0)
        {
            playerTransform.GetComponent<MobileObjectMovement>().TakeLife(-100f);
            if (SaveManager.saveGlob.level == 5)
            {
                playerTransform.GetComponent<MobileObjectMovement>().gmCntrlScrpt.EndLevel(true);
            }
            Destroy(gameObject);
        }
        else
        {
            if (!shouldFollow && !turningToPlyr)
            {
                turningToPlyr = true;
                StartCoroutine(TurnAndChangeMaterialRoutine());
            }
            if (life > 1)
                StartCoroutine(BlinkLightRtn());
        }
    }

    IEnumerator BlinkLightRtn()
    {
        while (life > 0)
        {
            lightProjctr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            lightProjctr.enabled = true;
            if (life == 2)
            {
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator TurnAndChangeMaterialRoutine()
    {
        // Change the material to ice
        capsuleCollider.material = iceMaterial;

        yield return TurnTowardsPlayerRoutine();

        // Change the material back to wood
        capsuleCollider.material = woodMaterial;
        turningToPlyr = false;
    }

    private IEnumerator TurnTowardsPlayerRoutine()
    {
        float turnDuration = 0.5f; // Total turning time
        float turnInterval = 0.05f; // Time interval between each step
        float totalElapsedTime = 0f; // Total elapsed time since the start of the routine

        Quaternion initialRotation = transform.rotation; // Initial rotation of the vampire

        // Get target rotation
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;

        while (totalElapsedTime < turnDuration && !shouldFollow)
        {
            totalElapsedTime += Time.deltaTime; // Increment total elapsed time

            // Compute a fraction that goes from 0 to 1
            float fractionOfTurn = totalElapsedTime / turnDuration;

            // Smoothly interpolate rotation
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, fractionOfTurn);

            yield return new WaitForSeconds(turnInterval);
        }

        // Ensure final rotation is correct
        transform.rotation = targetRotation;
    }

    private IEnumerator CheckPlayerVisibilityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.031f); // check every 0.031 seconds

            var mobileObjectMovement =
                playerTransform.gameObject.GetComponent<MobileObjectMovement>();
            if (mobileObjectMovement != null && mobileObjectMovement.vampOn)
            {
                // Check for player in front of this object up to 70 degrees from the z axis direction
                Vector3 directionToPlayer =
                    (playerTransform.position + Vector3.up) - (transform.position + Vector3.up);
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (!shouldFollow && angle < 70f)
                {
                    // Check with a raycast if the player is visible
                    RaycastHit hit;
                    if (
                        Physics.Raycast(
                            transform.position + Vector3.up,
                            directionToPlayer,
                            out hit,
                            Mathf.Infinity,
                            playerMask
                        )
                    )
                    {
                        // Draw debug line for the raycast
                        Debug.DrawLine(
                            transform.position + Vector3.up,
                            hit.point + Vector3.up,
                            Color.red
                        );

                        if (hit.collider.CompareTag("Player"))
                        {
                            // The player is visible. Now the vampire can turn to the player and follow
                            shouldFollow = true;
                        }
                    }
                }
            }
            else
            {
                shouldFollow = false;
            }
        }
    }

    private void Update()
    {
        // Ensure x and z rotation stay at 0, every frame
        Quaternion currentRotation = transform.rotation;
        transform.rotation = new Quaternion(0, currentRotation.y, 0, currentRotation.w);
    }

    private void FixedUpdate()
    {
        if (shouldFollow)
        {
            // Rotate towards player
            Vector3 direction = playerTransform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Keep rotation only around the Y-axis
            toRotation.x = 0;
            toRotation.z = 0;

            rb.MoveRotation(
                Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.fixedDeltaTime)
            );

            // Move towards player
            Vector3 moveDirection = playerTransform.position - transform.position;
            rb.MovePosition(
                transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime
            );

            // Jump if moving slower than or equal to a third of moveSpeed, not colliding with player, and at least 2 seconds have passed since last jump
            if (
                rb.velocity.magnitude <= moveSpeed / 3
                && !isCollidingWithPlayer
                && Time.time > jumpTimer
            )
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                jumpTimer = Time.time + jumpInterval;
                aniCntrlr.SetTrigger("jump");
            }
        }
    }

    private void FollowPlayer()
    {
        // Rotate towards player
        Vector3 direction = playerTransform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Keep rotation only around the Y-axis
        toRotation.x = 0;
        toRotation.z = 0;

        rb.MoveRotation(
            Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime)
        );

        // Move towards player
        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            if (!shouldFollow && !turningToPlyr)
            {
                turningToPlyr = true;
                StartCoroutine(TurnAndChangeMaterialRoutine());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
}
