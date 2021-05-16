using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathControl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CellRow")
        {
            Destroy(other.gameObject);
        }
    }
}
