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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.5f);
        if (hit.collider != null) { Destroy(gameObject); }
    }

    private void hittingFirstPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.5f);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            print(_gunType);
            if (_gunType == gunType.glock) {
                hit.collider.GetComponent<Player>().TakeDamage(2f);
                print("glock hit plauer");
                Destroy(gameObject); 
            } 
            if (_gunType == gunType.bombGun) {
                hit.collider.GetComponent<Player>().TakeDamage(4f);
                print("shotgun hit plauer");
                Destroy(gameObject); 
            }
            if (_gunType == gunType.machineGun) {
                hit.collider.GetComponent<Player>().TakeDamage(1);
                print("shotgun hit plauer");
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
