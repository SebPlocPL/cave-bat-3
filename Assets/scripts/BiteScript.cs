using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteScript : MonoBehaviour
{
    public List<GameObject> collidingObjects;

    [SerializeField]
    VampireScript vampScrpt;

    [SerializeField]
    GoblinScript goblinScrpt;

    [SerializeField]
    MobileObjectMovement mblObjtMvmntSxrpt;

    private void Start()
    {
        // Initialize the list
        collidingObjects = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (transform.parent.gameObject.tag == "Player")
            {
                if (other.CompareTag("fly"))
                {
                    Destroy(other.transform.gameObject);
                    mblObjtMvmntSxrpt.Eat("fly");
                }
                if (other.CompareTag("egg"))
                {
                    Destroy(other.transform.gameObject);
                    mblObjtMvmntSxrpt.Eat("egg");
                }
                if (other.CompareTag("bird") && !mblObjtMvmntSxrpt.birdAttking)
                {
                    mblObjtMvmntSxrpt.Eat("bird");
                }
                if (other.CompareTag("scorpion"))
                {
                    Destroy(other.transform.gameObject);
                    mblObjtMvmntSxrpt.Eat("scorpion");
                }
                if (other.CompareTag("mouse"))
                {
                    Destroy(other.transform.gameObject);
                    mblObjtMvmntSxrpt.Eat("mouse");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            if (!collidingObjects.Contains(other.gameObject))
            {
                collidingObjects.Add(other.gameObject);
            }
            if (transform.parent.gameObject.tag == "Player")
            {
                if (other.CompareTag("horse"))
                {
                    mblObjtMvmntSxrpt.DrinkHorseBlood();
                }
                if (!mblObjtMvmntSxrpt.attacking)
                {
                    if (other.CompareTag("vamp") || other.CompareTag("goblin"))
                    {
                        mblObjtMvmntSxrpt.attacking = true;
                        mblObjtMvmntSxrpt.VampAttack(other.gameObject);
                    }
                }
            }
            else if (transform.parent.gameObject.tag == "vamp")
            {
                if (!vampScrpt.attacking)
                {
                    if (other.CompareTag("Player"))
                    {
                        vampScrpt.attacking = true;
                        vampScrpt.VampAttack();
                    }
                }
            }
            else if (transform.parent.gameObject.tag == "goblin")
            {
                if (!goblinScrpt.attacking)
                {
                    if (other.CompareTag("Player"))
                    {
                        goblinScrpt.attacking = true;
                        goblinScrpt.GoblinAttack();
                    }
                }
            }
        }
    }

    // Function to be called when an object exits the collision
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            // If the other object is in the list, remove it
            if (collidingObjects.Contains(other.gameObject))
            {
                collidingObjects.Remove(other.gameObject);
            }
        }
    }
}
