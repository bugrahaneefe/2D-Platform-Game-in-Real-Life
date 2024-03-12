using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 15.0f;
    private float _direction = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementCalculation();
        hittingFloor();
        hittingFirstPlayer();
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    private void hittingFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.5f);
        if (hit.collider != null) { Destroy(gameObject); }
    }

    private void hittingFirstPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _direction, 0.5f);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<Player>().TakeDamage(1);
            Destroy(gameObject);
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
