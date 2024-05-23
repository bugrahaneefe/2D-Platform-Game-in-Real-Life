using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsController : MonoBehaviour
{
    static public bool _alreadyScored = false;
    //Horizontal Platform Logic 1
    [SerializeField]
    private GameObject _horizontalPlatform; 
    [SerializeField]
    private GameObject _hpCrate; 
    [SerializeField]
    private GameObject _redPlanePrefab;
    [SerializeField]
    private Sprite redPlaneSprite;  // Assign this in the Inspector
    [SerializeField]
    private Sprite bluePlaneSprite;
    private float _movementRange = 3f; 
    private float _movementSpeed = 1.5f; 
    private Vector3 _initialPosition;
    //Green Platform Logic 1
    [SerializeField]
    private GameObject _greenPlatform;
    //Gray Platform Logic 1
    [SerializeField]
    private GameObject _grayPlatformTrap1;
    //Gray Platform Logic 2
    [SerializeField]
    private GameObject _grayPlatformTrap2;
    //Gray Platform Logic 3
    [SerializeField]
    private GameObject _grayPlatform3;
    private bool isMovingUp = true;
    private bool isCoroutineRunning = false;

    //Bomb logic (prefab)
    [SerializeField] private GameObject _bombPrefab;
    private float _minSpawnDelay = 5f;
    private float _maxSpawnDelay = 15f;
    //Health point login (prefab)
    [SerializeField]
    private GameObject _healthPointPrefab;
    private GameObject currentHealthPoint;
    [SerializeField]
    private GameObject _healthPointPrefab2;
    private GameObject currentHealthPoint2;
    private float _spawnDelayMin = 5f;
    private float _spawnDelayMax = 10f; 
    //machine gun (prefab)
    [SerializeField]
    private GameObject _machineGunPrefab;
    //awp gun (prefab)
    [SerializeField]
    private GameObject _awpPrefab;
    [SerializeField]
    private GameObject _awpPrefabPlatform;
    //bomb gun (prefab)
    [SerializeField]
    private GameObject _bombGunPrefab;
    //chest (prefab)
    [SerializeField]
    private GameObject _chestPrefab;
    private GameObject currentAwp;
    private GameObject currentMachineGun;
    private GameObject currentBombGun;

    void Start()
    {
        _horizontalPlatform.transform.position = new Vector3(1.6f,4.7f,0);
        _horizontalPlatform.transform.localScale = new Vector3(1f , 0.5f, 0.5f);
        _initialPosition = _horizontalPlatform.transform.position;

        StartCoroutine(SpawnHealthPoints());
        StartCoroutine(SpawnHealthPointsPositionTwo());
        StartCoroutine(SpawnMachineGun());
        StartCoroutine(SpawnBombGun());
        StartCoroutine(MoveTrapsCoroutine());
        StartCoroutine(SpawnRedPlane());
        StartCoroutine(SpawnAwp());
    }

    void Update()
    {
        float horizontalMovement = Mathf.Sin(Time.time * _movementSpeed) * _movementRange;

        Vector3 newPosition = _initialPosition + Vector3.right * horizontalMovement;
        _horizontalPlatform.transform.position = newPosition;
    }


    IEnumerator SpawnRedPlane()
{
    while (true)
    {
        yield return new WaitForSeconds(Random.Range(10, _maxSpawnDelay));
        float randomY = Random.Range(5.75f, 6.5f);
        bool spawnFromLeft = Random.Range(0, 2) == 0;
        float spawnX = spawnFromLeft ? -20.5f : 20.5f;  
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);
        Quaternion rotation = spawnFromLeft ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObject newPlane = Instantiate(_redPlanePrefab, spawnPosition, rotation);

        SpriteRenderer planeSpriteRenderer = newPlane.GetComponent<SpriteRenderer>();

        if (Random.Range(0, 2) == 0)
        {
            planeSpriteRenderer.sprite = bluePlaneSprite;
        }
        else
        {
            planeSpriteRenderer.sprite = redPlaneSprite;
        }

        yield return new WaitForSeconds(Random.Range(3f, 5f)); 

        if (Random.Range(0, 4) == 0)
        {
            Instantiate(_bombPrefab, new Vector3(newPlane.transform.position.x, newPlane.transform.position.y - 0.5f, newPlane.transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(_chestPrefab, new Vector3(newPlane.transform.position.x, newPlane.transform.position.y - 0.5f, newPlane.transform.position.z), Quaternion.identity);
        }
        
        Destroy(newPlane, 7f); 
    }
}

    IEnumerator SpawnHealthPoints()
    {
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));

        if (currentHealthPoint != null)
        {
            Destroy(currentHealthPoint);
        }

        Vector3 spawnPosition = _horizontalPlatform.transform.position + new Vector3(0, 0.5f, 0);
        currentHealthPoint = Instantiate(_healthPointPrefab, spawnPosition, Quaternion.identity);
        currentHealthPoint.transform.SetParent(_horizontalPlatform.transform);
    }
    }

    IEnumerator SpawnHealthPointsPositionTwo()
    {
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));

        if (currentHealthPoint2 != null)
        {
            Destroy(currentHealthPoint2);
        }

        Vector3 spawnPosition = _hpCrate.transform.position + new Vector3(0, 0.5f, 0);
        currentHealthPoint2 = Instantiate(_healthPointPrefab2, spawnPosition, Quaternion.identity);
        currentHealthPoint2.transform.SetParent(_hpCrate.transform);
    }
    }

    IEnumerator SpawnAwp()
    {
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));

        if (currentAwp != null)
        {
            Destroy(currentAwp);
        }

        Vector3 spawnPosition = _awpPrefabPlatform.transform.position + new Vector3(0, 1f, 0);
        currentAwp = Instantiate(_awpPrefab, spawnPosition, Quaternion.identity);
        currentAwp.transform.SetParent(_awpPrefab.transform);
    }
    }

    IEnumerator SpawnMachineGun()
{
    while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));  

        if (currentMachineGun != null)
        {
            Destroy(currentMachineGun);
        }

        Vector3 spawnPosition = _greenPlatform.transform.position + new Vector3(0, -0.25f, 0);
        currentMachineGun = Instantiate(_machineGunPrefab, spawnPosition, Quaternion.identity);
        
        yield return StartCoroutine(MoveMachineGunUp(currentMachineGun, _greenPlatform.transform.position + new Vector3(0, 0.5f, 0)));

        yield return new WaitForSeconds(5f); 

        Destroy(currentMachineGun);
    }
}

