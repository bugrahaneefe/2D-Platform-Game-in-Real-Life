using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeartSP : MonoBehaviour
{
    public Sprite fullHeart, emptyHeart;
    Image heartImage;
    void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void setHeartStatus(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.full:
                heartImage.sprite = fullHeart;
                break; 
        }
    }
}