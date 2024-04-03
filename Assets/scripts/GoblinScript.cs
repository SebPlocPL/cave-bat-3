using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinScript : MonoBehaviour
{
    [SerializeField]
    private PhysicMaterial iceMaterial;

    [SerializeField]
    private PhysicMaterial woodMaterial;

    [SerializeField]
    LayerMask playerMask;

    private CapsuleCollider capsuleCollider;
    private int life = 3;
    private Transform playerTransform;
    private bool isCollidingWithPlayer = false;
    private bool shouldFollow = false;

    [SerializeField]
    Animator aniCntrlr;

    [SerializeField]
    ParticleSystem blood;
    public bool attacking;

    [SerializeField]
    BiteScript biteScrpt;
    private bool turningToPlyr;

    // Add the NavMeshAgent component
    private NavMeshAgent navAgent;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckPlayerVisibilityRoutine());
        StartCoroutine(UpdateNavMeshDestination());
    }

    IEnumerator UpdateNavMeshDestination()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.31f);
            if (shouldFollow)
            {
                navAgent.SetDestination(playerTransform.position);
            }
        }
    }

    public void GoblinAttack()
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
        StartCoroutine(GoblinAttackRoutine());
    }

    IEnumerator GoblinAttackRoutine()
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
                StartCoroutine(BleedRoutine());
        }
    }

    IEnumerator BleedRoutine()
    {
        while (life > 0)
        {
            yield return new WaitForSeconds(0.2f);
            blood.Emit(3 - life);
        }
    }

    private IEnumerator TurnAndChangeMaterialRoutine()
    {
        capsuleCollider.material = iceMaterial;
        yield return TurnTowardsPlayerRoutine();
        capsuleCollider.material = woodMaterial;
        turningToPlyr = false;
    }

    private IEnumerator TurnTowardsPlayerRoutine()
    {
        float turnDuration = 0.5f;
        float turnInterval = 0.05f;
        float totalElapsedTime = 0f;

        Quaternion initialRotation = transform.rotation;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;

        while (totalElapsedTime < turnDuration && !shouldFollow)
        {
            totalElapsedTime += Time.deltaTime;

            float fractionOfTurn = totalElapsedTime / turnDuration;
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, fractionOfTurn);

            yield return new WaitForSeconds(turnInterval);
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator CheckPlayerVisibilityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.031f);

            Vector3 directionToPlayer =
                (playerTransform.position + Vector3.up) - (transform.position + Vector3.up);

            if (!playerTransform.GetComponent<MobileObjectMovement>().vampOn)
            {
                directionToPlayer = (playerTransform.position) - (transform.position + Vector3.up);
            }

            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (!shouldFollow && angle < 70f)
            {
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
                    Debug.DrawLine(
                        transform.position + Vector3.up,
                        hit.point + Vector3.up,
                        Color.red
                    );
                    if (hit.collider.CompareTag("Player"))
                    {
                        shouldFollow = true;
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
        Quaternion currentRotation = transform.rotation;
        transform.rotation = new Quaternion(0, currentRotation.y, 0, currentRotation.w);
        if (navAgent.velocity.magnitude > 0.1f)
        {
            aniCntrlr.SetBool("walk", true);
        }
        else
        {
            aniCntrlr.SetBool("walk", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
}
