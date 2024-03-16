using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    private bool canBeCollected = true;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (canBeCollected && collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<Player>().setGunTypeForPlayer(gunType.machineGun);
        canBeCollected = false;
        Destroy(gameObject);
    }

    if (canBeCollected && collision.gameObject.CompareTag("SecondPlayer"))
    {
        collision.gameObject.GetComponent<SecondPlayer>().setGunTypeForPlayer(gunType.machineGun);
        canBeCollected = false;
        Destroy(gameObject);
    }
}
}
