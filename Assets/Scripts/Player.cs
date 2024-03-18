using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,-3.98f,0);
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
=======
        movement();
        fire();
        jumping();
        CheckVulnerable();

        if (_gunType == gunType.machineGun) 
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(_machineGunPrefab, transform.position + new Vector3(0.4f,-0.1f,0), Quaternion.identity);
            currentGunPrefab.GetComponent<MachineGun>().setMachineGunCanBeCollected(false);
            currentGunPrefab.transform.SetParent(transform);
            currentGunPrefab.transform.localScale = spriteRenderer.flipX ?
                                                new Vector3(-0.2f, 0.2f, 1) :
                                                new Vector3(0.2f, 0.2f, 1);
            currentGunPrefab.transform.position = spriteRenderer.flipX ? 
                                                transform.position - new Vector3(0.4f, 0.1f, 0) : 
                                                transform.position + new Vector3(0.4f, -0.1f, 0);
        }

        if (_gunType == gunType.bombGun) 
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(_bombGunPrefab, transform.position + new Vector3(0.4f,-0.1f,0), Quaternion.identity);
            currentGunPrefab.GetComponent<BombGun>().setBombGunCanBeCollected(false);            
            currentGunPrefab.transform.SetParent(transform);
            currentGunPrefab.transform.localScale = spriteRenderer.flipX ?
                                                new Vector3(-0.2f, 0.2f, 1) :
                                                new Vector3(0.2f, 0.2f, 1);
            currentGunPrefab.transform.position = spriteRenderer.flipX ? 
                                                transform.position - new Vector3(0.4f, 0.1f, 0) : 
                                                transform.position + new Vector3(0.4f, -0.1f, 0);
        }

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
            TakeSpikeDamage(2f);
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && (!jump))
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _canFire)
        {
            if (_gunType == gunType.glock) {
                _fireRate = 0.6f;
            }
            if (_gunType == gunType.bombGun) {
                _fireRate = 1.3f;
            }
            if (_gunType == gunType.machineGun) {
                _fireRate = 0.07f;
            }
            
            _canFire = Time.time + _fireRate;

            float bulletDirection = spriteRenderer.flipX ? -1f : 1f;
        
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0.5f * bulletDirection, 0, 0), Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
            
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
            healthone -= damage;
            if (healthone <= 0)
            {
                if (!_alreadyScored)
                {
                    _secondPlayer.setScoreSP(_secondPlayer.getScoreSP() + 1);
                    _alreadyScored = true;
                }
            }
    }
    public void TakeSpikeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            healthone -= damage;
            if (healthone <= 0)
            {
                if (!_alreadyScored)
                {
                    _secondPlayer.setScoreSP(_secondPlayer.getScoreSP() + 1);
                    _alreadyScored = true;
                }
            }
            else
            {
                isInvulnerable = true;
                invulnerabilityTimer = Time.time;
            }
        }
    }

    public void GetHealth(float hp) { 
        
        healthone += hp;
    
        if (healthone > maxHealthone) {healthone = maxHealthone;}
    }

    private void movement()
    {
>>>>>>> Stashed changes
        float horizontalInput = Input.GetAxis("Horizontal");
        
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);

    }
}
