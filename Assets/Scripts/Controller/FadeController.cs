using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeController : MonoBehaviour
{
    private enum FadeType { In, Out }

    [SerializeField]
    private Color defaultColorAtAwake;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private FadeInfo[] fadeInfoPresets;

    private void Awake()
    {
        fadeImage.color = defaultColorAtAwake;
    }

    public void FadeInWhite(float fadeTime)
    {
        StartFade(new FadeInfo(fadeTime, 1, 0, Color.white));
    }

    public void FadeOutWhite(float fadeTime)
    {
        StartFade(new FadeInfo(fadeTime, 0, 1, Color.white));
    }

    public void FadeInBlack(float fadeTime)
    {
        StartFade(new FadeInfo(fadeTime, 1, 0, Color.black));
    }

    public void FadeOutBlack(float fadeTime)
    {
        StartFade(new FadeInfo(fadeTime, 0, 1, Color.black));
    }

    public void StartFadeByName(string targetFadeName)
    {
        foreach (FadeInfo info in fadeInfoPresets)
        {
            if (info.FadeInfoName == targetFadeName)
            {
                StartFade(info);
                return;
            }
        }
    }

    private bool isFading = false;
    public void StartFade(FadeInfo fadeInfo)
    {
        if (isFading) return;

        isFading = true;
        StartCoroutine(FadeRoutine(fadeInfo));
    }

    private IEnumerator FadeRoutine(FadeInfo fadeInfo)
    {
        // 시작 딜레이
        yield return new WaitForSeconds(fadeInfo.delayTimeAtStart);

        // 이미지 색 변경
        fadeImage.color = fadeInfo.fadeColor;

        float timer = 0, percent = 0;

        // 이미지 알파 변경
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / fadeInfo.fadeTime;

            float newAlpha = Mathf.Lerp(fadeInfo.startAlpha, fadeInfo.endAlpha, fadeInfo.fadeCurve.Evaluate(percent));
            Color newColor = fadeImage.color;
            newColor.a = newAlpha;
            fadeImage.color = newColor;

            yield return null;
        }

        // 끝 딜레이
        yield return new WaitForSeconds(fadeInfo.delayTimeAtEnd);

        // 페이드 완료 이벤트 호출
        fadeInfo.onFinishedFade.Invoke();

        isFading = false;
    }
}

[Serializable]
public class FadeInfo
{
    public string FadeInfoName;
    public Color fadeColor = Color.white;
    [Range(0, 1)]
    public float startAlpha;
    [Range(0, 1)]
    public float endAlpha;
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float fadeTime;
    public float delayTimeAtStart = 0;
    public float delayTimeAtEnd = 0;

    public UnityEvent onFinishedFade = new UnityEvent();

    public FadeInfo(float fadeTime, float startAlpha, float endAlpha, Color fadeColor)
    {
        this.fadeTime = fadeTime;
        this.startAlpha = startAlpha;
        this.endAlpha = endAlpha;
        this.fadeColor = fadeColor;
    }
}
