using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Player player;
    List<HealthHeart> hearts = new List<HealthHeart>();

    public void Start() {
        drawHearts();
    }

    public void Update() 
    {
        drawHearts();
    }

    public void drawHearts() {
        ClearHeart();
        float maxHealthRemainder = player.maxHealth % 2;
        int heartsToMake = (int) ((player.maxHealth / 2) + maxHealthRemainder);

        for (int i=0; i<heartsToMake; i++) {
            CreateEmptyHeart();
        }

        for (int i=0; i<hearts.Count; i++) {
            int heartStatusRemainder = (int)Mathf.Clamp(player.health - (i*2), 0, 2);
            hearts[i].setHeartStatus((HeartStatus) heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart(){
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.setHeartStatus(HeartStatus.full);
        hearts.Add(heartComponent);
    }
    public void ClearHeart() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
}
