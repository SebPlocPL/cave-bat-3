using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnCollision : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f; // Adjust this value to change the speed of the movement.

    public float targetZ = -0.281f; // The z position to move to when colliding with the "Default" layer.

    [SerializeField]
    Transform raycastTransform; // Transform from which the raycast will be shot.

    private Vector3 originalPositionBat;

    private bool hasCollided = false;
    private bool blocked;

    private void Start()
    {
        originalPositionBat = transform.localPosition;
        StartCoroutine(PerformRaycastRoutine());
    }

    IEnumerator PerformRaycastRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.12f);
            if (targetZ < 1f)
            {
                Vector3 raycastDirection = transform.position - raycastTransform.position;
                RaycastHit hit;
                if (Physics.Raycast(raycastTransform.position, raycastDirection, out hit))
                {
                    if (
                        hit.collider.transform != transform
                        && hit.collider.transform.tag != "scorpion"
                        && hit.collider.transform.tag != "fly"
                        && hit.collider.transform.tag != "bird"
                    )
                    {
                        blocked = true;
                    }
                    else
                    {
                        blocked = false;
                    }
                }
            }
            else
            {
                Vector3 raycastDirection =
                    transform.position - (raycastTransform.position + new Vector3(0f, 1.5f, 0f));
                RaycastHit hit;
                if (
                    Physics.Raycast(
                        (raycastTransform.position + new Vector3(0f, 1.5f, 0f)),
                        raycastDirection,
                        out hit
                    )
                )
                {
                    if (
                        hit.collider.transform != transform
                        && hit.collider.transform.tag != "scorpion"
                        && hit.collider.transform.tag != "fly"
                        && hit.collider.transform.tag != "bird"
                    )
                    {
                        blocked = true;
                    }
                    else
                    {
                        blocked = false;
                    }
                }
            }
            hasCollided = false;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        hasCollided = true;
    }

    private void Update()
    {
        if (blocked)
        {
            // Move towards the target position at the specified speed.
            Vector3 targetPosition = new Vector3(
                originalPositionBat.x,
                originalPositionBat.y,
                targetZ
            );
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
        else if (!hasCollided)
        {
            // Move back to the original position at the specified speed.
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                originalPositionBat,
                moveSpeed * Time.deltaTime
            );
        }
    }
}
