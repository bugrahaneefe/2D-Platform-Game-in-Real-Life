using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _bulletmaterialPrefab;
    private float _canFire = -1f;
    private float _fireRate = 0.5f;

    private float _bulletAngle = 0.0f;
    private float angleAdjustmentSpeed = 2f;
    private bool jump;

    void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        transform.position = new Vector3(-9.57f,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        fire();
        jumping();
    }

    private void jumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.58f);

        if (hit.collider != null) { jump = false; }

        if (Input.GetKeyDown(KeyCode.Space) && (!jump))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 6f, 0);
            jump = true;
        }
    }

    private void fire()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        _bulletAngle += scrollWheelInput * angleAdjustmentSpeed;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_bulletPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 0, _bulletAngle));
            for (int i = 0; i < 5; i++)
            {
                Instantiate(_bulletmaterialPrefab, transform.position + new Vector3(0.3f, 0, 0), Quaternion.identity);
            }
        }
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

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