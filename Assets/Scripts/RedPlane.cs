using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlane : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.5f; 

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
