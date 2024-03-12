using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlagAnimationController : MonoBehaviour
{
    public GameObject flagPrefab;
    private int currentFlagIndex = 0;

    public void Start()
    {
        StartCoroutine(CreateFlagAnimation());
    }

    private IEnumerator CreateFlagAnimation()
    {
        while (true)
        {
            ClearFlags();
            yield return StartCoroutine(CreateFlag());
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator CreateFlag()
    {
        GameObject newFlag = Instantiate(flagPrefab, transform);
        FlagAnimation flagComponent = newFlag.GetComponent<FlagAnimation>();
        flagComponent.setFlagStatus((FlagStatus)currentFlagIndex);
        currentFlagIndex = (currentFlagIndex + 1) % 9;
        
        yield return new WaitForSeconds(0.1f);
    }

    public void ClearFlags()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
