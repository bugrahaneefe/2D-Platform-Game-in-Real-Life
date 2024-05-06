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
    private GameObject healthPointPrefab; 
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
        int whatToSpawn = Random.Range(0, 2);
        if (whatToSpawn == 0) {
            SpawnAwpPrefab();

        } else if (whatToSpawn == 1) {
            SpawnHealthPoint();
        }
        Destroy(gameObject,12f);
    }

    private void SpawnHealthPoint()
    {
        if (healthPointPrefab)
        {
            GameObject healthPoint = Instantiate(healthPointPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            Rigidbody2D hpRb = healthPoint.GetComponent<Rigidbody2D>();
            if (hpRb != null)
            {
                hpRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
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
