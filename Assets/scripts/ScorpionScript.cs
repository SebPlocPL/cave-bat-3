using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ScorpionScript : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private bool attking;
    private Animator anmtr;

    private void Start()
    {
        if (SaveManager.saveGlob.level > 4)
        {
            if (Random.Range(0, 2) == 0)
            {
                Destroy(gameObject);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anmtr = GetComponent<Animator>();
        if (player == null)
        {
            Debug.LogError(
                "Player not found in the scene! Make sure to tag the player object with 'Player'."
            );
        }
        StartCoroutine(FindRtn());
    }

    IEnumerator FindRtn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.15f));
            if (Vector3.Distance(transform.position, player.position) < 3f)
            {
                agent.SetDestination(player.position);
                anmtr.SetBool("walk", true);
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "Player" && !attking)
        {
            attking = true;
            transform.LookAt(other.transform);
            anmtr.SetTrigger("attack");
            StartCoroutine(AttkingOffRtn());
        }
    }

    IEnumerator AttkingOffRtn()
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<MobileObjectMovement>().TakeLife(5f);
        yield return new WaitForSeconds(0.5f);
        attking = false;
    }
}
