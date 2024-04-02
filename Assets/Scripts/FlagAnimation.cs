using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagAnimation : MonoBehaviour
{
    public Sprite flag1, flag2, flag3, flag4, flag5, flag6, flag7, flag8, flag9;
    Image flagImage;

    void Awake() 
    {
        flagImage = GetComponent<Image>();
    }  

    public void setFlagStatus(FlagStatus status)
    {
        switch (status)
        {
            case FlagStatus.first:
                flagImage.sprite = flag1;
                break;
            case FlagStatus.second:
                flagImage.sprite = flag2;
                break;
            case FlagStatus.third:
                flagImage.sprite = flag3;
                break;
            case FlagStatus.fourth:
                flagImage.sprite = flag4;
                break; 
            case FlagStatus.fifth:
                flagImage.sprite = flag5;
                break;
            case FlagStatus.sixth:
                flagImage.sprite = flag6;
                break; 
            case FlagStatus.seventh:
                flagImage.sprite = flag7;
                break;
            case FlagStatus.eighth:
                flagImage.sprite = flag8;
                break; 
            case FlagStatus.ninth:
                flagImage.sprite = flag9;
                break;
        }
    }
}

public enum FlagStatus 
{
    first = 0,
    second = 1,
    third = 2,
    fourth  = 3,
    fifth = 4,
    sixth = 5,
    seventh = 6,
    eighth = 7,
    ninth = 8
}
