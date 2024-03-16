using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().setGunTypeForPlayer(gunType.bombGun);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("SecondPlayer"))
        {
            collision.gameObject.GetComponent<SecondPlayer>().setGunTypeForPlayer(gunType.bombGun);
            Destroy(gameObject);
        }
    }
}
