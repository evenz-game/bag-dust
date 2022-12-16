using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class ObstacleDangerUI : MonoBehaviour
{
    public UnityEvent onDisabledUI = new UnityEvent();

    [Header("Twinkle")]
    [SerializeField]
    private float minAlpha = 0.3f;
    [SerializeField]
    private float maxAlpha = 0.8f;
    [SerializeField]
    private float twinkleTime = 2;

    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 2;

    [Header("Time")]
    [SerializeField]
    private float enableTime = 1;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(TwinkleRoutine());
        StartCoroutine(MoveRoutine());
    }

    private void Disable()
    {
        onDisabledUI.Invoke();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator TwinkleRoutine()
    {
        float startTime = Time.time;
        while (true)
        {
            yield return StartCoroutine(TwinkleUnitRoutine(minAlpha, maxAlpha, twinkleTime / 2f));
            yield return StartCoroutine(TwinkleUnitRoutine(maxAlpha, minAlpha, twinkleTime / 2f));

            if (Time.time - startTime >= enableTime)
                break;
        }

        Disable();
    }

    private IEnumerator TwinkleUnitRoutine(float startAlpha, float endAlpha, float time)
    {
        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / time;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, percent);
            Color newColor = spriteRenderer.color;
            newColor.a = newAlpha;

            spriteRenderer.color = newColor;

            yield return null;
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            spriteRenderer.size += Vector2.right * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
