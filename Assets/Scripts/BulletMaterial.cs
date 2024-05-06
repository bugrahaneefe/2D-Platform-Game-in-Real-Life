using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMaterial : MonoBehaviour
{
    // Start is called before the first frame update
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
        if (transform.position.y <= -4.98f)
        {
            Destroy(gameObject);
        }
    }
}
