using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthControl : MonoBehaviour
{
    // when the row follower collides with the gate
    // a new row is generated
    // and the row follower is reset

    public AutomataController automataController;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Follower") {
            automataController.GenerateRow();
            rowFollower.transform.position = Vector3.zero;
        }

    }

    GameObject rowFollower;
    void Start()
    {
        rowFollower =  GameObject.Find("RowFollower");
    }

}
