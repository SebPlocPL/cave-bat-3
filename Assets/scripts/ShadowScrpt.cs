using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScrpt : MonoBehaviour
{
    [SerializeField]
    Transform trgt;

    // Update is called once per frame
    void Update()
    {
        transform.position = trgt.position;
    }
}
