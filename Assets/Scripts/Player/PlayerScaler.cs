using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaler : MonoBehaviour, PlayerStatus.OnChangedScale
{
    [Header("Body")]
    [SerializeField]
    private Transform playerBodyTransform;      // 몸 트랜스폼

    [Header("Face")]
    [SerializeField]
    private Transform playerFaceTransform;                  // 얼굴 트랜스폼
    [SerializeField]
    private Vector3 minFaceLocalPosition;      // 스케일 1일 때 얼굴 로컬 위치
    [SerializeField]
    private Vector3 maxFaceLocalPosition;      // 스케일 1일 때 얼굴 로컬 위치

    [SerializeField]
    private float faceHeight;                   // 얼굴 높이

    public void OnChangedScale(float previousScale, float currentScale)
    {
        UpdateBodyScale(currentScale);
        UpdateHeadPosition(currentScale);
    }

    private void UpdateBodyScale(float currentScale)
    {
        playerBodyTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void UpdateHeadPosition(float currentScale)
    {
        float sin = (faceHeight / 2f) / currentScale;       // 높이 / 반지름(빗변)
        float rad = Mathf.Asin(sin);
        float y = Mathf.Cos(rad) * currentScale;

        playerFaceTransform.localPosition = new Vector3(0, -y, 0);
    }
}
