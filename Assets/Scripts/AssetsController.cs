using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsController : MonoBehaviour
{
    //Horizontal Platform Logic 1
    [SerializeField]
    private GameObject _horizontalPlatform; 
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
    private float _spawnDelayMin = 5f;
    private float _spawnDelayMax = 10f; 
    //machine gun (prefab)
    [SerializeField]
    private GameObject _machineGunPrefab;
    //bomb gun (prefab)
    [SerializeField]
    private GameObject _bombGunPrefab;
    private GameObject currentMachineGun;
    private GameObject currentBombGun;

    void Start()
    {
        _horizontalPlatform.transform.position = new Vector3(-2.2f,2.1f,0);
        _initialPosition = _horizontalPlatform.transform.position;

        StartCoroutine(SpawnBombs());
        StartCoroutine(SpawnHealthPoints());
        StartCoroutine(SpawnMachineGun());
        StartCoroutine(SpawnBombGun());
        StartCoroutine(MoveTrapsCoroutine());
    }

    void Update()
    {
        float horizontalMovement = Mathf.Sin(Time.time * _movementSpeed) * _movementRange;

        Vector3 newPosition = _initialPosition + Vector3.right * horizontalMovement;
        _horizontalPlatform.transform.position = newPosition;
    }

    IEnumerator SpawnBombs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
            float randomX = Random.Range(-10f, 10f);
            Instantiate(_bombPrefab, new Vector3(randomX, 8f, 0f), Quaternion.identity);
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

    IEnumerator SpawnMachineGun()
    {
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayMin, _spawnDelayMax));

        if (currentMachineGun != null)
        {
            Destroy(currentMachineGun);
        }

        Vector3 spawnPosition = _greenPlatform.transform.position + new Vector3(0, 1.2f, 0);
        currentMachineGun = Instantiate(_machineGunPrefab, spawnPosition, Quaternion.identity);
        currentMachineGun.transform.SetParent(_greenPlatform.transform);
    }
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

        Vector3 spawnPosition = _grayPlatform3.transform.position + new Vector3(0, 1.2f, 0);
        currentBombGun = Instantiate(_bombGunPrefab, spawnPosition, Quaternion.identity);
        currentBombGun.transform.SetParent(_grayPlatform3.transform);
    }
    }


    IEnumerator MoveTrapsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(3f, 6f);
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
        while (_grayPlatformTrap1.transform.position.y < -0.13f)
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
        while (_grayPlatformTrap1.transform.position.y > -0.37f)
        {
            _grayPlatformTrap1.transform.Translate(Vector3.down * Time.deltaTime);
            _grayPlatformTrap2.transform.Translate(Vector3.down * Time.deltaTime);
            yield return null;
        }

        isMovingUp = true;
        isCoroutineRunning = false;
    }
}
