using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetHealth(2);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("SecondPlayer"))
        {
            collision.gameObject.GetComponent<SecondPlayer>().GetHealth(2);

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorizontalPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }
}