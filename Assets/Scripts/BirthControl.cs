using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthControl : MonoBehaviour
{
    public AutomataController automataController;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CellRow") {
            automataController.GenerateRow();
        }

    }
}
