using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_smaller : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player playeroneScript = collision.gameObject.GetComponent<Player>();
            if (playeroneScript != null)
            {
                playeroneScript.setScoreP(playeroneScript.getScoreP() + 1);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("SecondPlayer")) {
            SecondPlayer playertwoScript = collision.gameObject.GetComponent<SecondPlayer>();
            if (playertwoScript != null)
            {
                playertwoScript.setScoreSP(playertwoScript.getScoreSP() + 1);
            }
            Destroy(gameObject);
        }
    }
}
