using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
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
    public float health, maxHealth;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(-9.57f,-3.0f,0);

    }

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

            float bulletDirection = spriteRenderer.flipX ? -1f : 1f;

            GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(1f * bulletDirection, 0, 0), Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));

            bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
            for (int i = 0; i < 5; i++)
            {
                Instantiate(_bulletmaterialPrefab, transform.position + new Vector3(0.3f * bulletDirection, 0, 0), Quaternion.identity);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        print(health);
        if (health <= 0)
        {
            Debug.Log("Player is dead!");
        }
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        if (direction.x < 0.0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0.0f)
        {
            spriteRenderer.flipX = false;
        }

        transform.Translate(direction * _speed * Time.deltaTime);
        setBoundaries();
        crouching();
    }

    private void crouching()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 scale = transform.localScale;
            scale.y = 0.5f; // Halve the scale of Y-axis
            transform.localScale = scale;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Vector3 scale = transform.localScale;
            scale.y = 1.0f; // Halve the scale of Y-axis
            transform.localScale = scale;
        }
    }

    private void setBoundaries()
    {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorizontalPlatform"))
        {
            transform.parent = collision.transform; // Attach player to the platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorizontalPlatform"))
        {
            // Detach player from the platform
            transform.parent = null;
        }
    }
}