using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    [SerializeField]
    private AudioClip openSound;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _healthPointPrefab; 

    [SerializeField]
    private GameObject _coinSmallerPrefab; 
    [SerializeField]
    private GameObject _bombPrefab;
    [SerializeField]
    private GameObject _gemPrefab;

    [SerializeField]
    private GameObject _awpPrefab; 
    public float bounceForce = 5f; 
    private bool isOpened = false;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("SecondPlayer")) && !isOpened)
        {
            isOpened = true;
            OpenChest();
        }
    }

    private void OpenChest()
    {
        _anim.SetTrigger("player_opens");

        if (openSound && _audioSource)
        {
            _audioSource.PlayOneShot(openSound);
        }
        switch (Random.Range(0, 10))
        {
            case 0:
                SpawnAwpPrefab();
                break;
            case 1:
                SpawnHealthPoint();
                break;
            case 2:
                SpawnCoinPrefab();
                break;
            case 3:
                SpawnBombPrefab();
                break;
            case 4:
                SpawnGemPrefab();
                break;
            case 5:
                SpawnAwpPrefab();
                break;
            case 6:
                SpawnBombPrefab();
                break;
            case 7:
                SpawnGemPrefab();
                break;
            case 8:
                SpawnBombPrefab();
                break;
            case 9:
                SpawnAwpPrefab();
                break;
        }
        Destroy(gameObject,12f);
    }

    private void SpawnGemPrefab()
    {
        if (_gemPrefab)
        {
            GameObject gem = Instantiate(_gemPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Rigidbody2D gRb = gem.GetComponent<Rigidbody2D>();
            if (gRb != null)
            {
                gRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
    private void SpawnHealthPoint()
    {
        if (_healthPointPrefab)
        {
            GameObject healthPoint = Instantiate(_healthPointPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Rigidbody2D hpRb = healthPoint.GetComponent<Rigidbody2D>();
            if (hpRb != null)
            {
                hpRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnCoinPrefab()
    {
        if (_coinSmallerPrefab)
        {
            GameObject coinSmaller = Instantiate(_coinSmallerPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Rigidbody2D csRb = coinSmaller.GetComponent<Rigidbody2D>();
            if (csRb != null)
            {
                csRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnBombPrefab()
    {

        if (_bombPrefab)
        {
            GameObject bomb = Instantiate(_bombPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Rigidbody2D bRb = bomb.GetComponent<Rigidbody2D>();
            if (bRb != null)
            {
                bRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnAwpPrefab()
    {
        if (_awpPrefab)
        {
            GameObject awp = Instantiate(_awpPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        }
    }
}
