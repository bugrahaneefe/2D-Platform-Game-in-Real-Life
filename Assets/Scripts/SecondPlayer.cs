using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class SecondPlayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _bulletmaterialPrefab;
    private float _canFire = -1f;
    private float _fireRate = 0.2f;

    private float _bulletAngle = 0.0f;
    private float angleAdjustmentSpeed = 5f;
    private bool jump;
    public float healhttwo, maxHealthtwo;
    [SerializeField]
    private AudioClip _gunShotAudioSource;
    [SerializeField]
    private AudioClip _bombGunShotAudioSource;
    private AudioSource _audioSource;
    private gunType _gunType;
    
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 2f;
    private float invulnerabilityTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(9.09f,-3.0f,0);
        _audioSource = GetComponent<AudioSource>();
       setGunTypeForPlayer(gunType.glock);
    }

    void Update()
    {
        movement();
        fire();
        jumping();
        CheckVulnerable();
    }

    private void CheckVulnerable()
    {
        if (isInvulnerable)
        {
            float blinkInterval = 0.2f;
            float timeSinceLastBlink = Time.time - invulnerabilityTimer;
            bool isVisible = timeSinceLastBlink % (blinkInterval * 2) < blinkInterval;
            spriteRenderer.enabled = isVisible;

            if (Time.time >= invulnerabilityTimer + invulnerabilityDuration)
            {
                spriteRenderer.enabled = true;
                isInvulnerable = false;
            }
        }
    }

    private void jumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.58f);
        if (hit.collider != null) { jump = false; }

        if (hit.collider != null && hit.collider.CompareTag("Spike"))
        {
            TakeDamage(2f);
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.I) && (!jump))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 6f, 0);
            jump = true;
        }
    }

    private void fire()
    {
        // Will be changed according to gyroscope inputs
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        _bulletAngle += scrollWheelInput * angleAdjustmentSpeed;
        if (Input.GetKeyDown(KeyCode.F) && Time.time > _canFire)
        {
            if (_gunType == gunType.glock) {
                _fireRate = 0.5f;
            }
            if (_gunType == gunType.bombGun) {
                _fireRate = 1f;
            }
            if (_gunType == gunType.machineGun) {
                _fireRate = 0.15f;
            }
            
            _canFire = Time.time + _fireRate;

            float bulletDirection = spriteRenderer.flipX ? -1f : 1f;
        
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(1f * bulletDirection, 0, 0), Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
            
            if (_gunType == gunType.bombGun) {
                bullet.GetComponent<Bullet>().SetBulletSize(4);
                _audioSource.PlayOneShot(_bombGunShotAudioSource, 0.9f);
            } else {
                bullet.GetComponent<Bullet>().SetBulletSize(1);
                _audioSource.PlayOneShot(_gunShotAudioSource, 0.5f);
            }

            bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
            bullet.GetComponent<Bullet>().SetGunType(getGunType());


            for (int i = 0; i < 5; i++)
            {
                Instantiate(_bulletmaterialPrefab, transform.position + new Vector3(0.3f * bulletDirection, 0, 0), Quaternion.identity);
            }
        }
    }

    public void setGunTypeForPlayer(gunType gunType)
    {
        switch (gunType)
        {
            case gunType.glock:
                _gunType = (gunType)1;
                break;
            case gunType.bombGun:
                _gunType = (gunType)2;
                break;
            case gunType.machineGun:
                _gunType = (gunType)3;
                break;
        }
    }

    public gunType getGunType() {
        return _gunType;
    }

    public void TakeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            healhttwo -= damage;
            if (healhttwo <= 0)
            {
                Debug.Log("Player2 is dead!");
            }
            else
            {
                isInvulnerable = true;
                invulnerabilityTimer = Time.time;
            }
        }
    }

    public void GetHealth(float hp) { 
        
        healhttwo += hp;
    
        if (healhttwo > maxHealthtwo) {healhttwo = maxHealthtwo;}
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal2");
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 scale = transform.localScale;
            scale.y = 0.5f;
            transform.localScale = scale;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            Vector3 scale = transform.localScale;
            scale.y = 1.0f;
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
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorizontalPlatform"))
        {
            transform.parent = null;
        }
    }
}