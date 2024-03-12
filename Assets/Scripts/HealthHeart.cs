using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
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

public enum HeartStatus 
{
    empty = 0,
    full = 1
}