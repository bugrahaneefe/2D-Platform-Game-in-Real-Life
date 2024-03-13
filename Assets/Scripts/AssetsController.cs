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

    //Bomb logic (prefab)
    [SerializeField] private GameObject _bombPrefab;
    private float _minSpawnDelay = 2f;
    private float _maxSpawnDelay = 5f;
    //Health point login (prefab)
    [SerializeField]
    private GameObject _healthPointPrefab;
    private GameObject currentHealthPoint;
    private float _spawnDelayMin = 5f;
    private float _spawnDelayMax = 15f;   

    void Start()
    {
        _horizontalPlatform.transform.position = new Vector3(-6.26f,0,0);
        _initialPosition = _horizontalPlatform.transform.position;

         StartCoroutine(SpawnBombs());
         StartCoroutine(SpawnHealthPoints());
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
}
