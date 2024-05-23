using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awp_silencer : MonoBehaviour
{
    private bool _canBeCollected = true;

    void Start()
    {
    }

    void Update()
    {
        
    }
    public void setAwpCanBeCollected(bool canBeCollected) {
        _canBeCollected = canBeCollected;   
    }
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (_canBeCollected && collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<Player>().setGunTypeForPlayer(gunType.awp);
        _canBeCollected = false;
        Destroy(gameObject);
    }

    if (_canBeCollected && collision.gameObject.CompareTag("SecondPlayer"))
    {
        collision.gameObject.GetComponent<SecondPlayer>().setGunTypeForPlayer(gunType.awp);
        _canBeCollected = false;
        Destroy(gameObject);
    }

    _canBeCollected = false;
}
}
