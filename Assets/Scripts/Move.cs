using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;

    void FixedUpdate()
    {
        transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
    }
}
