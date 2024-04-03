using System.Collections;
using UnityEngine;

public class SmoothFollowRotation : MonoBehaviour
{
    public float minRotation = -80f;
    public float maxRotation = 80f;
    public bool followRotation;

    [SerializeField]
    Camera camScrpt;

    [SerializeField]
    Transform target;

    [SerializeField]
    float rotationSpeed = 5f;

    void Start()
    {
        followRotation = true;
        StartCoroutine(FollowRtn());
    }

    IEnumerator FollowRtn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (followRotation && target != null)
            {
                transform.position = target.position;
                // Calculate the desired rotation based on the target's rotation
                Quaternion targetRotation = Quaternion.LookRotation(target.forward, target.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    public void FollowRotOnOff(bool how)
    {
        followRotation = how;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if (followRotation)
            {
                if (camScrpt.depth == 0)
                {
                    StopCoroutine(FollowOnRoutine());
                    StartCoroutine(FollowOnRoutine());
                }
            }
            else
            {
                camScrpt.depth = 0;
            }
        }

        // Get the current rotation of the object
        Quaternion currentRotation = transform.rotation;

        // Convert the rotation to a normalized Euler representation
        Vector3 eulerRotation = currentRotation.eulerAngles;

        // Normalize the X-axis rotation to the range of -180 to 180 degrees
        eulerRotation.x = (eulerRotation.x > 180f) ? eulerRotation.x - 360f : eulerRotation.x;

        // Clamp the X-axis rotation to the specified limits
        eulerRotation.x = Mathf.Clamp(eulerRotation.x, minRotation, maxRotation);

        // Convert the Euler rotation back to a Quaternion
        Quaternion clampedRotation = Quaternion.Euler(eulerRotation);

        // Apply the clamped rotation to the object
        transform.rotation = clampedRotation;
    }

    IEnumerator FollowOnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        camScrpt.depth = -2;
    }
}
