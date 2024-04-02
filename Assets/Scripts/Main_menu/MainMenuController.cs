using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _dinoIdle;
    private SpriteRenderer _backgroundRenderer;

    private void Start()
    {
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        StartCoroutine(BackgroundAlphaEffect());
        StartCoroutine(DinoIdleMovement());
    }

    private IEnumerator BackgroundAlphaEffect()
    {
        while (true)
        {
            for (float alpha = 1f; alpha >= 0.6f; alpha -= Time.deltaTime * 0.2f)
            {
                Color newColor = _backgroundRenderer.color;
                newColor.a = alpha;
                _backgroundRenderer.color = newColor;
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            for (float alpha = 0.6f; alpha <= 1f; alpha += Time.deltaTime * 0.2f)
            {
                Color newColor = _backgroundRenderer.color;
                newColor.a = alpha;
                _backgroundRenderer.color = newColor;
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator DinoIdleMovement()
    {
        Vector3 startPosition = _dinoIdle.transform.position;
        float offsetY = 0.1f;
        float speed = 1.0f;

        while (true)
        {
            float newY = Mathf.Sin(Time.time * speed) * offsetY;
            _dinoIdle.transform.position = startPosition + new Vector3(0, newY, 0);
            yield return null;
        }
    }

    public void loadGame() {
           StartCoroutine(StartGameAnimation());
    }

    private IEnumerator StartGameAnimation()
    {
        float duration = 2.0f;
        float timer = 0f;

        Vector3 initialScale = _dinoIdle.transform.localScale;
        Quaternion initialRotation = _dinoIdle.transform.rotation;

        while (timer < duration)
        {
            float scaleFactor = Mathf.Lerp(1f, 2f, timer / duration);
            _dinoIdle.transform.localScale = initialScale * scaleFactor;

            float rotationAngle = Mathf.Lerp(0f, 360f, timer / duration);
            _dinoIdle.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, rotationAngle);

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}
