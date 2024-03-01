using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 10.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementCalculation();
    }

    private void movementCalculation()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
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