IEnumerator MoveMachineGunUp(GameObject machineGun, Vector3 targetPosition)
{
    while (machineGun.transform.position.y < targetPosition.y)
    {
        machineGun.transform.Translate(Vector3.up * Time.deltaTime);
        yield return null;
    }

    machineGun.transform.SetParent(_greenPlatform.transform); 
}


    IEnumerator SpawnBombGun()
    {
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));

        if (currentBombGun != null)
        {
            Destroy(currentBombGun);
        }

        Vector3 spawnPosition = _grayPlatform3.transform.position + new Vector3(0, 0.7f, 0);
        currentBombGun = Instantiate(_bombGunPrefab, spawnPosition, Quaternion.identity);
        currentBombGun.transform.SetParent(_grayPlatform3.transform);
    }
    }


    IEnumerator MoveTrapsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(1f, 4f);
            yield return new WaitForSeconds(delay);

            if (!isCoroutineRunning)
            {
                isCoroutineRunning = true;
                if (isMovingUp)
                {
                    StartCoroutine(MoveTrapUp());
                }
                else
                {
                    StartCoroutine(MoveTrapDown());
                }
            }
        }
    }

    IEnumerator MoveTrapUp()
    {
        while (_grayPlatformTrap1.transform.position.y < -1.85f)
        {
            _grayPlatformTrap1.transform.Translate(Vector3.up * Time.deltaTime);
            _grayPlatformTrap2.transform.Translate(Vector3.up * Time.deltaTime);
            yield return null;
        }

        isMovingUp = false;
        isCoroutineRunning = false;
    }

    IEnumerator MoveTrapDown()
    {
        while (_grayPlatformTrap1.transform.position.y > -2f)
        {
            _grayPlatformTrap1.transform.Translate(Vector3.down * Time.deltaTime);
            _grayPlatformTrap2.transform.Translate(Vector3.down * Time.deltaTime);
            yield return null;
        }

        isMovingUp = true;
        isCoroutineRunning = false;
    }
}
