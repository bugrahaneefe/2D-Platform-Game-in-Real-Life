using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsController : MonoBehaviour
{
    [SerializeField]
    private GameObject _horizontalPlatform;    
    private float _movementRange = 3f; 
    private float _movementSpeed = 1.5f; 
    private Vector3 _initialPosition;

    void Start()
    {
        _horizontalPlatform.transform.position = new Vector3(-6.26f,0,0);
        _initialPosition = _horizontalPlatform.transform.position;
    }

    void Update()
    {
        float horizontalMovement = Mathf.Sin(Time.time * _movementSpeed) * _movementRange;

        Vector3 newPosition = _initialPosition + Vector3.right * horizontalMovement;
        _horizontalPlatform.transform.position = newPosition;
    }
}
