using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaler : PlayerComponent, PlayerStatus.OnChangedScale
{
    [Header("Body")]
    [SerializeField]
    private Transform playerBodyTransform;      // 몸 트랜스폼

    [Header("Face")]
    [SerializeField]
    private Transform playerFaceTransform;      // 얼굴 트랜스폼
    [SerializeField]
    private float faceHeight;                   // 얼굴 높이

    [Header("Time")]
    [SerializeField]
    private float changeTime;                   // 스케일 값에 의해 크기가 변하는 시간 

    public void OnChangedScale(float previousScale, float currentScale)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScaleRoutine(currentScale));
    }

    private IEnumerator ChangeScaleRoutine(float targetScale)
    {
        float timer = 0, percent = 0;
        float startScale = playerBodyTransform.localScale.x;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / changeTime;

            float scale = Mathf.Lerp(startScale, targetScale, percent);

            UpdateBodyScale(scale);
            UpdateHeadPosition(scale);

            yield return null;
        }
    }

    private void UpdateBodyScale(float targetScale)
    {
        playerBodyTransform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    private void UpdateHeadPosition(float targetScale)
    {
        float sin = (faceHeight / 2f) / targetScale;       // 높이 / 반지름(빗변)
        float rad = Mathf.Asin(sin);
        float y = Mathf.Cos(rad) * targetScale;

        playerFaceTransform.localPosition = new Vector3(playerFaceTransform.localPosition.x, y, playerFaceTransform.localPosition.z);
    }
}
