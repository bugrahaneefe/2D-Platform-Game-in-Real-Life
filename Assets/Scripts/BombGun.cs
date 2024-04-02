using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGun : MonoBehaviour
{
    private bool _canBeCollected = true;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void setBombGunCanBeCollected(bool canBeCollected) {
        _canBeCollected = canBeCollected;   
    }
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (_canBeCollected && collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<Player>().setGunTypeForPlayer(gunType.bombGun);
        _canBeCollected = false;
        Destroy(gameObject);
    }

    if (_canBeCollected && collision.gameObject.CompareTag("SecondPlayer"))
    {
        collision.gameObject.GetComponent<SecondPlayer>().setGunTypeForPlayer(gunType.bombGun);
        _canBeCollected = false;
        Destroy(gameObject);
    }
}
}
