using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    void Start() {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
        _anim.SetTrigger("OnBombIsExploded");
        _audioSource.Play();
        _rb.simulated = false;
        Destroy(gameObject, 2.5f);
    }
}
