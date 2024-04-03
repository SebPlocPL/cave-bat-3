using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public float moveAwaySpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float turnSpeed = 45.0f;
    private Rigidbody rb;
    private GameObject player;
    private Animator anim;
    private const float THRESHOLD_DISTANCE = 3.0f;
    private const float MIN_JUMP_INTERVAL = 2.0f;
    private const float ANIMATION_CHECK_INTERVAL = 0.11f;
    private float lastJumpTime = -2.0f;
    private float lastAnimationCheckTime = -0.3f;
    private float timePlayerWasLastClose = 0f; // New variable to keep track of the time

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the object with MouseScript.");
        }

        if (anim == null)
        {
            Debug.LogError("Animator component missing from the object with MouseScript.");
        }

        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (IsPlayerClose())
            {
                if (ShouldJump())
                {
                    Jump();
                    anim.SetTrigger("jump");
                }

                MoveAwayAndInstantlyLookAwayFromPlayer();

                timePlayerWasLastClose = Time.time; // Update the time whenever the player is close
            }
            else if (Time.time - timePlayerWasLastClose > 1.0f) // Check if it's been more than 1 second since the player was close
            {
                SlowlyTurnToPlayer();
            }

            if (ShouldCheckAnimationState())
            {
                UpdateAnimationState();
                lastAnimationCheckTime = Time.time;
            }
        }
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(player.transform.position, transform.position) < THRESHOLD_DISTANCE;
    }

    private bool ShouldJump()
    {
        return rb.velocity.magnitude < 0.1f && Time.time - lastJumpTime >= MIN_JUMP_INTERVAL;
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (Random.Range(0, 2) == 0)
        {
            rb.AddForce(transform.right * jumpForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-transform.right * jumpForce, ForceMode.Impulse);
        }
        lastJumpTime = Time.time;
    }

    private void MoveAwayAndInstantlyLookAwayFromPlayer()
    {
        Vector3 moveDirection = (transform.position - player.transform.position).normalized;
        rb.velocity = moveDirection * moveAwaySpeed;

        // Instantly look away from player, only around Y-axis
        float newYRotation = Quaternion.LookRotation(moveDirection).eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
    }

    private void SlowlyTurnToPlayer()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // Get the new Y rotation and keep X and Z at 0
        float newYRotation = Quaternion
            .RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime)
            .eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
    }

    private bool ShouldCheckAnimationState()
    {
        return Time.time - lastAnimationCheckTime >= ANIMATION_CHECK_INTERVAL;
    }

    private void UpdateAnimationState()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            anim.SetBool("run", false);
        }
        else
        {
            anim.SetBool("run", true);
        }
    }
}
