using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15.0f;
    private float _direction = 1f;
    private gunType _gunType;
    private float _size;

    void Start()
    {
        transform.localScale *= _size;
    }

    void Update()
    {
        movementCalculation();
        HittingFloor();
        hittingFirstPlayer();
        hittingSecondPlayer();
    }

    public void SetGunType(gunType gunType) 
    {
        _gunType = gunType;
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    public void SetBulletSize(float size)
    {
        _size = size;
    }

    private void HittingFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.01f);
        if (hit.collider != null) { Destroy(gameObject); }
    }

    private void hittingFirstPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.01f);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (_gunType == gunType.glock) {
                hit.collider.GetComponent<Player>().TakeDamage(1f,gunType.glock);
                Destroy(gameObject); 
            } 
            if (_gunType == gunType.bombGun) {
                hit.collider.GetComponent<Player>().TakeDamage(4f,gunType.bombGun);
                Destroy(gameObject); 
            }
            if (_gunType == gunType.machineGun) {
                hit.collider.GetComponent<Player>().TakeDamage(0.6f,gunType.machineGun);
                Destroy(gameObject); 
            }
            if (_gunType == gunType.awp) {
                hit.collider.GetComponent<Player>().TakeDamage(9f,gunType.awp);
                Destroy(gameObject); 
            } 

        }
    }

    private void hittingSecondPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.01f);
        if (hit.collider != null && hit.collider.CompareTag("SecondPlayer"))
        {
            if (_gunType == gunType.glock) {
                hit.collider.GetComponent<SecondPlayer>().TakeDamage(1f, gunType.glock);
                Destroy(gameObject); 
            } 
            if (_gunType == gunType.bombGun) {
                hit.collider.GetComponent<SecondPlayer>().TakeDamage(4f, gunType.bombGun);
                Destroy(gameObject); 
            }
            if (_gunType == gunType.machineGun) {
                hit.collider.GetComponent<SecondPlayer>().TakeDamage(0.6f, gunType.machineGun);
                Destroy(gameObject); 
            }
            if (_gunType == gunType.awp) {
                hit.collider.GetComponent<SecondPlayer>().TakeDamage(9f,gunType.awp);
                Destroy(gameObject); 
            } 

        }
    }

    private void movementCalculation()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime * _direction);
        
        if (transform.position.x >= 9.257071f)
        {
            Destroy(gameObject);
        }
        if (transform.position.x <= -9.245456f)
        {
            Destroy(gameObject);
        }
    }
}
