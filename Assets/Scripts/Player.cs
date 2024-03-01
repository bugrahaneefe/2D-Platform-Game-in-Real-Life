using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private CharacterController _characterController;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _bulletPrefab;
    void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        transform.position = new Vector3(0,-3.98f,0);
    }

    // Update is called once per frame
    void Update()
    {
        movementCalculation();
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    private void movementCalculation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {

            transform.Translate(direction * _speed * Time.deltaTime);
        }

        if (transform.position.x >= 9.257071f)
        {
            transform.position = new Vector3(9.25707f, transform.position.y, 0);
        }
        if (transform.position.x <= -9.245456f)
        {
            transform.position = new Vector3(-9.24545f, transform.position.y, 0);
        }
        if (transform.position.y <= -3.98f)
        {
            transform.position = new Vector3(transform.position.x, -3.98f, 0);
        }
    }
}
