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
    private GameObject _bombGunBulletPrefab;
    [SerializeField]
    private GameObject _awpBulletPrefab;
    [SerializeField]
    private GameObject _bulletmaterialPrefab;
    [SerializeField]
    private GameObject _fireEffectPrefab;
    [SerializeField]
    private GameObject _bloodPrefab;
    private float _canFire = -1f;
    private float _fireRate = 0.2f;

    private float _bulletAngle = 0.0f;
    private float angleAdjustmentSpeed = 5f;
    private bool jump;
    public float healhttwo, maxHealthtwo;
    [SerializeField]
    private AudioClip _jumpingAudioSource;
    [SerializeField]
    private AudioClip _pistolShotAudioSource;
    [SerializeField]
    private AudioClip _awpShotAudioSource;
    [SerializeField]
    private AudioClip _bombGunShotAudioSource;
    [SerializeField]
    private AudioClip _machineGunShotAudioSource;
    [SerializeField]
    private AudioClip _damagaTakenAudioSource;
    [SerializeField] private AudioClip _changeToGlockSound;
    [SerializeField] private AudioClip _changeToBombGunSound;
    [SerializeField] private AudioClip _changeToMachineGunSound;
        [SerializeField] private AudioClip _changeToAwpSound;

    private AudioSource _audioSource;
    private gunType _gunType;
    [SerializeField] private GameObject _primaryGunPrefab;
    [SerializeField] private GameObject _machineGunPrefab;
    [SerializeField] private GameObject _bombGunPrefab;
    [SerializeField] private GameObject _awpGunPrefab;
    private GameObject currentGunPrefab;
    
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 2f;
    private float invulnerabilityTimer = 0f;
    [SerializeField]
    private int _score;
    [SerializeField]
    private Player _player;

    // Arduino related variables.
    [SerializeField]
    private ArduinoController arduinoController;
    private float xInput;
    private float yInput;
    private float zInput;
    private float fireInput = 100;

    void Start()
    {
        arduinoController = GetComponent<ArduinoController>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(9.09f,-3.0f,0);
        _audioSource = GetComponent<AudioSource>();
       setGunTypeForPlayer(gunType.glock);
       currentGunPrefab = Instantiate(_primaryGunPrefab, transform.position - new Vector3(0.2f,0.1f,0), Quaternion.identity);
       currentGunPrefab.transform.localScale = new Vector3(-0.1f, 0.1f, 1); 
       currentGunPrefab.transform.SetParent(transform);
       _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        // Process data received from arduino
        //ProcessSerialData(arduinoController.LatestData);
        movement();
        fire();
        jumping();
        CheckVulnerable();

        if (_gunType == gunType.machineGun) 
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(_machineGunPrefab, transform.position + new Vector3(0.2f,-0.1f,0), Quaternion.identity);
            currentGunPrefab.GetComponent<MachineGun>().setMachineGunCanBeCollected(false);
            currentGunPrefab.transform.SetParent(transform);
            currentGunPrefab.transform.localScale = spriteRenderer.flipX ?
                                                new Vector3(-0.12f, 0.12f, 1) :
                                                new Vector3(0.12f, 0.12f, 1);
            currentGunPrefab.transform.position = spriteRenderer.flipX ? 
                                                transform.position - new Vector3(0.2f, 0.1f, 0) : 
                                                transform.position + new Vector3(0.2f, -0.1f, 0);
        }
        if (_gunType == gunType.bombGun) 
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(_bombGunPrefab, transform.position + new Vector3(0.4f,-0.1f,0), Quaternion.identity);
            currentGunPrefab.GetComponent<BombGun>().setBombGunCanBeCollected(false);            
            currentGunPrefab.transform.SetParent(transform);
            currentGunPrefab.transform.localScale = spriteRenderer.flipX ?
                                                new Vector3(-0.2f, 0.35f, 1) :
                                                new Vector3(0.2f, 0.35f, 1);
            currentGunPrefab.transform.position = spriteRenderer.flipX ? 
                                                transform.position - new Vector3(0.2f, 0.1f, 0) : 
                                                transform.position + new Vector3(0.2f, -0.1f, 0);
        }

        if (_gunType == gunType.awp) 
        {
            Destroy(currentGunPrefab);
            currentGunPrefab = Instantiate(_awpGunPrefab, transform.position + new Vector3(0.4f,-0.1f,0), Quaternion.identity);
            currentGunPrefab.GetComponent<Awp_silencer>().setAwpCanBeCollected(false);            
            currentGunPrefab.transform.SetParent(transform);
            currentGunPrefab.transform.localScale = spriteRenderer.flipX ?
                                                new Vector3(-0.2f, 0.35f, 1) :
                                                new Vector3(0.2f, 0.35f, 1);
            currentGunPrefab.transform.position = spriteRenderer.flipX ? 
                                                transform.position - new Vector3(0.2f, 0.1f, 0) : 
                                                transform.position + new Vector3(0.2f, -0.1f, 0);
        }
    }

    private void ProcessSerialData(string data)
{
    Debug.Log("Data received: " + data);
    string[] parts = data.Split(',');

    foreach (var part in parts)
    {
        if (part.StartsWith("Sensor:"))
        {
            string sensorPart = part.Substring(7); // Remove "Sensor:"
            if (float.TryParse(sensorPart, out float fireInput))
            {
                //Debug.Log("Sensor value: " + fireInput);
                if (fireInput < 60) {
                    fire();
                }
            }
            else
            {
                Debug.LogError("Failed to parse Sensor value.");
            }
        }
        else if (part.StartsWith("X:"))
        {
            string xPart = part.Substring(2); // Remove "X:"
            if (float.TryParse(xPart, out xInput))
            {
                //Debug.Log("X value: " + xInput);
            }
            else
            {
                Debug.LogError("Failed to parse X value.");
            }
        }
        else if (part.StartsWith("Y:"))
        {
            string yPart = part.Substring(2); // Remove "Y:"
            if (float.TryParse(yPart, out yInput))
            {
                Debug.Log("Y value: " + yInput);
            }
            else
            {
                Debug.LogError("Failed to parse Y value.");
            }
        }
        else if (part.StartsWith("Z:"))
        {
            string zPart = part.Substring(2); // Remove "Z:"
            if (float.TryParse(zPart, out zInput))
            {
                //Debug.Log("Z value: " + zInput);
            }
            else
            {
                Debug.LogError("Failed to parse Z value.");
            }
        }
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
            TakeSpikeDamage(1f);
            jump = false;
        }

        //Input.GetKeyDown(KeyCode.I)
        //yInput > 1f
        if (Input.GetKeyDown(KeyCode.I) && (!jump))
        {
            _audioSource.PlayOneShot(_jumpingAudioSource, 0.5f);
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 6f, 0);
            jump = true;
        }
    }

     private void fire()
    {
        float angleInput = Input.GetAxis("Mouse ScrollWheel");
        _bulletAngle += angleInput * angleAdjustmentSpeed;
        if (Input.GetKeyDown(KeyCode.P) && Time.time > _canFire)
        {
            if (_gunType == gunType.glock) {
                _fireRate = 0.8f;
                FireBullet(audioClip: _pistolShotAudioSource);
            } else if (_gunType == gunType.bombGun) {
                _fireRate = 1.7f;
                StartCoroutine(FireBombGunDelay());
            } else if (_gunType == gunType.machineGun) {
                _fireRate = 0.5f;
                FireMachineGunBurst();
            } else if (_gunType == gunType.awp) {
            _fireRate = 2.5f;
            FireBullet(audioClip: _awpShotAudioSource);
        }

            _canFire = Time.time + _fireRate;
        }
    }

    private IEnumerator FireBombGunDelay()
    {
        _audioSource.PlayOneShot(_bombGunShotAudioSource, 0.5f);
        yield return new WaitForSeconds(1.7f);
        FireBullet(size: 1f);
    }

    private void FireMachineGunBurst()
    {
        for (int i = 0; i < 4; i++) { 
            FireBullet(size: 0.6f, audioClip: _machineGunShotAudioSource);
        }
    }

    private void FireBullet(float size = 1, AudioClip audioClip = null)
    {
        float bulletDirection = spriteRenderer.flipX ? -1f : 1f;
        Vector3 bulletSpawnPosition = transform.position + new Vector3(0.5f * bulletDirection, 0, 0);
        
        Vector3 materialSpawnPosition = transform.position + new Vector3(0.05f * bulletDirection, 0, 0);
        Quaternion materialRotation = Quaternion.Euler(0, 0, -90);  
        if (_gunType == gunType.bombGun) {
            GameObject bullet = Instantiate(_bombGunBulletPrefab, bulletSpawnPosition + new Vector3(0,-0.08f,0), Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
            bullet.GetComponent<Bullet>().SetBulletSize(size);
            bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
            bullet.GetComponent<Bullet>().SetGunType(getGunType());
            bullet.transform.localScale = new Vector3(bulletDirection *  0.15f, 0.15f, 0.15f);
            ApplyKnockback(gunType.bombGun);
            GameObject fireEffectMaterial = Instantiate(_fireEffectPrefab, bulletSpawnPosition + new Vector3(0.1f * bulletDirection, -0.1f, 0), Quaternion.Euler(0, 0, 0));
            fireEffectMaterial.transform.localScale = new Vector3(bulletDirection *  0.5f, 0.5f, 0.5f);
        } else if (_gunType == gunType.machineGun){
            GameObject bullet = Instantiate(_bulletPrefab, bulletSpawnPosition, Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
            bullet.GetComponent<Bullet>().SetBulletSize(size);
            bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
            bullet.GetComponent<Bullet>().SetGunType(getGunType());

            GameObject bulletMaterial = Instantiate(_bulletmaterialPrefab, materialSpawnPosition, materialRotation);
            GameObject fireEffectMaterial = Instantiate(_fireEffectPrefab, bulletSpawnPosition + new Vector3(0.1f * bulletDirection, -0.02f, 0), Quaternion.Euler(0, 0, 0));
            fireEffectMaterial.transform.localScale = new Vector3(bulletDirection *  0.2f, 0.2f, 0.2f);
        } else if (_gunType == gunType.awp) {
        GameObject awpbullet = Instantiate(_awpBulletPrefab, bulletSpawnPosition + new Vector3(0,-0.08f,0), Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
        awpbullet.GetComponent<Bullet>().SetBulletSize(size);
        awpbullet.GetComponent<Bullet>().SetDirection(bulletDirection);
        awpbullet.GetComponent<Bullet>().SetGunType(getGunType());

        ApplyKnockback(gunType.awp);

        GameObject bulletMaterial = Instantiate(_bulletmaterialPrefab, materialSpawnPosition, materialRotation);
        GameObject fireEffectMaterial = Instantiate(_fireEffectPrefab, bulletSpawnPosition + new Vector3(0.25f * bulletDirection, -0.02f, 0), Quaternion.Euler(0, 0, 0));
        fireEffectMaterial.transform.localScale = new Vector3(bulletDirection *  0.3f, 0.3f, 0.3f);
    }
        else {
        GameObject bullet = Instantiate(_bulletPrefab, bulletSpawnPosition, Quaternion.Euler(0, 0, _bulletAngle * bulletDirection));
        bullet.GetComponent<Bullet>().SetBulletSize(size);
        bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
        bullet.GetComponent<Bullet>().SetGunType(getGunType());

        GameObject bulletMaterial = Instantiate(_bulletmaterialPrefab, materialSpawnPosition, materialRotation);
        GameObject fireEffectMaterial = Instantiate(_fireEffectPrefab, bulletSpawnPosition + new Vector3(0.02f * bulletDirection, -0.02f, 0), Quaternion.Euler(0, 0, 0));
        fireEffectMaterial.transform.localScale = new Vector3(bulletDirection *  0.2f, 0.2f, 0.2f);
    }


        if (audioClip != null) {
            _audioSource.PlayOneShot(audioClip, 0.5f);
        }
    }
    public void setGunTypeForPlayer(gunType newGunType)
    {
        if (_gunType == newGunType) return;  

        _gunType = newGunType;
        switch (newGunType)
        {
            case gunType.glock:
            _audioSource.PlayOneShot(_changeToGlockSound);
            break;
            case gunType.bombGun:
            _audioSource.PlayOneShot(_changeToBombGunSound);
            break;
            case gunType.machineGun:
            _audioSource.PlayOneShot(_changeToMachineGunSound);
            break;
            case gunType.awp:
            _audioSource.PlayOneShot(_changeToAwpSound);
            break;
        }
    }

    public gunType getGunType() {
        return _gunType;
    }

    public void TakeDamage(float damage, gunType gunType)
    {
            healhttwo -= damage;
            _audioSource.PlayOneShot(_damagaTakenAudioSource, 0.5f);
            for (int i = 0; i < 7; i++)
            {
                Instantiate(_bloodPrefab, transform.position + new Vector3(-0.3f, 0, 0), Quaternion.identity);
            }
            if (healhttwo <= 0)
            {
                if (!AssetsController._alreadyScored)
                {
                    _player.setScoreP(_player.getScoreP() + 1);
                    AssetsController._alreadyScored = true;
                }
                
            }else
            {
                ApplyKnockback(gunType);
            }
    }
    private void ApplyKnockback(gunType gunType)
    {
        float knockbackStrength;
        switch (gunType)
        {
            case gunType.glock:
                knockbackStrength = 1f; // Minimal knockback
                break;
            case gunType.bombGun:
                knockbackStrength = 3f; // Stronger knockback
                break;
            case gunType.machineGun:
                knockbackStrength = 0.5f; // Continuous fire might mean less knockback per hit
                break;
            case gunType.awp:
                knockbackStrength = 4.0f;
                break;
            default:
                knockbackStrength = 2.0f; // Default value if unspecified
                break;
        }

        Vector2 knockbackDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);
    }
        public void TakeSpikeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            healhttwo -= damage;
            _audioSource.PlayOneShot(_damagaTakenAudioSource, 0.5f);
            if (healhttwo <= 0)
            {
                if (!AssetsController._alreadyScored)
                {
                    _player.setScoreP(_player.getScoreP() + 1);
                    AssetsController._alreadyScored = true;
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
        
        healhttwo += hp;
    
        if (healhttwo > maxHealthtwo) {healhttwo = maxHealthtwo;}
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal2");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        if (xInput < 0.15 && xInput > -0.15) {
            xInput = 0;
        }

        //Vector3 direction = new Vector3(xInput*1.35f, 0, 0);

                if (direction.x < 0.0f)
    { 
        spriteRenderer.flipX = true;
        if (currentGunPrefab != null)
        {
            currentGunPrefab.transform.localScale = new Vector3(-0.17f, 0.17f, 1);
            currentGunPrefab.transform.position = transform.position - new Vector3(0.2f,0.1f,0);
        }
    }
    else if (direction.x > 0.0f)
    {
        spriteRenderer.flipX = false;
        if (currentGunPrefab != null)
        {
            currentGunPrefab.transform.localScale = new Vector3(0.17f, 0.17f, 1);
            currentGunPrefab.transform.position = transform.position + new Vector3(0.2f,-0.1f,0);
        }
    }

        transform.Translate(direction * _speed * Time.deltaTime);
        setBoundaries();
        crouching();
    }

    private void crouching()
    {
        //Input.GetKeyDown(KeyCode.K)
        //yInput < -0.25f
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 scale = transform.localScale;
            scale.y = 0.35f;
            transform.localScale = scale;
        }

        //Input.GetKeyUp(KeyCode.K)
        //yInput > 0.25f
        if (Input.GetKeyUp(KeyCode.K))
        {
            Vector3 scale = transform.localScale;
            scale.y = 0.7f;
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

    public void setScoreSP(int score)
    {
        _score = score;
    }

    public int getScoreSP()
    {
       return _score;
    }
}