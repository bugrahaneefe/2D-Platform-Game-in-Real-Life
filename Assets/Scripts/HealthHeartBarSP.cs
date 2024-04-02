using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBarSP : MonoBehaviour
{
    public GameObject heartPrefab;
    public SecondPlayer secondPlayer;
    List<HealthHeartSP> hearts = new List<HealthHeartSP>();

    public void Start() {
        drawHearts();
    }

    public void Update() 
    {
        drawHearts();
    }

    public void drawHearts() {
        ClearHeart();
        float maxHealthRemainder = secondPlayer.maxHealthtwo % 2;
        int heartsToMake = (int) ((secondPlayer.maxHealthtwo / 2) + maxHealthRemainder);
        for (int i=0; i<heartsToMake; i++) {
            CreateEmptyHeart();
        }

        for (int i=0; i<hearts.Count; i++) {
            int heartStatusRemainder = (int)Mathf.Clamp(secondPlayer.healhttwo - (i*2), 0, 2);
            hearts[i].setHeartStatus((HeartStatus) heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart(){
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeartSP heartComponent = newHeart.GetComponent<HealthHeartSP>();
        heartComponent.setHeartStatus(HeartStatus.full);
        hearts.Add(heartComponent);
    }
    public void ClearHeart() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeartSP>();
    }
}
